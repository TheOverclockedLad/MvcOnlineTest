using MvcOnlineTest.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace MvcOnlineTest.Controllers
{
    public class StartTestController : Controller
    {
        private MvcOnlineTestDb db = new MvcOnlineTestDb();

        // GET: StartTest - Display all tests
        public ActionResult Index()
        {
            return View(db.Tests);
        }

        [HttpPost]
        // Retrieve questions from selected test and store student name
        public ActionResult Index(string name, int id)
        {
            Session["StudentName"] = name;

            Test test = db.Tests.Find(id);
            if (test == null)
                return HttpNotFound();

            Session["TestId"] = id;
            Session["MarksPerQue"] = test.MarksPerQue;
            Session["QuestionList"] = (from q in test.Questions select q).ToList();
            Session["Question"] = ((List<Question>)Session["QuestionList"]).First();
            Session["Attempted"] = 0;
            Session["Correct"] = 0;
            Session["Score"] = 0;
            ViewBag.DisabledPrev = true;

            return View("Test", (Question)Session["Question"]);
        }

        //public PartialViewResult PreviousQuestion(string option)
        //{
        //    int index = ((List<Question>)Session["QuestionList"]).IndexOf((Question)Session["Question"]);

        //    if (--index == 0)
        //    {
        //        ViewBag.DisabledPrev = true;
        //        //return View("Test", (Question)Session["Question"]);
        //    }

        //    Session["Question"] = ((List<Question>)Session["QuestionList"]).ElementAt(index);

        //    return PartialView("_TestQuestions", (Question)Session["Question"]);
        //}

        //public ActionResult NextQuestion(string option)
        //{
        //    if (option != null)
        //        return Content("<script>alert('Hello World! " + option + "')</script>");

        //    int index = ((List<Question>)Session["QuestionList"]).IndexOf((Question)Session["Question"]);

        //    Session["Question"] = ((List<Question>)Session["QuestionList"]).ElementAt(++index);

        //    // if the last question is displayed
        //    if (((List<Question>)Session["QuestionList"]).Count() - index == 1)
        //        ViewBag.DisabledNext = true;

        //    return PartialView("_TestQuestions", (Question)Session["Question"]);
        //}

        [HttpGet]
        public ActionResult Test(string direction)
        {
            int index = ((List<Question>)Session["QuestionList"]).IndexOf((Question)Session["Question"]);

            Session["Question"] = direction == "next" ? ((List<Question>)Session["QuestionList"]).ElementAt(++index) : ((List<Question>)Session["QuestionList"]).ElementAt(--index);

            if (index == 0)
                ViewBag.DisabledPrev = true;
            else if (((List<Question>)Session["QuestionList"]).Count - index == 1)
                ViewBag.DisabledNext = true;

            return PartialView("_TestQuestions", (Question)Session["Question"]);
        }

        [HttpPost]
        public ActionResult Test(string next, string option, string previous, string submit)
        {
            if (option != null)
            {
                Session["Attempted"] = (int)Session["Attempted"] + 1;

                if (option == ((Question)Session["Question"]).Answer)
                {
                    Session["Correct"] = (int)Session["Correct"] + 1;
                    Session["Score"] = (int)Session["Correct"] * (int)Session["MarksPerQue"];
                }
            }

            if (submit != null)
            {
                int[] parameters = { (int)Session["Attempted"], (int)Session["Correct"], ((List<Question>)Session["QuestionList"]).Count, (int)Session["Score"] };
                ViewData["parameters"] = parameters;
                DoLeaderboardStuff();
                List<VM_Leaderboard> model =
                                (from st in db.StudentsTests
                                join s in db.Students on new { st.StudentId } equals new { s.StudentId } into ss
                                from s in ss
                                orderby st.Score descending
                                select new VM_Leaderboard { FirstName = s.FirstName, LastName = s.LastName, Score = st.Score }).ToList();

                return PartialView("_TestResult", model);
            }

            return RedirectToAction("Test", new { direction = next == null ? "previous" : "next" });
        }

        //public ActionResult Test(string next = null, string previous = null, string option = null, string submit = null)
        //{
        //    int index = ((List<Question>)Session["QuestionList"]).IndexOf((Question)Session["Question"]);

        //    // if the question page reloads
        //    if (next == null && previous == null && submit == null && option != null)
        //        option = null;

        //    // if a radio-button is selected and the form is submitted by clicking one of the next, previous, or the submit buttons
        //    if ((option != null) && (next != null || previous != null || submit != null))
        //    {
        //        Session["Attempted"] = (int)Session["Attempted"] + 1;

        //        if (option == ((Question)Session["Question"]).Answer)
        //        {
        //            Session["Correct"] = (int)Session["Correct"] + 1;
        //            Session["Score"] = (int)Session["Correct"] * (int)Session["MarksPerQue"];
        //        }
        //    }

        //    // if the user ends the test
        //    if (submit != null)
        //    {
        //        int[] parameters = { (int)Session["Attempted"], (int)Session["Correct"], ((List<Question>)Session["QuestionList"]).Count, (int)Session["Score"] };
        //        ViewData["parameters"] = parameters;
        //        //DisplayLeaderboard(staticName, score);
        //        return View("TestResult");
        //    }

        //    // if the next button is clicked
        //    if (next != null)
        //    {
        //        Session["Question"] = ((List<Question>)Session["QuestionList"]).ElementAt(++index);

        //        // if the last question is displayed
        //        if (((List<Question>)Session["QuestionList"]).Count() - index == 1)
        //            ViewBag.DisabledNext = true;
        //    }

        //    // if the previous button is clicked
        //    if (previous != null)
        //    {
        //        if (--index >= 0)
        //        {
        //            Session["Question"] = ((List<Question>)Session["QuestionList"]).ElementAt(index);
        //            if (index == 0)
        //                ViewBag.DisabledPrev = true;
        //        }
        //    }

        //    //return View((Question)Session["Question"]);
        //    return PartialView("_TestQuestions", (Question)Session["Question"]);
        //}

        public void DoLeaderboardStuff()
        {
            using (MvcOnlineTestDb db = new MvcOnlineTestDb())
            {
                if (ModelState.IsValid)
                {
                    Student student = new Student();

                    string[] name = Session["StudentName"].ToString().Split(' ');
                    string firstName = name[0];
                    string lastName = string.Empty;
                    if (name.Length >= 2)
                        for (byte i = 1; i < name.Length; i++)
                            lastName += name[i] + ' ';
                    
                    student.FirstName = firstName;
                    student.LastName = lastName;
                    db.Students.Add(student);
                    db.SaveChanges();

                    StudentTest st = new StudentTest();
                    st.StudentId = student.StudentId;
                    st.TestId = (int)Session["TestId"];
                    st.Score = (int)Session["Score"];
                    db.StudentsTests.Add(st);
                    db.SaveChanges();
                }
            }
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (db != null)
        //        db.Dispose();
        //    base.Dispose(disposing);
        //}
    }
}