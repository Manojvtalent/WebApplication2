using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class UsersD
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
        public string phoneno { get; set; }

    }
}
