using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Magnesium.Core.Extension;

namespace Magnesium.Core.DataBase.Sqlite
{
    public class Sqlite3 : DbProvider
    {
        public string ConnectionString { get; private set; }

        public Sqlite3(string path)
        {
            ConnectionString = $"Data Source={path}";
        }

        /// <summary>
        /// 执行一条非查询的SQL语句，返回受影响的行数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlPara"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, params SQLiteParameter[] sqlPara)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(sqlPara);

                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 执行一组非查询功能的SQL语句，全部执行完毕后才提交，以提升性能
        /// </summary>
        /// <param name="sqls">一组SQL语句</param>
        /// <returns>执行一组SQL语句受影响的总行数</returns>
        public int ExecuteNonQueryRange(IEnumerable<string> sqls)
        {
            int affect = 0;
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    using (var cmd = new SQLiteCommand("", conn, tran))
                    {
                        foreach (var sql in sqls)
                        {
                            cmd.CommandText = sql;
                            affect += cmd.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                }
            }
            return affect;
        }

        /// <summary>
        /// 执行一组非查询功能的SQLiteCommand，全部执行完毕后才提交，以提升性能
        /// </summary>
        /// <param name="cmds">一组SQLiteCommand</param>
        /// <returns>执行一组SQLiteCommand受影响的总行数</returns>
        public int ExecuteNonQueryRange(IEnumerable<SQLiteCommand> cmds)
        {
            int affect = 0;
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {

                    foreach (var cmd in cmds)
                    {
                        cmd.Connection = conn;
                        cmd.Transaction = tran;
                        affect += cmd.ExecuteNonQuery();
                    }
                    tran.Commit();
                }
            }
            return affect;
        }

        /// <summary>
        /// 执行一组使用相同SQL语句，不同参数的命令
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="paras">参数组</param>
        /// <returns>执行后受影响的总行数</returns>
        public int ExecuteNonQueryRange(string sql, IEnumerable<IEnumerable<SQLiteParameter>> paras)
        {
            int affect = 0;
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var tran = conn.BeginTransaction())
                {
                    using (var cmd = new SQLiteCommand(sql, conn, tran))
                    {
                        foreach (var para in paras)
                        {
                            cmd.Parameters.AddRange(para.ToArray());
                            affect += cmd.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                }
            }
            return affect;
        }

        public object ExecuteScalar(string sql, params SQLiteParameter[] sqlPara)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(sqlPara);

