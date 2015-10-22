using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PhotoBattles.App.Startup))]
namespace PhotoBattles.App
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
