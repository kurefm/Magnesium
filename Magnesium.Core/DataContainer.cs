using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Core
{
    public class DataContainer
    {
        #region Property

        public Guid GUID { get; private set; }
        public object Data { get; set; }
        #endregion

        public DataContainer(Guid guid)
        {
            GUID = guid;
        }

        public DataContainer(Guid guid, object data)
        {
            GUID = guid;
            Data = data;
        }
    }
}
