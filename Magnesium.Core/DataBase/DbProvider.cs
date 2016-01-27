using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core.DataBase
{
    /// <summary>
    /// 定义一个数据库服务提供程序应该实现的方法    
    /// </summary>
    public abstract class DbProvider
    {
        public abstract IEnumerable<string> GetTables();

        public abstract DataTable GetDatas(string tableName, int limit = 500);

        //public abstract IEnumerable<IEnumerable<object>> Execute(string sql, params object[] paras);
    }
}
