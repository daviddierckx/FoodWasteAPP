using FoodWaste.Application.Data;
using FoodWaste.Application.Interfaces;
using FoodWaste.Application.ViewModels;
using FoodWaste.Domain;
using FoodWaste.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodWasteMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IStudentRepo _studentRepo ;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ApplicationDbContext context, IStudentRepo studentRepo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _studentRepo = studentRepo;
        }

        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View(response);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress) ;
            if(user != null)
            {
                //User is found, check password
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    //Password correct, sign in
                   var result = await _signInManager.PasswordSignInAsync(user,loginViewModel.Password, false,false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                //Password is incorrect
                TempData["Error"] = "Wrong credentials. Please, try again";
                return View(loginViewModel);
            }
            //User not found
            TempData["Error"] = "Wrong credentials. Please, try again";
            return View(loginViewModel);
        }

        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid) return View(registerViewModel);
            var user = await _userManager.FindByEmailAsync(registerViewModel.EmailAddress);
            if(user != null)
            {
                TempData["Error"] = "This email address is already in use";
                return View(registerViewModel);
            }
            //TODO wachtwoord complexiteit checkken
            var newUser = new AppUser()
            {
                Naam = registerViewModel.EmailAddress,
                TelefoonNummer = registerViewModel.TelefoonNummer,
                Geboortedatum = registerViewModel.Geboortedatum,
                UserName = registerViewModel.EmailAddress,
                Email = registerViewModel.EmailAddress,
                EmailAdress = registerViewModel.EmailAddress,
                EmailConfirmed = true,
            };
            var newStudent = new Student()
            {
                Studentnummer = "1",
                StudieStad = nameof(registerViewModel.StudieStad),
                AppUserId = newUser.Id
            };


            var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (newUserResponse.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, UserRoles.Student);
                _studentRepo.Add(newStudent);
            }



            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