                    return cmd.ExecuteScalar();
                }
            }
        }

        public T ExecuteScalar<T>(string sql, params SQLiteParameter[] sqlPara)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(sqlPara);
                    return (T)cmd.ExecuteScalar();
                }
            }
        }

        public T ExecuteScalar<T>(string sql, params object[] objs)
        {
            int paraCount;
            int objCount = objs.Count();

            //解析SQL
            sql = ParseSql(sql, out paraCount);

            if (paraCount > objCount)
            {
                throw new ApplicationException($"参数数量不足，需要{paraCount}个参数，仅提供了{objCount}个");
            }

            //将object转化为对应的参数
            var sqlPara = objs.Select((para, index) => new SQLiteParameter($"@{index}", para));

            //执行查询
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(sqlPara.ToArray());
                    var result = cmd.ExecuteScalar();
                    if (result == null)
                    {
                        return default(T);
                    }
                    return (T)result;
                }
            }
        }

        //protected IEnumerable<SQLiteDataReader> ExecuteReader(string sql, SQLiteParameter[] sqlPara)
        //{
        //    var conn = new SQLiteConnection(ConnectionString);
        //    conn.Open();
        //    using (var cmd = conn.CreateCommand())
        //    {
        //        cmd.CommandText = sql;
        //        cmd.Parameters.AddRange(sqlPara);

        //        using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
        //        {
        //            while (reader.Read())
        //            {
        //                yield return reader;
        //            }
        //        }
        //    }
        //}

        public IEnumerable<SQLiteDataReader> ExecuteReader(string sql, params object[] objs)
        {
            int paraCount;
            int objCount = objs.Count();

            //解析SQL
            sql = ParseSql(sql, out paraCount);
            if (paraCount > objCount)
            {
                throw new ApplicationException($"参数数量不足，需要{paraCount}个参数，仅提供了{objCount}个");
            }
            //将object转化为对应的参数
            var sqlPara = objs.Select((para, index) => new SQLiteParameter($"@{index}", para)).ToArray();

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(sqlPara);

                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            yield return reader;
                        }
                    }
                }
            }
        }

        public SQLiteDataReader ExecuteReaderOne(string sql, params object[] objs)
        {
            int paraCount;
            int objCount = objs.Count();

            //解析SQL
            sql = ParseSql(sql, out paraCount);
            if (paraCount > objCount)
            {
                throw new ApplicationException($"参数数量不足，需要{paraCount}个参数，仅提供了{objCount}个");
            }
            //将object转化为对应的参数
            var sqlPara = objs.Select((para, index) => new SQLiteParameter($"@{index}", para)).ToArray();

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(sqlPara);

                    var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (reader.Read())
                    {
                        return reader;
                    }
                    return null;
                }
            }
        }

        /// <summary>
        /// 改进的SQL执行方法，可以解析SQL中的?或{n}参数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public int Execute(string sql, params object[] objs)
        {
            int paraCount;
            int objCount = objs.Count();

            //解析SQL
            sql = ParseSql(sql, out paraCount);
            if (paraCount > objCount)
            {
                throw new ApplicationException($"参数数量不足，需要{paraCount}个参数，仅提供了{objCount}个");
            }
            //将object转化为对应的参数
            var sqlPara = objs.Select((para, index) => new SQLiteParameter($"@{index}", para));

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(sqlPara.ToArray());
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        public int Execute(string sql, IEnumerable<IEnumerable<object>> objss)
        {
            int affect = 0;
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                //创建事务
                using (var tran = conn.BeginTransaction())
                {
                    int paraCount;
                    sql = ParseSql(sql, out paraCount);
                    using (var cmd = new SQLiteCommand(sql, conn, tran))
                    {
                        foreach (var objs in objss)
                        {
                            int objCount = objs.Count();
                            if (paraCount > objCount)
                            {
                                throw new ApplicationException($"参数数量不足，需要{paraCount}个参数，仅提供了{objCount}个");
                            }

                            //将object转化为对应的参数
                            var sqlPara = objs.Select((para, index) => new SQLiteParameter($"@{index}", para));

                            cmd.Parameters.Clear();
                            cmd.Parameters.AddRange(sqlPara.ToArray());
                            affect += cmd.ExecuteNonQuery();
                        }
                    }
                    //提交事务
                    tran.Commit();
                }
            }
            return affect;
        }

        /// <summary>
        /// SQL参数解析，可解析SQL语句中的占位符?或{n}
        /// </summary>
        /// <param name="sql">要解析的SQL语句源字符串</param>
        /// <param name="paraCount">解析出的参数个数</param>
        /// <returns>解析完参数的SQL语句，语句中的占位符被替换成@n</returns>
        private string ParseSql(string sql, out int paraCount)
        {
            Debug.Assert(!String.IsNullOrWhiteSpace(sql), $"参数sql为空, sql: {sql}");

            //因为在匿名函数中无法使用ref,out变量
            int _paraCount = 0;
            var s = Regex.Replace(sql, @"\?", m => $"@{_paraCount++}");
            if (_paraCount == 0)
            {
                s = Regex.Replace(sql, @"\{(\d+)\}", m =>
                {
                    int value = Convert.ToInt32(m.Groups[1].Value) + 1;
                    if (value > _paraCount)
                    {
                        _paraCount = value;
                    }
                    return $"@{m.Groups[1].Value}";
                });
            }
            //out解析的参数数量
            paraCount = _paraCount;
            return s;
        }

        public override IEnumerable<string> GetTables()
        {
            return ExecuteReader(@"select name from sqlite_master where type='table' order by name").ForEach(r => r.GetString(0));
        }

        public override DataTable GetDatas(string tableName, int limit = 500)
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = $"select * from {tableName} limit {limit}";

                    var sqlDataAdapter = new SQLiteDataAdapter(cmd);
                    DataSet dataSet = new DataSet();
                    sqlDataAdapter.Fill(dataSet);
                    return dataSet.Tables[0];
                }
            }
        }
    }
}
