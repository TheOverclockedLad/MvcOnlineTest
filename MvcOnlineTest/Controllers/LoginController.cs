using MvcOnlineTest.Models;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace MvcOnlineTest.Controllers
{
    [HandleError()]
    public class LoginController : Controller
    {
        public ActionResult Login(string returnUrl)
        {
            if (string.IsNullOrEmpty(returnUrl) && Request.UrlReferrer != null)
                returnUrl = Server.UrlEncode(Request.UrlReferrer.PathAndQuery);

            if (Url.IsLocalUrl(returnUrl) && !string.IsNullOrEmpty(returnUrl))
                ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        public ActionResult Login(Login model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                using (MvcOnlineTestDb db = new MvcOnlineTestDb())
                {
                    string username = model.username;
                    string password = model.password;

                    bool authentic = db.Login.Any(user => user.username == username && user.password == password);

                    if (authentic)
                    {
                        FormsAuthentication.SetAuthCookie(username, false);
                        if (Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        else
                            return RedirectToAction("Index", "StartTest");
                    }
                    else
                        ModelState.AddModelError("", "Incorrect username and/or password");
                }
            }

            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "StartTest");
        }
    }
}