using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ReseauPsy.Startup))]
namespace ReseauPsy
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
