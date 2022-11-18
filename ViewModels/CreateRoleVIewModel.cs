using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement1.ViewModels
{
    public class CreateRoleVIewModel
    {
        [Required]
        public string RoleName { get; set; }
    }
}
