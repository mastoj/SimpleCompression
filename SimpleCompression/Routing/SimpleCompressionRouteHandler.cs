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
