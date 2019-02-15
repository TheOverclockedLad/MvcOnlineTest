using System.Linq;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Security.Principal;

namespace MvcOnlineTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_PostAuthenticateRequest(object sender, System.EventArgs e)
        {
            if (FormsAuthentication.CookiesSupported == true)
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    try
                    {
                        string username = FormsAuthentication.Decrypt(Request.Cookies[FormsAuthentication.FormsCookieName].Value).Name;
                        string roles = string.Empty;

                        using (Models.MvcOnlineTestDb db = new Models.MvcOnlineTestDb())
                        {
                            Models.Login user = db.Login.SingleOrDefault(u => u.username == username);
                            roles = user.roles;
                        }

                        System.Web.HttpContext.Current.User = new GenericPrincipal(new GenericIdentity(username, "Forms"), roles.Split(';'));
                    }
                    catch(System.Exception)
                    {

                    }
                }
        }
    }
}