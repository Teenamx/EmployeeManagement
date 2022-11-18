using EmployeeManagement1.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement1.Controllers
{
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleVIewModel createRoleVIewModel)
        {

            if(ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = createRoleVIewModel.RoleName
                };
                IdentityResult identityResult = await roleManager.CreateAsync(identityRole);
                if(identityResult.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Administration");

                }
                foreach(IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }

            }
        

            return View(createRoleVIewModel);
        }

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }



    }
}
