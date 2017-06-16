using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebMaxiFarmacia.Startup))]
namespace WebMaxiFarmacia
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
