using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace Magnesium.Core.Config
{
    public class MagnesiumConfig
    {
        private int _httpConnectionLimit = 500;
        private int _requestDegreeOfParallelism = 50;

        public event EventHandler ConfigChanged;


        /// <summary>
        /// HTTP连接数限制
        /// </summary>
        public int HttpConnectionLimit
        {
            get { return _httpConnectionLimit; }
            set
            {
                if (_httpConnectionLimit != value)
                {
                    _httpConnectionLimit = value;
                    OnConfigChanged();
                }
            }
        }

        /// <summary>
        /// 最大请求并行度
        /// </summary>
        public int RequestDegreeOfParallelism
        {
            get { return _requestDegreeOfParallelism; }
            set
            {
                if (_requestDegreeOfParallelism != value)
                {
                    _requestDegreeOfParallelism = value;
                    OnConfigChanged();
                }
            }
        }

        private void OnConfigChanged()
        {
            ConfigChanged?.Invoke(this, new EventArgs());
        }
    }
}
