using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magnesium.Core;
using Magnesium.Core.DataBase.Sqlite;
using Magnesium.Core.Extension;

namespace Magnesium.Core.Debug
{
    class MagnesiumRunTests
    {
        public static void RunTests()
        {
            var m = new Magnesium();
            m.MagnesiumInit();
            m.LoadConfig();
            m.LoadDcp(@"C:\Users\kuref\OneDrive\Codes\Visual Studio\Projects\Magnesium\SaltTiger\bin\Debug\SaltTiger.dll");
            Console.WriteLine(m);


            var guid = Magnesium.Current.GetDcpList().ElementAt(0).GUID;
            Magnesium.Current.RunAutoGenerator(guid).Wait();
        }

        public static void DbTests()
        {
            var db = new Sqlite3("BookDB.sqlite3");
            using (var r = db.ExecuteReaderOne("select count(*) from book"))
            {
                var v = r.GetValues();
                foreach (var i in Enumerable.Range(0, r.FieldCount))
                {
                    Console.WriteLine("{0}: {1}", v.GetKey(i), r.GetValue(i));
                }
            }
        }
    }
}
