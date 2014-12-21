using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Mobimarket.Startup))]
namespace Mobimarket
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
