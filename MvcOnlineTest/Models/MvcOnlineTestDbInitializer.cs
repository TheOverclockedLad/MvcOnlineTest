//using System.Collections.Generic;
//using System.Data.Entity;

//namespace MvcOnlineTest.Models
//{
//    public class MvcOnlineTestDbInitializer : DropCreateDatabaseAlways<MvcOnlineTestDb>
//    {
//        protected override void Seed(MvcOnlineTestDb context)
//        {
//            context.Tests.Add(new Tests { TestName = ".NET" });
//            context.Tests.Add(new Tests { TestName = "Java" });
//            context.Tests.Add(new Tests { TestName = "SQL", Questions = new List<Questions> { new Questions { Question = "SQL stands for what?", Choice1 = "Structured Query Language", Choice2 = "Syntax Query Language", Choice3 = "Structured Query List", Choice4 = "None of the above", Answer = "Structured Query Language" } } });
//            base.Seed(context);
//        }
//    }
//}