using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LMS_Application.Startup))]
namespace LMS_Application
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
