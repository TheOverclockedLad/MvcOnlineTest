using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcOnlineTest.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required, StringLength(25, MinimumLength = 1, ErrorMessage = "Last name cannot be longer than 25 characters."), Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required, StringLength(25, MinimumLength = 1, ErrorMessage = "First name cannot be longer than 25 characters."), Display(Name = "First Name")]
        public string FirstName { get; set; }

        public virtual ICollection<Test> Tests { get; set; }
    }
}