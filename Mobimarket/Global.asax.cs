using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Mobimarket
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public static string EnterpriseImage = "/Images/Enterprises/";
        public static string ProductsImage = "/Images/Products/";
        public static string UsersImage = "/Images/Users/";
        public static string FacebookIcon = "/Images/facebook.png";
        public static string LinkedInIcon = "/Images/linkedin.png";
        public static string PhoneIcon = "/Images/linkedin.png";

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
