using System.Web.Routing;
using SimpleCompression;
using SimpleCompression.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof(SampleMVCSite.App_Start.SimpleCompression), "Start")]
namespace SampleMVCSite.App_Start
{
    public class SimpleCompression
    {
        public static void Start()
        {
            var defaultConfig = new SimpleCompressionConfiguration()
            {
                Compress = true,
                Compressor = new YUICompression(),
                ClientVersionPrefix = "ver1",
                Disable = false,
                FolderForCachedResources = "SuperCache",
            };
            SimpleCompressionConfiguration.SetDefaultConfiguration(defaultConfig);

            var routes = RouteTable.Routes;
            AddRoutes(routes, defaultConfig);
        }

        private static void AddRoutes(RouteCollection routes, SimpleCompressionConfiguration configuration)
        {
            routes.Add(new Route(configuration.FolderForCachedResources + "{*catchall}", new SimpleCompressionRouteHandler()));
        }
    }
}
