using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using MvcOnlineTest.Models;

namespace MvcOnlineTest.Controllers
{
    [Authorize(Roles = "admin"), HandleError()]
    public class QuestionsController : Controller
    {
        private MvcOnlineTestDb db = new MvcOnlineTestDb();

        // GET: Questions
        public ActionResult Index()
        {
            IQueryable<Question> questions = db.Questions.Include(q => q.Test);
            return View(questions.ToList());
        }

        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Question question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();

            System.Collections.Generic.List<string> tests = 
                                                    (from tq in db.TestsQuestions
                                                     join t in db.Tests on new { tq.TestId } equals new { t.TestId } into qt
                                                     where tq.QuestionId == id
                                                     from q in qt
                                                     select q.TestName
                                                     ).ToList();
            ViewBag.Tests = tests;

            return View(question);
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            ViewBag.TestId = new SelectList(db.Tests, "TestId", "TestName");
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Question question, bool redirect = false)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);

                if (question.TestId != null)
                {
                    TestQuestion tq = new TestQuestion();
                    tq.TestId = (int)question.TestId;
                    tq.QuestionId = question.QuestionId;
                    db.TestsQuestions.Add(tq);
                }

                db.SaveChanges();

                if (redirect)
                    return RedirectToAction("Details", "Tests", new { id = question.TestId });

                return RedirectToAction("Index");
            }

            ViewBag.TestId = new SelectList(db.Tests, "TestId", "TestName", question.TestId);

            if (redirect)
                return RedirectToAction("Details", "Tests", new { id = question.TestId });
            
            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.TestId = new SelectList(db.Tests, "TestId", "TestName", question.TestId);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "QuestionId,TestId,Que,OptionA,OptionB,OptionC,OptionD,Answer")] Question question, bool redirect = false)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();

                if (redirect)
                    return RedirectToAction("Details", "Tests", new { id = question.TestId });

                return RedirectToAction("Index");
            }
            ViewBag.TestId = new SelectList(db.Tests, "TestId", "TestName", question.TestId);
            
            if (redirect)
                return RedirectToAction("Details", "Tests", new { id = question.TestId });

            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}