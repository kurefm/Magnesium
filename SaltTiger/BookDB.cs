using Magnesium.Core.Extension;
using SaltTiger.Model;
using Magnesium.Core.DataBase.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magnesium.Core.Log;

namespace SaltTiger
{
    public class BookDB
    {
        #region SQL Script

        private const string CREATE_TABLES = @"
            BEGIN TRANSACTION;
            CREATE TABLE IF NOT EXISTS `tag` (
                `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                `name`	TEXT NOT NULL UNIQUE
            );
            CREATE TABLE IF NOT EXISTS `hyperlink` (
                `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                `name`	TEXT NOT NULL,
                `href`	TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS `category` (
                `id`	INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                `name`	TEXT NOT NULL UNIQUE
            );
            CREATE TABLE IF NOT EXISTS `book_tag` (
                `book_id`	INTEGER NOT NULL,
                `tag_id`	INTEGER NOT NULL,
                PRIMARY KEY(book_id, tag_id),
                FOREIGN KEY(`book_id`) REFERENCES book(id ),
                FOREIGN KEY(`tag_id`) REFERENCES tag(id )
            );
            CREATE TABLE IF NOT EXISTS `book_href` (
                `book_id`	    INTEGER NOT NULL,
                `hyperlink_id`	INTEGER NOT NULL,
                PRIMARY KEY(book_id, hyperlink_id),
                FOREIGN KEY(`book_id`) REFERENCES book(id),
                FOREIGN KEY(`hyperlink_id`) REFERENCES hyperlink(id)
            );
            CREATE TABLE IF NOT EXISTS `book_categoy` (
                `book_id`	    INTEGER NOT NULL,
                `category_id`	INTEGER NOT NULL,
                PRIMARY KEY(book_id, category_id),
                FOREIGN KEY(`book_id`) REFERENCES book(id),
                FOREIGN KEY(`category_id`) REFERENCES category(id)
            );
            CREATE TABLE IF NOT EXISTS `book` (
                `id`	            INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                `name`	            TEXT NOT NULL,
                `publish_date`	    TEXT NOT NULL,
                `official_href`	TEXT NOT NULL,
                `introduction`      TEXT
            );
            COMMIT;
            ";

        //private const string INSERT_BOOK = "insert into ";

        #endregion

        private Sqlite3 DB;

        //缓存在内存中的分类项和标签项
        private Dictionary<string, long> CategoryDict = new Dictionary<string, long>();
        private Dictionary<string, long> TagDict = new Dictionary<string, long>();

        public string FilePath { get; private set; }

        public BookDB(string filePath = @"\BookDB.sqlite3")
        {
            FilePath = filePath;
            Logger.Debug($"Connect to database: {filePath}");
            DB = new Sqlite3(FilePath);
            CreateTables();
        }

        private void CreateTables()
        {
            Logger.Debug("Create table");
            DB.ExecuteNonQuery(CREATE_TABLES);
        }

        public long Insert(Book book)
        {
            //添加book的基本信息
            var bookId = DB.ExecuteScalar<long>("insert into book values(NULL,?,date(?),?,?); select last_insert_rowid()",
                book.Name,
                book.PublishDate,
                book.OfficialHref,
                book.Introduction);
            //添加book的标签、分类和超链接
            book.Hrefs.ForEach(href => AddHrefToBook(bookId, href));
            book.Categorys.ForEach(category => AddCategoryToBook(bookId, category));
            book.Tags.ForEach(tag => AddTagToBook(bookId, tag));
            Logger.Debug($"Insert book {book.Name} successful");
            return bookId;
        }

        public Book GetBook(long bookId)
        {
            using (var r = DB.ExecuteReaderOne("select * from book where book.id=?", bookId))
            {
                if (r == null)
                {
                    return null;
                }
                var book = new Book(r.GetString(1), r.GetDateTime(2), r.GetString(3)) { Id = r.GetInt64(0) };

                book.Hrefs.AddRange(GetHrefs(book.Id));
                book.Categorys.AddRange(GetCategorys(book.Id));
                book.Tags.AddRange(GetTags(book.Id));

                return book;
            }
        }

        public long AddHyperlink(Hyperlink hyperlink)
        {
            return DB.ExecuteScalar<long>("insert into hyperlink values(NULL,?,?); select last_insert_rowid()", hyperlink.Name, hyperlink.Href);
        }

        /// <summary>
        /// 往数据库中添加一个分类
        /// </summary>
        /// <param name="category">分类名称</param>
        /// <returns>该分类在数据库中的ID</returns>
        public long AddCategory(string category)
        {
            var id = DB.ExecuteScalar<long>("insert into category values(NULL,?); select last_insert_rowid()", category);
            CategoryDict[category] = id;
            return id;
        }

        /// <summary>
        /// 往数据库中添加一个标签
        /// </summary>
        /// <param name="tag">标签名称</param>
        /// <returns>该标签在数据库中的ID</returns>
        public long AddTag(string tag)
        {
            var id = DB.ExecuteScalar<long>("insert into tag values(NULL,?); select last_insert_rowid()", tag);
            TagDict[tag] = id;
            return id;
        }

        /// <summary>
        /// 获取分类在数据库中的ID, 若数据库中不存在该分类则添加它
        /// </summary>
        /// <param name="category">分类名称</param>
        /// <returns>分类在数据库中的ID</returns>
        public long GetCategoryId(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                throw new ArgumentNullException("category");
            }
            //查找字典
            long id;
            if (CategoryDict.TryGetValue(category, out id))
            {
                return id;
            }
            //查找数据库
            id = DB.ExecuteScalar<long>("select id from category where name=?", category);
            if (id != 0)
            {
                return id;
            }
            //前2步没有返回结果，说明内容不存在数据库中
            return AddCategory(category);
        }

