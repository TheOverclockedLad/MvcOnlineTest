using MvcOnlineTest.ViewModels;
using MvcOnlineTest.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MvcOnlineTest.Controllers
{
    [HandleError()]
    public class StartTestController : Controller
    {
        MvcOnlineTestDb db = new MvcOnlineTestDb();
        _TestQuestions viewModel = new _TestQuestions();

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

            viewModel.TestId = id;

            viewModel.MarksPerQue = test.MarksPerQue;

            viewModel.QuestionList = (from q in test.Questions select q).ToList();

            viewModel.QueAns.Add(viewModel.QuestionList.First(), null);

            viewModel.Question = viewModel.QuestionList.First();

            viewModel.Attempted = 0;

            viewModel.Correct = 0;

            viewModel.Score = 0;

            ViewBag.DisabledPrev = true;

            Session["ViewModel"] = viewModel;

            return View("Test", viewModel.Question);
        }

        [HttpGet]
        public ActionResult Test(string direction)
        {
            viewModel = Session["ViewModel"] as _TestQuestions;

            int index = viewModel.QuestionList.IndexOf(viewModel.Question);

            viewModel.Question = direction == "next" ? viewModel.QuestionList.ElementAt(++index) : viewModel.QuestionList.ElementAt(--index);

            if (!viewModel.QueAns.ContainsKey(viewModel.Question))
                viewModel.QueAns.Add(viewModel.Question, null);

            if (index == 0)
                ViewBag.DisabledPrev = true;
            else if (viewModel.QuestionList.Count - index == 1)
                ViewBag.DisabledNext = true;

            ViewBag.Option = viewModel.QueAns[viewModel.Question];

            return PartialView("_TestQuestions", viewModel.Question);
        }

        [HttpPost]
        public ActionResult Test(string next, string option, string previous, string submit)
        {
            viewModel = Session["ViewModel"] as _TestQuestions;

            if (option != null)
            {
                if (viewModel.QueAns[viewModel.Question] == null)
                {
                    viewModel.QueAns[viewModel.Question] = option;
                    viewModel.Attempted += 1;
                    if (option == viewModel.Question.Answer)
                    {
                        viewModel.Correct += 1;
                        viewModel.Score = viewModel.Correct * viewModel.MarksPerQue;
                    }
                }
                else if (viewModel.QueAns[viewModel.Question] != option)
                {
                    viewModel.QueAns[viewModel.Question] = option;
                    if (option == viewModel.Question.Answer)
                    {
                        viewModel.Correct += 1;
                        viewModel.Score = viewModel.Correct * viewModel.MarksPerQue;
                    }
                }
            }

            if (submit != null)
            {
                int[] parameters = { viewModel.Attempted, viewModel.Correct, viewModel.QuestionList.Count, viewModel.Score };
                ViewData["parameters"] = parameters;
                DoLeaderboardStuff();
                List<_Leaderboard> model =
                                (from st in db.StudentsTests
                                join s in db.Students on new { st.StudentId } equals new { s.StudentId } into ss
                                from s in ss
                                orderby st.Score descending
                                select new _Leaderboard { FirstName = s.FirstName, LastName = s.LastName, Score = st.Score }
                                ).ToList();

                return PartialView("_TestResult", model);
            }

            return RedirectToAction("Test", new { direction = next == null ? "previous" : "next" });
        }

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

                    StudentTest st = new StudentTest();
                    st.StudentId = student.StudentId;
                    st.TestId = viewModel.TestId;
                    st.Score = viewModel.Score;
                    db.StudentsTests.Add(st);

                    db.SaveChanges();
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (db != null)
                db.Dispose();
            base.Dispose(disposing);
        }
    }
}