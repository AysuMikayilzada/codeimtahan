using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Areas.Manage.ViewModels
{
    public class LoginVm
    {
        [Required]
        public string UserNameorEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; } 

    }
}
