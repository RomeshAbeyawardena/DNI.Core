using DNI.Shared.Services.Attributes;
using System.ComponentModel.DataAnnotations;

namespace DNI.Shared.App.Domains
{
    public class RegisterUser
    {
        public string Password { get; set; }
        [Display(Name = "Confirm password"), MustMatch(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
