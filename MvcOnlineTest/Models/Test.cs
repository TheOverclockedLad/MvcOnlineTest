using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcOnlineTest.Models
{
    public class Test
    {
        public int TestId { get; set; }

        public int? StudentId { get; set; }

        [Required, StringLength(30), MinLength(3), Display(Name = "Test")]
        public string TestName { get; set; }

        [Range(1, 5), Required, Display(Name = "Marks per question")]
        public int MarksPerQue { get; set; }

        public virtual Student Student { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}