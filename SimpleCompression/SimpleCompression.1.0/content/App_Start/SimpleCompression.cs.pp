using System.Web.Routing;
using SimpleCompression.Routing;

[assembly: WebActivator.PreApplicationStartMethod(typeof($rootnamespace$.App_Start.SparkWebMvc), "Start")]
namespace $rootnamespace$.App_Start
{
    public static class SimpleCompression
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
            routes.Add(new Route(configuration.FolderForCachedResources + "/{*catchall}", new SimpleCompressionRouteHandler()));
        }
    }
}
