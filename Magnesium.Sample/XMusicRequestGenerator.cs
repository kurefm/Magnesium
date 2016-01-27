using Magnesium.Core;
using Magnesium.Core.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Magnesium.Sample
{
    public class XMusicRequestGenerator : DataRequestGenerator
    {
        public XMusicRequestGenerator(Guid guid) : base(guid)
        {
        }

        public override void AutoRun(CancellationToken token)
        {

            Enumerable.Range(1, 10000).ForEach(i => Add(new XMusicRequest(GUID, $"http://www.xiami.com/album/{i}", i)));
        }
    }
}
