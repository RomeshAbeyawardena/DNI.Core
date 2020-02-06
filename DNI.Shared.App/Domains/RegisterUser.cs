using DNI.Shared.Services.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNI.Shared.App.Domains
{
    public class RegisterUser
    {
        public string Password { get; set; }
        [Display(Name = "Confirm password"), MustMatch(nameof(Password))]
        public string ConfirmPassword { get; set; }
    }
}
