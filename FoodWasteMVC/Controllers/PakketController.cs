using FoodWaste.Application;
using FoodWaste.Application.Interfaces;
using FoodWaste.Application.Services;
using FoodWaste.Application.ViewModels;
using FoodWaste.Domain;
using FoodWaste.Domain.Enums;
using FoodWaste.Infrastructure.Data;
using FoodWaste.Infrastructure.Repository;
using FoodWasteMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FoodWasteMVC.Controllers
{
    public class PakketController : Controller

    {
        private readonly ApplicationDbContext _context;
        private readonly IPakketRepo _pakketRepo;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IProductRepo _productRepo;
        public PakketController(IPakketRepo pakketRepo, IProductRepo productRepo , IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _pakketRepo = pakketRepo;
            _httpContextAccessor = httpContextAccessor;
            _productRepo = productRepo;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Pakket> pakkets = await _pakketRepo.GetAll();
            return View(pakkets);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var pakket = await _pakketRepo.GetByIdAsyncNoTracking(id);
            if (pakket == null) return View("Error");

            var createPakketViewModel = new CreatePakketViewModel
            {
                BeschrijvendeNaam = pakket.BeschrijvendeNaam,
                SelectedProductId = pakket.SelectedProductId,
                ProductCollectie = await _pakketRepo.GetAllProductsFromPakket(pakket.SelectedProductId),
                Stad = (Stad)Enum.Parse(typeof(Stad), pakket.Stad),
                Kantine = (Locatie)Enum.Parse(typeof(Locatie), pakket.Kantine),
                TijdOphalen = pakket.TijdOphalen,
                TijdTotOphalen = pakket.TijdTotOphalen,
                Meerderjarig = pakket.Meerderjarig,
                Prijs = pakket.Prijs,
                TypeMaaltijd = (Maaltijd)Enum.Parse(typeof(Maaltijd), pakket.TypeMaaltijd),
                AppUserId = curUserID,
            };
            return View(createPakketViewModel);
        }
        public IActionResult Create()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var createPakketViewModel = new CreatePakketViewModel {ProductCollectie = _context.Products.ToList(), AppUserId = curUserID };
            return View(createPakketViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePakketViewModel pakketVM)
        {
            var producten = string.Join(",", pakketVM.SelectIDArray);


                var pakket = new Pakket

                {
                    BeschrijvendeNaam = pakketVM.BeschrijvendeNaam,
                    SelectedProductId = producten,
                    Stad = pakketVM.Stad.ToString(),
                    Kantine = pakketVM.Kantine.ToString(),
                    TijdOphalen = pakketVM.TijdOphalen,
                   TijdTotOphalen = pakketVM.TijdTotOphalen,
                   Meerderjarig = pakketVM.Meerderjarig,
                   Prijs = pakketVM.Prijs,
                   TypeMaaltijd = pakketVM.TypeMaaltijd.ToString(),
                };

                _pakketRepo.Add(pakket);
                return RedirectToAction("Index");

            
      
        }

        public async Task<IActionResult> Reserve(int id)
        {
           var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var pakket = await _pakketRepo.GetByIdAsyncNoTracking(id);
            if (pakket == null) return View("Error");
          
            var createPakketViewModel = new CreatePakketViewModel {
                BeschrijvendeNaam = pakket.BeschrijvendeNaam,
                SelectedProductId = pakket.SelectedProductId,
                Stad = (Stad)Enum.Parse(typeof(Stad),pakket.Stad),
                Kantine = (Locatie)Enum.Parse(typeof(Locatie), pakket.Kantine),
                TijdOphalen = pakket.TijdOphalen,
                TijdTotOphalen = pakket.TijdTotOphalen,
                Meerderjarig = pakket.Meerderjarig,
                Prijs = pakket.Prijs,
                TypeMaaltijd = (Maaltijd)Enum.Parse(typeof(Maaltijd), pakket.TypeMaaltijd),
                AppUserId = curUserID,
            };
            return View(createPakketViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Reserve(int id, CreatePakketViewModel pakketvm)
        {
            var userPakket = await _pakketRepo.GetByIdAsyncNoTracking(id);
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();

            if (_pakketRepo != null)
            {
                var pakket = new Pakket
                {
                    BeschrijvendeNaam = pakketvm.BeschrijvendeNaam,
                    SelectedProductId = pakketvm.SelectedProductId,
                    Stad = pakketvm.Stad.ToString(),
                    Kantine = pakketvm.Kantine.ToString(),
                    TijdOphalen = pakketvm.TijdOphalen,
                    TijdTotOphalen = pakketvm.TijdTotOphalen,
                    Meerderjarig = pakketvm.Meerderjarig,
                    Prijs = pakketvm.Prijs,
                    TypeMaaltijd = pakketvm.TypeMaaltijd.ToString(),
                    AppUserId = curUserID,
                };
                _pakketRepo.Add(pakket);
                return RedirectToAction("Index");

            }
            else
            {
                return View(pakketvm);
            }
        }

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var pakket = await _pakketRepo.GetByIdAsync(id);
        //    if (pakket == null) return View("Error");
        //}
    }
}
