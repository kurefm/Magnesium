using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core
{
    public static class WorkPath
    {
        public static string CurrentPath { get; set; } = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        public static string TempPath { get; } = GetPath(Path.Combine(CurrentPath, "temp"));

        public static string DcpPath { get; } = GetPath(Path.Combine(CurrentPath, "dcp"));

        public static string ConfigPath { get; } = Path.Combine(CurrentPath, "config.json");

        public static string DbPath { get; } = GetPath(Path.Combine(CurrentPath, "db"));
        public static string ErrorPagePath { get; } = GetPath(Path.Combine(CurrentPath, "error pages"));


        /// <summary>
        /// 若目录不存在则创建
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static string GetPath(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
