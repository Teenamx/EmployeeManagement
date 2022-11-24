using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement1.ViewModels
{
    public class UserClaimsViewModel
    {
        public string UserId { get; set; }

        public List<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}
