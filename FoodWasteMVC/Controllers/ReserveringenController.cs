using FoodWaste.Application.Interfaces;
using FoodWaste.Application.ViewModels;
using FoodWaste.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace FoodWasteMVC.Controllers
{
    public class ReserveringenController : Controller
    {
        private readonly IUserRepo _userRepo;

        public ReserveringenController(IUserRepo userRepo)
        {
            _userRepo = userRepo;
        }
        public async Task<IActionResult> Index()
        {
            var userPakkets = await _userRepo.GetAllStudentPakkets();
            var userViewModel = new UserViewModel() { Pakkets = userPakkets };  
            return View(userViewModel);
        }
    }
}
