using Magnesium.DataBase.Sqlite;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.DataBase.XMusic
{
    public class XMusicDB
    {
        #region SQL Script

        private const string CREATE_TABLE =
            @"BEGIN TRANSACTION;
            CREATE TABLE IF NOT EXISTS `song` (
                `id`            INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                `x_id`          INTEGER NOT NULL UNIQUE,
                `title`         TEXT NOT NULL,
                `subtitle`      TEXT,
                `x_artist_id`   INTEGER NOT NULL,
                `x_album_id`    INTEGER NOT NULL,
                `track`	        INTEGER NOT NULL,
                `lyricist`      TEXT,
                `composer`      TEXT,
                `arranger`      TEXT,
                `listen_count`  INTEGER,
                `share_count`   INTEGER,
                `comment_count` INTEGER,                
                `disc_num`      INTEGER NOT NULL,
                `last_update`   TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS `album` (
                `id`            INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                `x_id`          INTEGER NOT NULL UNIQUE,
                `title`         TEXT NOT NULL,
                `subtitle`      TEXT,
                `img_path`      TEXT,
                `rating`        REAL,
                `x_artist_id`   INTEGER NOT NULL,
                `language`      TEXT,
                `publisher`     TEXT,                
                `publish_date`	TEXT,
                `media_type`	TEXT,
                `genre`         TEXT,
                `listen_count`  INTEGER,
                `share_count`   INTEGER,
                `comment_count` INTEGER,
                `like_count`    INTEGER,
                `disc_count`	INTEGER NOT NULL,
                `introduction`  TEXT,
                `last_update`   TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS `artist` (
                `id`            INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE,
                `x_id`          INTEGER NOT NULL UNIQUE,
                `name`          TEXT NOT NULL,
                `alias`         TEXT,
                `area`          TEXT,
                `genre`         TEXT,
                `profile`	    TEXT,
                `listen_count`  INTEGER,
                `share_count`   INTEGER,
                `comment_count` INTEGER,
                `fans_count`    INTEGER,
                `last_update`   TEXT NOT NULL
            );
            CREATE INDEX IF NOT EXISTS `song_title_index` ON `song` (`title` ASC);
            CREATE INDEX IF NOT EXISTS `artist_name_index` ON `artist` (`name` ASC);
            CREATE INDEX IF NOT EXISTS `album_title_index` ON `album` (`title` ASC);
            COMMIT;";

        // 使用replace避免插入重复数据时出错。
        private const string REPLACE_ALBUM = "replace into album values(NULL,@XId,@Title,@Subtitle,@ImagePath,@Rating,@XArtistId,@Language,@Publisher,date(@PublishDate),@MediaType,@Genre,@ListenCount,@ShareCount,@CommentCount,@LikeCount,@DiscCount,@Introduction,@LastUpdate)";
        private const string REPLACE_SONG = "replace into song values(NULL,@XId,@Title,@Subtitle,@XArtistId,@XAlbumId,@Track,@Lyricist,@Composer,@Arranger,@ListenCount,@ShareCount,@CommentCount,@DiscNumber,@LastUpdate)";
        private const string REPLACE_ARTIST = "replace into artist values(NULL,@XId,@Name,@Alias,@Area,@Genre,@Profile,@ListenCount,@ShareCount,@CommentCount,@FansCount,@LastUpdate)";

        #endregion

        private Sqlite3 db;


        public string FilePath { get; private set; } = "XMusic.db";


        public XMusicDB()
        {
            db = new Sqlite3(FilePath);
            CreateTables();
        }

        public XMusicDB(string path)
        {
            FilePath = path;
            db = new Sqlite3(path);
            CreateTables();
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        private void CreateTables()
        {
            db.ExecuteNonQuery(CREATE_TABLE);
        }

        /// <summary>
        /// 插入一个Album对象
        /// </summary>
        /// <param name="album"></param>
        /// <returns></returns>
        public bool Insert(Album album)
        {
            //int result = db.ExecuteNonQuery("replace into album values(NULL,@XId,@Title,@Subtitle,@ImagePath,@Rating,@XArtistId,@Language,@Publisher,date(@PublishDate),@MediaType,@Genre,@ListenCount,@ShareCount,@CommentCount,@LikeCount,@DiscCount,@Introduction,@LastUpdate)",
            //        new SQLiteParameter("@XId", album.XId),
            //        new SQLiteParameter("@Title", album.Title),
            //        new SQLiteParameter("@Subtitle", album.Subtitle),
            //        new SQLiteParameter("@ImagePath", album.ImagePath),
            //        new SQLiteParameter("@Rating", album.Rating),
            //        new SQLiteParameter("@XArtistId", album.XArtistId),
            //        new SQLiteParameter("@Language", album.Language),
            //        new SQLiteParameter("@Publisher", album.Publisher),
            //        new SQLiteParameter("@PublishDate", album.PublishDate),
            //        new SQLiteParameter("@MediaType", album.MediaType),
            //        new SQLiteParameter("@Genre", album.Genre),
            //        new SQLiteParameter("@ListenCount", album.ListenCount),
            //        new SQLiteParameter("@ShareCount", album.ShareCount),
            //        new SQLiteParameter("@CommentCount", album.CommentCount),
            //        new SQLiteParameter("@LikeCount", album.LikeCount),
            //        new SQLiteParameter("@DiscCount", album.DiscCount),
            //        new SQLiteParameter("@Introduction", album.Introduction),
            //        new SQLiteParameter("@LastUpdate", album.LastUpdate)
            //    );

            int result = db.Execute("replace into album values(NULL,?,?,?,?,?,?,?,?,date(?),?,?,?,?,?,?,?,?,?)",
                    album.XId,
                    album.Title,
                    album.Subtitle,
                    album.ImagePath,
                    album.Rating,
                    album.XArtistId,
                    album.Language,
                    album.Publisher,
                    album.PublishDate,
                    album.MediaType,
                    album.Genre,
                    album.ListenCount,
                    album.ShareCount,
                    album.CommentCount,
                    album.LikeCount,
                    album.DiscCount,
                    album.Introduction,
                    album.LastUpdate
                );

            return result == 1;
        }

        /// <summary>
        /// 插入一组Album对象
        /// </summary>
        /// <param name="albums"></param>
        /// <returns></returns>
        public bool Insert(IEnumerable<Album> albums)
        {
            var paras = from album in albums
                        select new SQLiteParameter[]{
                            new SQLiteParameter("@XId", album.XId),
                            new SQLiteParameter("@Title", album.Title),
                            new SQLiteParameter("@Subtitle", album.Subtitle),
                            new SQLiteParameter("@ImagePath", album.ImagePath),
                            new SQLiteParameter("@Rating", album.Rating),
                            new SQLiteParameter("@XArtistId", album.XArtistId),
                            new SQLiteParameter("@Language", album.Language),
                            new SQLiteParameter("@Publisher", album.Publisher),
                            new SQLiteParameter("@PublishDate", album.PublishDate),
                            new SQLiteParameter("@MediaType", album.MediaType),
                            new SQLiteParameter("@Genre", album.Genre),
                            new SQLiteParameter("@ListenCount", album.ListenCount),
                            new SQLiteParameter("@ShareCount", album.ShareCount),
                            new SQLiteParameter("@CommentCount", album.CommentCount),
                            new SQLiteParameter("@LikeCount", album.LikeCount),
                            new SQLiteParameter("@DiscCount", album.DiscCount),
                            new SQLiteParameter("@Introduction", album.Introduction),
                            new SQLiteParameter("@LastUpdate", album.LastUpdate)
                        };
            var result = db.ExecuteNonQueryRange(REPLACE_ALBUM, paras);

            return albums.Count() == result;
        }

        public bool Insert(Song song)
        {
            var result = db.ExecuteNonQuery(REPLACE_SONG,
                    new SQLiteParameter("@XId", song.XId),
                    new SQLiteParameter("@Title", song.Title),
                    new SQLiteParameter("@Subtitle", song.Subtitle),
                    new SQLiteParameter("@XArtistId", song.XArtistId),
                    new SQLiteParameter("@XAlbumId", song.XAlbumId),
                    new SQLiteParameter("@Track", song.Track),
                    new SQLiteParameter("@Lyricist", song.Lyricist),
                    new SQLiteParameter("@Composer", song.Composer),
                    new SQLiteParameter("@Arranger", song.Arranger),
                    new SQLiteParameter("@ListenCount", song.ListenCount),
                    new SQLiteParameter("@ShareCount", song.ShareCount),
                    new SQLiteParameter("@CommentCount", song.CommentCount),
                    new SQLiteParameter("@DiscNumber", song.DiscNumber),
                    new SQLiteParameter("@LastUpdate", song.LastUpdate)
                );
            return result == 1;
        }

        public bool Insert(IEnumerable<Song> songs)
        {
            var paras = from song in songs
                        select new SQLiteParameter[]{
                            new SQLiteParameter("@XId", song.XId),
                            new SQLiteParameter("@Title", song.Title),
                            new SQLiteParameter("@Subtitle", song.Subtitle),
                            new SQLiteParameter("@XArtistId", song.XArtistId),
                            new SQLiteParameter("@XAlbumId", song.XAlbumId),
                            new SQLiteParameter("@Track", song.Track),
                            new SQLiteParameter("@Lyricist", song.Lyricist),
                            new SQLiteParameter("@Composer", song.Composer),
                            new SQLiteParameter("@Arranger", song.Arranger),
                            new SQLiteParameter("@ListenCount", song.ListenCount),
                            new SQLiteParameter("@ShareCount", song.ShareCount),
                            new SQLiteParameter("@CommentCount", song.CommentCount),
                            new SQLiteParameter("@DiscNumber", song.DiscNumber),
                            new SQLiteParameter("@LastUpdate", song.LastUpdate)
                        };
            var result = db.ExecuteNonQueryRange(REPLACE_SONG, paras);

            return songs.Count() == result;
        }

        public bool Insert(Artist artist)
        {
            var result = db.ExecuteNonQuery(REPLACE_ARTIST,
                    new SQLiteParameter("@XId", artist.XId),
                    new SQLiteParameter("@Name", artist.Name),
                    new SQLiteParameter("@Alias", artist.Alias),
                    new SQLiteParameter("@Area", artist.Area),
                    new SQLiteParameter("@Genre", artist.Genre),
                    new SQLiteParameter("@Profile", artist.Profile),
                    new SQLiteParameter("@ListenCount", artist.ListenCount),
                    new SQLiteParameter("@ShareCount", artist.ShareCount),
                    new SQLiteParameter("@CommentCount", artist.CommentCount),
                    new SQLiteParameter("@FansCount", artist.FansCount),
                    new SQLiteParameter("@LastUpdate", artist.LastUpdate)
                );
            return result == 1;
        }

        public bool Insert(IEnumerable<Artist> artists)
        {
            var paras = from artist in artists
                        select new SQLiteParameter[]{
                            new SQLiteParameter("@XId", artist.XId),
                            new SQLiteParameter("@Name", artist.Name),
                            new SQLiteParameter("@Alias", artist.Alias),
                            new SQLiteParameter("@Area", artist.Area),
                            new SQLiteParameter("@Genre", artist.Genre),
                            new SQLiteParameter("@Profile", artist.Profile),
                            new SQLiteParameter("@ListenCount", artist.ListenCount),
                            new SQLiteParameter("@ShareCount", artist.ShareCount),
                            new SQLiteParameter("@CommentCount", artist.CommentCount),
                            new SQLiteParameter("@FansCount", artist.FansCount),
                            new SQLiteParameter("@LastUpdate", artist.LastUpdate)
                        };
            var result = db.ExecuteNonQueryRange(REPLACE_ARTIST, paras);

            return artists.Count() == result;
        }



        public bool CheckAlbum(int xid)
        {
            var r = db.ExecuteScalar(@"select * from album where album.x_id=@XId", new SQLiteParameter("@XId", xid));

            return r == null ? false : true;
        }

        public bool CheckSong(int xid)
        {
            var r = db.ExecuteScalar(@"select * from song where album.x_id=@XId", new SQLiteParameter("@XId", xid));

            return r == null ? false : true;
        }

        public bool CheckArtist(int xid)
        {
            var r = db.ExecuteScalar(@"select * from artist where album.x_id=@xid", new SQLiteParameter("@xid", xid));

            return r == null ? false : true;
        }

    }
}
