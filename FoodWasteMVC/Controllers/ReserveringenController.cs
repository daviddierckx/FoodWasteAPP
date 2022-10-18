using FoodWaste.Application;
using FoodWaste.Application.Interfaces;
using FoodWaste.Application.ViewModels;
using FoodWaste.Domain;
using FoodWaste.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodWasteMVC.Controllers
{
    public class ReserveringenController : Controller
    {
        private readonly IUserRepo _userRepo;
        private UserManager<AppUser> _userManager;

        public ReserveringenController(IUserRepo userRepo, UserManager<AppUser> userManager)
        {
            _userRepo = userRepo;
            _userManager = userManager;

        }
        public async Task<IActionResult> Index()
        {
            var userPakkets = await _userRepo.GetAllStudentPakkets();
            var user = await _userManager.GetUserAsync(User);
            var userViewModel = new UserViewModel() { Pakkets = userPakkets, AppUser = user  };  
            return View(userViewModel);
        }
    }
}
