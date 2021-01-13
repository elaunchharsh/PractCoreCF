using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PractCoreCF.ModelView
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Please enter email")]
        [EmailAddress(ErrorMessage ="Please enter valid email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Please enter password")]
        public string Password { get; set; }
    }
}
