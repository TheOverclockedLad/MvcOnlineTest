using System.ComponentModel.DataAnnotations;

namespace MvcOnlineTest.Models
{
    public class Login
    {
        [Key]
        public string username { get; set; }

        public string password { get; set; }

        public string roles { get; set; }
    }
}