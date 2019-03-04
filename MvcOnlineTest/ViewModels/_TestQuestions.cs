namespace MvcOnlineTest.ViewModels
{
    public class _TestQuestions
    {
        public int TestId { get; set; }

        public System.DateTime? time { get; set; }

        public System.Collections.Generic.List<Models.Question> QuestionList { get; set; }

        public System.Collections.Generic.Dictionary<Models.Question, string> QueAns = new System.Collections.Generic.Dictionary<Models.Question, string>();

        public Models.Question Question { get; set; }

        public int Attempted { get; set; }

        public int Correct { get; set; }

        public int Score { get; set; }

        public int MarksPerQue { get; set; }
    }
}