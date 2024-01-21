using ExamBusiness.Helper;
using ExamBusiness.Models;
using ExamBusiness.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ExamBusiness.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserVM registerUserVM)
        {
            if(!ModelState.IsValid) 
            {
                return View(registerUserVM);
            }
            AppUser appUser = new AppUser()
            {
                Name = registerUserVM.Name,
                Email = registerUserVM.Email,
                Surname = registerUserVM.Surname,   
                UserName = registerUserVM.Username
            };
            var create  = await _userManager.CreateAsync(appUser , registerUserVM.Password);
            if (!create.Succeeded)
            {
                foreach (var item in create.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);

                }
                return View(registerUserVM);
            }
            await _userManager.AddToRoleAsync(appUser , UserRole.Admin.ToString());
            return RedirectToAction("login" , "account");

        }
        public async Task<IActionResult> Login ()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            AppUser user= await _userManager.FindByEmailAsync(loginVM.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Email ve ya password sehvdir");
                return View(loginVM);
            }


            var result = await _signInManager.PasswordSignInAsync(user:user , password:loginVM.Password , false ,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email ve ya password sehvdir");
                return View(loginVM);
            }
            return RedirectToAction("Index", "Home");

        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }






        public async Task<IActionResult> CreateRole()
        {
            foreach (UserRole item in Enum.GetValues(typeof(UserRole)))
            {
                if(await _roleManager.FindByNameAsync(item.ToString()) == null)
                {
                    await _roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = item.ToString(),
                    });
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
