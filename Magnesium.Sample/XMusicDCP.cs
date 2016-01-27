using Magnesium.Core;
using Magnesium.Core.Web;
using Magnesium.Sample.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magnesium.Sample
{
    public class XMusicDCP : DataCollectProviderBase
    {
        public override string FriendlyName { get; } = "虾米音乐数据采集插件";

        public override Guid GUID { get; } = Guid.NewGuid();

        public override DataRequestGenerator RequestGenerator
        {
            get
            {
                return new XMusicRequestGenerator(GUID);
            }
        }

        public override GeneratorUI GeneratorUI
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override SettingUI SettingUI
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override DataViewUI DataViewUI
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override DataResponse SendHandler(DataRequest dr)
        {
            var xmr = (XMusicRequest)dr;
            var resp = Http.Get(dr.Url);
            return new XMusicResponse(dr.GUID, resp.Text, resp.StatusCode, xmr.ID);
        }

        public override DataContainer ParseHandler(DataResponse dr)
        {
            var xmresp = (XMusicResponse)dr;
            return new DemoModel(dr.GUID, dr.Text, xmresp.ID);
        }

        public override bool StoreHandler(DataContainer dc)
        {
            var dm = (DemoModel)dc;
            File.WriteAllText($@"C:\Users\kuref\Desktop\tempf\{dm.ID}.html", dm.Html);
            return true;
        }
    }
}
