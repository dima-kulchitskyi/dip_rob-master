using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewsWebSite.Startup))]
namespace NewsWebSite
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.MapSignalR();

        }
    }
}
