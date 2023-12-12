using System.ComponentModel.DataAnnotations;

namespace FashionHub.ViewModels
{
    public class RegisterVM
    {


        public string UserId { get; set; }=null!;
        [Required(ErrorMessage = "*")]
        public string Username { get; set; }
        [Required(ErrorMessage = "*")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "*")]
        public string Email { get; set; }
        [Required(ErrorMessage = "*")]
        public string Address { get; set; }
        [Required(ErrorMessage = "*")]
        public string PhoneNumber { get; set; }

    }
}
