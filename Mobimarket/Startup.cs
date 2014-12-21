using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mobimarket.Startup))]
namespace Mobimarket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
        }
    }
}
