using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Areas.Manage.ViewModels
{
    public class RegisterVm
    {
        [Required]
        [MaxLength(10)]
        [MinLength(3)]
        public string Name { get; set; }
        [MaxLength(10)]
        [MinLength(3)]
        public string Surname { get; set; }
        [Required]
       
        public string Username { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]

        public string ConfimPassword { get; set; }
    }
}
