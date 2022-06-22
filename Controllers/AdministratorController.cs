using EFTask.Models;
using EFTask.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EFTask.Controllers
{
    /* [Authorize (Roles ="Admin,User")] 
    it will work if login member Admin or User */
    
    /* it will work when login member Admin and User
     [Authorize(Roles = "User")]
     [Authorize(Roles = "Admin")]*/


    // AdminRolePolicy by using role
    [Authorize(Policy  = "AdminRolePolicy")]

    public class AdministratorController : Controller
    {
        private readonly UserManager<ApplicationUser> _UserManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public ILogger<AdministratorController> _Logger { get; }

        public AdministratorController(RoleManager<IdentityRole> roleManager,
                                        UserManager<ApplicationUser> userManager,
                                           ILogger<AdministratorController> logger)
        {
            this._roleManager = roleManager;
            _UserManager = userManager;
            _Logger = logger;
        }

        //ManageUserClaims
        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string UserId)
        {
            ViewBag.UserId = UserId;

            var user = await _UserManager.FindByIdAsync(UserId);

            if (user is not null)
            {
                var existingClaims = await _UserManager.GetClaimsAsync(user);
                var model = new UserClaimsViewModel()
                {
                    UserId = UserId
                };


                foreach (var claim in ClaimsStore.AllClaims)
                {
                    var userClaim = new UserClaims
                    {
                        ClaimType = claim.Type

                    };
                    // If the user has the claim, set IsSelected property to true, so the checkbox
                    // next to the claim is checked on the UI

                    if (existingClaims.Any(c => c.Type == claim.Type))
                    {
                        userClaim.IsSelected = true;
                    }
                    else
                    {
                        userClaim.IsSelected = false;
                    }

                    model.Claims.Add(userClaim);
                    
                }
                return View(model);
            }
            else
            {
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");

            }

        }
        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await _UserManager.FindByIdAsync(model.UserId);

            if (user is not  null)
            {
                var Existingclaims = await _UserManager.GetClaimsAsync(user);
                var result = await _UserManager.RemoveClaimsAsync(user, Existingclaims);

                
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove user existing claims");
                    return View(model);
                }

                // Add all the claims that are selected on the UI
             result = await _UserManager.AddClaimsAsync(user,model.Claims.Where(c => c.IsSelected).Select(c => new Claim(c.ClaimType, c.ClaimType)));

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected roles to user");
                    return View(model);
                }

                return RedirectToAction("EditUser", new { Id = model.UserId });
              
            }
            else
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} cannot be found";
                return View("NotFound");
            }

            
        }

        //  EditUserInRole
        [HttpGet]
        public async Task<IActionResult> ManageUserRole(string UserId)
        {
            ViewBag.UserId = UserId;

            var user = await _UserManager.FindByIdAsync(UserId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {UserId} cannot be found";
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _UserManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserRole(List<UserRolesViewModel> model, string userId)
        {
            var user = await _UserManager.FindByIdAsync(userId);

            if (user is not null)
            {
                var roles = await _UserManager.GetRolesAsync(user);
                var result = await _UserManager.RemoveFromRolesAsync(user, roles);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot remove user existing roles");
                    return View(model);
                }

                result = await _UserManager.AddToRolesAsync(user,
                    model.Where(x => x.IsSelected).Select(y => y.RoleName));

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "Cannot add selected roles to user");
                    return View(model);
                }

                return RedirectToAction("EditUser", new { Id = userId });
            }
            else
            {
                ViewBag.ErrorMessage = $"User with Id = {userId} cannot be found";
                return View("NotFound");
            }
           
        }

        // DeleteUser

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var user = await _UserManager.FindByIdAsync(Id);
            if (user is not null)
            {
                var result = await _UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("UsersList");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return RedirectToAction("UsersList");
            }
            else
            {
                ViewBag.ErrorMessage = $"User With Id = {Id} is not found";
                return View("NotFound");
            }
        }

        [HttpPost]
        [Authorize(Policy = "DeleteRolePolicy")]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role is not null)
            {
                try
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("RolesList");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return RedirectToAction("RolesList");
                }
                catch (DbUpdateException ex)
                {
                    //Log the exception to a file.
                    _Logger.LogError($"Exception Occured : {ex}");
                    // Pass the ErrorTitle and ErrorMessage that you want to show to
                    // the user using ViewBag. The Error view retrieves this data
                    // from the ViewBag and displays to the user.
                    ViewBag.ErrorTitle = $"{role.Name} role is in use";
                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users in this role. If you want to delete this role, please remove the users from the role and then try to delete";
                    return View("Error");
                }
            }
            else
            {
                ViewBag.ErrorMessage = $"Role With Id = {Id} is not found";
                return View("NotFound");
            }
        }

        [HttpGet]
        public IActionResult UsersList()
        {
            var users = _UserManager.Users;
            return View(users);
        }


        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _UserManager.FindByIdAsync(id);
            if (user is not null)
            {
                var UserClaims = await _UserManager.GetClaimsAsync(user);
                var userRoles = await _UserManager.GetRolesAsync(user);
                var model = new EditUserViewModel()
                {
                    UserName = user.UserName,
                    Country = user.Country,
                    Email = user.Email,
                    Id = user.Id,
                    Claims = UserClaims.Select(c => c.Value).ToList(),
                    Roles = userRoles.ToList()
                };
                return View(model);
            }
            else
            {
                ViewBag.ErrorMessage = $"User With Id = {id} is not found";
                return View("NotFound");
            }

        }
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            var user = await _UserManager.FindByIdAsync(model.Id);
            if (user is not null)
            {

                user.UserName = model.UserName;
                user.Country = model.Country;
                user.Email = model.Email;

                var result = await _UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("UsersList");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            else
            {
                ViewBag.ErrorMessage = $"User With Id = {model.Id} is not found";
                return View("NotFound");
            }
        }

        [HttpGet]
        public IActionResult RolesList()
        {
            var role = _roleManager.Roles;
            return View(role);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = model.RoleName
                };
                var result = await _roleManager.CreateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList", "Administrator");
                }
                foreach (IdentityError error in result.Errors)
                {

                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }


        [HttpGet]
        //[Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is not null)
            {
                var model = new EditRoleViewModel
                {
                    Id = role.Id,
                    RoleName = role.Name
                };
                foreach (var user in _UserManager.Users)
                {
                    if (await _UserManager.IsInRoleAsync(user, role.Name))
                    {
                        model.Users.Add(user.UserName);
                    }
                }
                return View(model);
            }
            else
            {
                ViewBag.ErrorMessage = $"Role With Id = {id} is not found";
                return View("NotFound");
            }

        }
        [HttpPost]
       // [Authorize(Policy = "EditRolePolicy")]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);
            if (role is not null)
            {

                role.Name = model.RoleName;
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("RolesList", "Administrator");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }
            else
            {
                ViewBag.ErrorMessage = $"Role With Id = {model.Id} is not found";
                return View("NotFound");
            }

        }

        //  EditRoleInRole
        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string RoleId)
        {
            ViewBag.RoleId = RoleId;
            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role is not null)
            {
                var model = new List<UserRoleViewModel>();

                foreach (var user in _UserManager.Users)
                {
                    var UserRoleViewModel = new UserRoleViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName
                    };
                    if (await _UserManager.IsInRoleAsync(user, role.Name))
                    {
                        UserRoleViewModel.IsSelected = true;
                    }
                    else
                    {
                        UserRoleViewModel.IsSelected = false;
                    }
                    model.Add(UserRoleViewModel);
                }
                return View(model);
            }
            else
            {
                ViewBag.ErrorMessage = $"Role With Id = {RoleId} is not found";
                return View("NotFound");
            }

        }
        [HttpPost]
        public async Task<IActionResult> EditUserInRole(List<UserRoleViewModel> model, string RoleId)
        {

            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role is not null)
            {
                for (int i = 0; i < model.Count; i++)
                {
                    var user = await _UserManager.FindByIdAsync(model[i].UserId);
                    IdentityResult result = null;
                    if (model[i].IsSelected && !(await _UserManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await _UserManager.AddToRoleAsync(user, role.Name);
                    }
                    else if (!model[i].IsSelected && (await _UserManager.IsInRoleAsync(user, role.Name)))
                    {
                        result = await _UserManager.RemoveFromRoleAsync(user, role.Name);
                    }
                    else
                    {
                        continue;
                    }

                    if (result.Succeeded)
                    {
                        if (i < model.Count - 1)
                            continue;
                        else
                        {
                            return RedirectToAction("EditRole", new { Id = RoleId });
                        }
                    }
                }
                return RedirectToAction("EditRole", new { Id = RoleId });
            }
            else
            {
                ViewBag.ErrorMessage = $"Role With Id = {RoleId} is not found";
                return View("NotFound");
            }
        }

    }

}
