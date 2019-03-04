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
                    bool authenticUser = db.Login.Any(user => user.username == model.username && user.password == model.password);

                    if (authenticUser)
                    {
                        FormsAuthentication.SetAuthCookie(model.username, false);
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