        /// <summary>
        /// 获取标签在数据库中的ID, 若数据库中不存在该标签则添加它
        /// </summary>
        /// <param name="tag">标签名称</param>
        /// <returns>标签在数据库中的ID</returns>
        public long GetTagId(string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
            {
                throw new ArgumentNullException("tag");
            }
            long id;
            if (CategoryDict.TryGetValue(tag, out id))
            {
                return id;
            }
            id = DB.ExecuteScalar<long>("select id from tag where name=?", tag);
            if (id != 0)
            {
                return id;
            }
            return AddTag(tag);
        }

        public void AddHrefToBook(long bookId, Hyperlink hyperlink)
        {
            DB.Execute("insert into book_href values(?,?)", bookId, AddHyperlink(hyperlink));
        }

        public void AddCategoryToBook(long bookId, string category)
        {
            DB.Execute("insert into book_categoy values(?,?)", bookId, GetCategoryId(category));
        }

        public void AddTagToBook(long bookId, string tag)
        {
            DB.Execute("insert into book_tag values(?,?)", bookId, GetTagId(tag));
        }

        public IEnumerable<Hyperlink> GetHrefs(long bookId)
        {
            return DB.ExecuteReader("select href,name from book_href,hyperlink where book_href.book_id=? and book_href.hyperlink_id=hyperlink.id", bookId)
                     .ForEach(reader => new Hyperlink(reader.GetString(0), reader.GetString(1)));
        }

        public IEnumerable<string> GetCategorys(long bookId)
        {
            return DB.ExecuteReader("select name from book_categoy,category where book_categoy.book_id=? and book_categoy.category_id=category.id", bookId)
                     .ForEach(reader => reader.GetString(0));
        }

        public IEnumerable<string> GetTags(long bookId)
        {
            return DB.ExecuteReader("select name from book_tag,tag where book_tag.book_id=? and book_tag.tag_id=tag.id", bookId)
                     .ForEach(reader => reader.GetString(0));
        }
    }
}
