using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcOnlineTest.Models
{
    public class StudentTest
    {
        [Key, Column(Order = 0)]
        public int StudentId { get; set; }

        [Key, Column(Order = 1)]
        public int TestId { get; set; }

        public int Score { get; set; }

        public virtual Student Student { get; set; }
        public virtual Test Test { get; set; }
    }
}