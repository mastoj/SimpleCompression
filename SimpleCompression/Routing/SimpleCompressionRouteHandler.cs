using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using SimpleCompression.Web;

namespace SimpleCompression.Routing
{
    public class SimpleCompressionRouteHandler : IRouteHandler
    {
        public System.Web.IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new FileResourceHandler();
        }
    }
}
