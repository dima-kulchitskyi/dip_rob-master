using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NewsUa.Startup))]
namespace NewsUa
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
