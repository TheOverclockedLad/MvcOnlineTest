using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MvcOnlineTest.Startup))]
namespace MvcOnlineTest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
