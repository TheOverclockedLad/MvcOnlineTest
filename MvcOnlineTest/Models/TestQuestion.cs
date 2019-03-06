using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcOnlineTest.Models
{
    public class TestQuestion
    {
        [Key, Column(Order = 0)]
        public int TestId { get; set; }

        [Key, Column(Order = 1)]
        public int QuestionId { get; set; }

        public virtual Test Test { get; set; }
        public virtual Question Question { get; set; }
    }
}