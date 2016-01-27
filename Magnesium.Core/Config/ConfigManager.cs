using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Magnesium.Core.Extension;
using System.IO;
using Newtonsoft.Json;

namespace Magnesium.Core.Config
{
    internal class ConfigManager
    {
        private string _filePath;
        private MagnesiumConfig _config;

        public MagnesiumConfig Config => _config;

        public ConfigManager(string filePath)
        {
            //文件存在则加载，否则创建
            if (File.Exists(filePath))
            {
                Reload(filePath);
            }
            else
            {
                _filePath = filePath;
                _config = new MagnesiumConfig();
                Save();
            }
            _config.ConfigChanged += (sender, e) => Save();
        }

        /// <summary>
        /// 重新从指定路径加载配置文件
        /// </summary>
        /// <param name="filePath"></param>
        public void Reload(string filePath)
        {
            if (filePath.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            if (File.Exists(filePath) && File.GetAttributes(filePath).HasFlag(FileAttributes.Directory))
            {
                throw new ArgumentException("提供的path为目录，path必须为配置文件");
            }
            _filePath = filePath;
            using (TextReader sr = File.OpenText(filePath))
            {
                var serializer = new JsonSerializer();
                _config = (MagnesiumConfig)serializer.Deserialize(sr, typeof(MagnesiumConfig));
            }
        }

        /// <summary>
        /// 重新加载配置文件
        /// </summary>
        public void Reload()
        {
            Reload(_filePath);
        }

        /// <summary>
        /// 保存配置文件到指定路径
        /// </summary>
        /// <param name="filePath"></param>
        public void Save(string filePath)
        {
            if (filePath.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            using (StreamWriter file = File.CreateText(filePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, _config);
            }
        }

        /// <summary>
        /// 保存配置文件
        /// </summary>
        public void Save()
        {
            Save(_filePath);
        }
    }
}
