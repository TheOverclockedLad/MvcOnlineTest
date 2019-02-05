using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcOnlineTest.Models
{
    public class Question
    {
        public int QuestionId { get; set; }

        public int? TestId { get; set; }

        [Required]
        public string Que { get; set; }

        [Display(Name = "A")]
        public string OptionA { get; set; }

        [Display(Name = "B")]
        public string OptionB { get; set; }

        [Display(Name = "C")]
        public string OptionC { get; set; }

        [Display(Name = "D")]
        public string OptionD { get; set; }

        public string Answer { get; set; }

        public virtual Test Test { get; set; }
    }
}