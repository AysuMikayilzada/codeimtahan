using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using WebApplication1.Areas.Manage.ViewModels;
using WebApplication1.DAL;
using WebApplication1.Helpers;
using WebApplication1.Models;

namespace WebApplication1.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signmanager;
        private readonly RoleManager<IdentityRole> _rolemanager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signmanager, RoleManager<IdentityRole> rolemanager)
        {
            _userManager = userManager;
            _signmanager = signmanager;
            _rolemanager = rolemanager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVm registervm)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser()
            {
                Name = registervm.Name,
                Email = registervm.Email,
                Surname = registervm.Surname,
                UserName = registervm.Username
            };
            IdentityResult result=await _userManager.CreateAsync(user,registervm.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View();
                }
            }
            //await _userManager.AddToRoleAsync(user, UserRole.Member(ToString);
            await _signmanager.SignInAsync(user,false);
            return RedirectToAction("Index", "Home", new {area=""});
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task< IActionResult> Login(LoginVm loginvm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var exist = await _userManager.FindByNameAsync(loginvm.UserNameorEmail);
            if (exist == null)
            {
                exist=await _userManager.FindByEmailAsync(loginvm.UserNameorEmail);
                if(exist == null)
                {
                    ModelState.AddModelError("", "username ve ya password sehvdir");
                    return View();
                }
            }
            var signincheck= _signmanager.CheckPasswordSignInAsync(exist, loginvm.Password,true).Result;
            if(!signincheck.Succeeded)
            {
                ModelState.AddModelError("", "username ve ya password sehvdir");
                return View();
            }
            await _signmanager.SignInAsync(exist, loginvm.RememberMe);
            return RedirectToAction("Index", "Home", new { area = "" });

        }
        public async Task<IActionResult> Logout()
        {
            await _signmanager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public async Task<IActionResult> CreateRole()
        {
            foreach (var role in Enum.GetValues(typeof(UserRole)))
            {
                if (await _rolemanager.RoleExistsAsync(role.ToString()))
                {
                    await _rolemanager.CreateAsync(new IdentityRole
                    {
                        Name = role.ToString(),
                    });

                }
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }




    }
}
