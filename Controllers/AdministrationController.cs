using EmployeeManagement1.Models;
using EmployeeManagement1.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement1.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdministrationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id={id} cannot be found";
                return View();
            }
            else
            {
                var result = await userManager.DeleteAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("ListUsers");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUsers");

            }
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id={id} cannot be found";
                return View();
            }
            else
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListRoles");

            }
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ListUsers()
        {
           var users= userManager.Users;
            return View(users);
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
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if(role==null)
            {
                ViewBag.ErrorMessage = $"Role with id={id} cannot be found";
                return View("NotFound");
            }
            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name

            };

            foreach(var user in await userManager.GetUsersInRoleAsync(role.Name))
            {
               
                    model.Users.Add(user.UserName);

                
            }

            return View(model);


        }
        [HttpPost]

        public async Task<IActionResult> EditRole(EditRoleViewModel editRoleViewModel)
        {
            var role = await roleManager.FindByIdAsync(editRoleViewModel.Id);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with id={editRoleViewModel.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                role.Name = editRoleViewModel.RoleName;
               var result=await roleManager.UpdateAsync(role);

                if(result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("",error.Description);
                }
                return View(editRoleViewModel);
            }
          
           
        }
        [HttpGet]

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if(user==null)
            {
                ViewBag.ErrorMessage = $"User with id={id} cannot be found";
                return View("NotFound");
            }
            var userRoles = await userManager.GetRolesAsync(user);
            var userClaims = await userManager.GetClaimsAsync(user);

            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                City = user.City,
                UserName = user.UserName,
                Claims = userClaims.Select(c => c.Value).ToList(),
                Roles = userRoles

            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel modelEditUser)
        {
            var user = await userManager.FindByIdAsync(modelEditUser.Id);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with id={modelEditUser.Id} cannot be found";
                return View("NotFound");
            }
            else
            {
                user.Email = modelEditUser.Email;
                user.UserName = modelEditUser.UserName;
                user.City = modelEditUser.City;
                var result = await userManager.UpdateAsync(user);
                if(result.Succeeded)
                {
                    return RedirectToAction("ListUsers");

                }

                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(modelEditUser);

            }
           
        }

        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string roleId)
        {
            ViewBag.roleId = roleId;
            var role = await roleManager.FindByIdAsync(roleId);
            if(role==null)
            {
                ViewBag.ErrorMessage = $"Role with Id={roleId} cannot be found";
                return View("NotFound");
            }
            var model = new List<UserRoleViewModel>();
            foreach(var user in userManager.Users.ToList())
            {
                var userRoleViewModel = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if(await userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                        
                }
                model.Add(userRoleViewModel);
            }

            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model,string roleId)
        {

            var role = await roleManager.FindByIdAsync(roleId);
            if(role==null)
            {
                ViewBag.ErrorMessage = $"Role with Id={roleId} cannot be found";
                return View(model);
            }
            for (int i = 0; i < model.Count;i++)
            {
                var user =await userManager.FindByIdAsync(model[i].UserId);
                IdentityResult result = null;
                if(model[i].IsSelected && !(await userManager.IsInRoleAsync(user,role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if(!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
               if(result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { id = roleId });

                }
            }
            return RedirectToAction("EditRole", new { id = roleId });


        }





        }
    }
