using Microsoft.Owin;

[assembly: OwinStartup(typeof(PhotoBattles.App.Startup))]

namespace PhotoBattles.App
{
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            this.ConfigureAuth(app);
        }
    }
}