using Microsoft.VisualStudio.TestTools.UnitTesting;
using Magnesium.Core.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Magnesium.Core.Web.Tests
{
    [TestClass()]
    public class HttpTests
    {
        [TestMethod()]
        public void HttpMethodTests()
        {
            var url = "http://localhost:19397/";
            TraceHttpResponse(Http.Get(url));
            TraceHttpResponse(Http.Post(url, ""));
            TraceHttpResponse(Http.Head(url));

        }

        private void TraceHttpResponse(HttpResponse hr)
        {
            Trace.WriteLine($"Body: {hr.Text}, Status Code: {hr.StatusCode}");
        }
    }
}