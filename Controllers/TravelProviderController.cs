using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using GrandTravelPackages.Models;
using GrandTravelPackages.Services;
using GrandTravelPackages.ViewModels;
using Microsoft.AspNetCore.Identity;


namespace GrandTravelPackages.Controllers
{
    public class TravelProviderController : Controller
    {
        #region Class, Constructor and Injection

  
 
            private UserManager<IdentityUser> _userManagerService;
            private SignInManager<IdentityUser> _signInManagerService;
            private RoleManager<IdentityRole> _roleManagerService;
            private IDataService<TravelProvider> _providerDataService;


            public TravelProviderController(UserManager<IdentityUser> managerService,
                                     SignInManager<IdentityUser> signInService,
                                     RoleManager<IdentityRole> roleService,
                                     IDataService<TravelProvider> providerService
                                    )
            {
                _userManagerService = managerService;
                _signInManagerService = signInService;
                _roleManagerService = roleService;
                _providerDataService = providerService;

            }
        #endregion

        public IActionResult Index()
        {
            return View();
        }


        #region ProviderUpdate



        [HttpGet]
            public async Task<IActionResult> ProviderUpdate()
            {
                //get the user who already logged in
                IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
                //get the profile for this user
                TravelProvider provider = _providerDataService.GetSingle(p => p.UserId == user.Id);
                //create vm
                TravelProviderUpdateProfileViewModel vm = new TravelProviderUpdateProfileViewModel
                {

                    CompanyName = provider.CompanyName,
                    Phone = provider.Phone,
                    Address = provider.Address,
                    WebSite = provider.WebSite,
                    Email = user.Email,
                    UserName = user.UserName
                };
                return View(vm);

            }
            [HttpPost]
            public async Task<IActionResult> ProviderUpdate(TravelProviderUpdateProfileViewModel vm)
            {
                if (ModelState.IsValid)
                {
                    //get the user who already logged in
                    IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
                    //get the profile for this user
                    TravelProvider provider = _providerDataService.GetSingle(p => p.UserId == user.Id);
                    //map the vm
                  //  provider.UserId = user.Id;
                    provider.CompanyName = vm.CompanyName;
                    provider.Phone = vm.Phone;
                    provider.Address = vm.Address;
                    provider.WebSite = vm.WebSite;
                    user.Email = vm.Email;
                    user.UserName = vm.UserName;

                    //save changes
                    _providerDataService.Update(provider);                   
                    await _userManagerService.UpdateAsync(user);
                    //go home
                    return RedirectToAction("Index", "Home");
                }
                return View(vm);
            }
        #endregion
        #region PasswordUpdate

        [HttpGet]
        public async Task<IActionResult> PasswordUpdate()
        {
            //get the user who already logged in
            IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
            var hashPassword = await _userManagerService.HasPasswordAsync(user);
            
            // create vm
            TravelProviderPasswordUpdateViewModel vm = new TravelProviderPasswordUpdateViewModel
            {

            };
            return View(vm);

        }

        [HttpPost]
        public async Task<IActionResult> PasswordUpdate(TravelProviderPasswordUpdateViewModel vm)
        {

            if (ModelState.IsValid)
            {
                IdentityUser user = await _userManagerService.FindByNameAsync(User.Identity.Name);
                TravelProvider profile = _providerDataService.GetSingle(p => p.UserId == user.Id);
                var result = await _userManagerService.ChangePasswordAsync(user, vm.OldPassword, vm.NewPassword);
                if (vm.OldPassword != vm.NewPassword)
                {
                    vm.NewPassword = vm.ConfirmPassword;
                }
                if (!result.Succeeded)
                {
                    ViewBag.notification = "Sorry! Please try again";

                }
                await _signInManagerService.SignInAsync(user, false);

                return RedirectToAction("Login", "Account");

            }
            return View(vm);
        }

        #endregion
    }
}