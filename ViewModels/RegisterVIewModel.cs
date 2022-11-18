using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement1.Utilities;

namespace EmployeeManagement1.ViewModels
{
    public class RegisterVIewModel
    {
        [Required]
        [EmailAddress]
        [ValidEmailDomain(allowedDomain:"gmail.com",ErrorMessage ="Email Domain must be gmail.com")]
     //   [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name ="Confirm Password")]
        [Compare("Password",ErrorMessage ="Password and confirmation password do not match")]

        public string ConfirmPassword { get; set; }

        public string City { get; set; }
    }
}
