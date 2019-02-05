using MvcOnlineTest.Models;
using System.Linq;
using System.Web.Mvc;

namespace MvcOnlineTest.Controllers
{
    public class StartTestController : Controller
    {
        static bool submitFlag = false;
        static int staticIndex = 0, attempted = 0, correct = 0, score = 0;
        private MvcOnlineTestDb db = new MvcOnlineTestDb();

        // GET: StartTest
        public ActionResult Index()
        {
            return View(db.Tests);
        }

        public ActionResult Test(int? id = null, int index = 0, string next = null, string previous = null, string option = null, string submit = null)
        {
            // if the user clicks the back button of browser after submitting the test
            if (submit == null && submitFlag == true)
                return Content("<h1>Session expired</h1>");

            index = staticIndex;

            var test = db.Tests.Find(id);
            if (test == null)
                return HttpNotFound();

            var ques = from q in test.Questions select q;

            // if the result page reloads
            if (submitFlag)
            {
                option = null;
                int[] parameters = { attempted, correct, ques.Count(), score };
                ViewData["parameters"] = parameters;
                return View("TestResult");
            }

            // if the question page reloads
            if (next == null && previous == null && submit == null && option != null)
                option = null;

            // if a radio-button is selected and the form is submitted by clicking one of the next, previous, or the submit buttons
            if ((option != null) && (next != null || previous != null || submit != null))
            {
                attempted++;
                if (option == ques.ElementAt(index).Answer)
                {
                    correct++;
                    score = correct * test.MarksPerQue;
                }
            }

            // if the user ends the test
            if (submit != null)
            {
                submitFlag = true;
                option = null;
                int[] parameters = { attempted, correct, ques.Count(), score };
                ViewData["parameters"] = parameters;
                return View("TestResult");
            }

            // if the next button is clicked
            if (next != null)
            {
                staticIndex = ++index;
                // if the last question is displayed
                if (ques.Count() - staticIndex == 1)
                    ViewBag.DisabledNext = true;
                return View(ques.ElementAt(index));
            }

            // if the first question is displayed
            if (staticIndex == 0)
                ViewBag.DisabledPrev = true;

            // if the previous button is clicked
            if (previous != null)
            {
                staticIndex = --index;
                if (staticIndex == 0)
                    ViewBag.DisabledPrev = true;
                return View(ques.ElementAt(index));
            }

            staticIndex = index;

            return View(ques.ElementAt(index));
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}