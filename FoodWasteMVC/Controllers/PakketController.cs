using FoodWaste.Application;
using FoodWaste.Application.Interfaces;
using FoodWaste.Domain;
using FoodWaste.Infrastructure.Data;
using FoodWaste.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FoodWasteMVC.Controllers
{
    public class PakketController : Controller
    {
        private readonly IPakketRepo _pakketRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PakketController(IPakketRepo pakketRepo, IHttpContextAccessor httpContextAccessor)
        {
            _pakketRepo = pakketRepo;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Pakket> pakkets = await _pakketRepo.GetAll();
            return View(pakkets);
        }

        public async Task<IActionResult> Detail(int id)
        {
            //Lazy loading is a way to conserve data (Include => like a join in sql)
            //Product product = _context.Products.Include(a => a.Address).FirstOrDefault(c => c.Id == id);

            Pakket pakkets = await _pakketRepo.GetByIdAsync(id);
            return View(pakkets);
        }
        public IActionResult Create()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Pakket pakket)
        {
            if (!ModelState.IsValid)
            {
                return View(pakket);
            }
            _pakketRepo.Add(pakket);
            return RedirectToAction("Index");
        }

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var pakket = await _pakketRepo.GetByIdAsync(id);
        //    if (pakket == null) return View("Error");
        //}
    }
}
