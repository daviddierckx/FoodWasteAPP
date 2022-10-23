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
        private readonly IUserRepo _userRepo;
        public PakketController(IPakketRepo pakketRepo, IProductRepo productRepo ,IUserRepo userRepo, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _pakketRepo = pakketRepo;
            _httpContextAccessor = httpContextAccessor;
            _productRepo = productRepo;
            _userRepo = userRepo;
            _context = context;
        }
        public async Task<IActionResult> Index(string sortExpression="",string sortKantine="")
        {
            ViewData["SortParamDate"] = "date";
            ViewData["SortParamKantine"] = "";

            ViewData["SortIconDate"] = "";
            ViewData["SortIconDesc"] = "";

            ViewData["CurrentDataParam"] = "";

            ViewData["Meerderjarig"] = "";

            SortOrder sortOrder;
            string sortOrderKantine;
            string sortproperty;
            string sortpropertyKantine;

            var student = await _userRepo.GetStudentByAppuserId();

            var bday = student.Geboortedatum;
            DateTime now = DateTime.Today;
            int age = now.Year - bday.Year;
            if (age >= 18)
            {
                ViewData["Meerderjarig"] = "+18";
                Console.WriteLine("Meerderjarig");
            }

            ViewData["Student"] = student.Id;
            ViewData["Reserved"] = false;
            switch (sortExpression.ToLower())
            {
                case "date_desc":
                    sortOrder = SortOrder.Descending;
                    sortproperty = "date";
                    ViewData["SortParamDate"] = "date";
                    ViewData["CurrentDataParam"] = "date";

                    ViewData["SortIconDate"] = "fa fa-arrow-up";

                    break;

          
                default:
                    sortOrder = SortOrder.Ascending;
                    sortproperty = "date";
                    ViewData["SortIconDate"] = "fa fa-arrow-down";
                    ViewData["SortParamDate"] = "date_desc";
                    break;
            }
            switch (sortKantine.ToLower())
            {
                case "lx":
                    sortOrderKantine = "LX";
                    sortpropertyKantine = "kantine"; //aanpassen
                    ViewData["SortParamKantine"] = "lx";

                    break;

                case "la":
                    sortOrderKantine = "LA";
                    sortpropertyKantine = "kantine"; //aanpassen
                    ViewData["SortParamKantine"] = "la";

                    break;
                case "hl":
                    sortOrderKantine = "HL";
                    sortpropertyKantine = "kantine"; //aanpassen
                    ViewData["SortParamKantine"] = "hl";

                    break;
                case "ld":
                    sortOrderKantine = "LD";
                    sortpropertyKantine = "kantine"; //aanpassen
                    ViewData["SortParamKantine"] = "ld";

                    break;
                default :
                    sortOrderKantine = "";
                    sortpropertyKantine = "kantine"; //aanpassen
                    ViewData["SortParamKantine"] = "";

                    break;

            }

            IEnumerable<Pakket> pakkets = await _pakketRepo.GetAll(sortproperty,sortpropertyKantine,sortOrder,sortOrderKantine);
            return View(pakkets.Where(r => r.AppUserId == null));
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePakketViewModel pakketVM)
        {
            ViewData["Alcohol"] = "";
            bool containAlcohol = false;
            pakketVM.ProductCollectie = _context.Products.ToList();
            if (pakketVM.SelectIDArray == null)
            {
                pakketVM.ProductCollectie = new Product[] { };
                TempData["Error"] = "No products selected";
                pakketVM.ProductCollectie = _context.Products.ToList();
                return View(pakketVM);
            }
            if (pakketVM.ProductCollectie == null)
            {
                TempData["Error"] = "No products selected";
            }
            var producten = string.Join(",", pakketVM.SelectIDArray);
            for (int i = 0; i < pakketVM.SelectIDArray.Length; i++)
            {
                foreach (var item in await _pakketRepo.GetAllProductsFromPakket(pakketVM.SelectIDArray[i]))
                {
                    if(item.Alcohol == true)
                    {
                        containAlcohol = true;
                    }
                } 
            }
            if (ModelState.IsValid)
            {


                var pakket = new Pakket

                {
                    BeschrijvendeNaam = pakketVM.BeschrijvendeNaam,
                    SelectedProductId = producten,
                    Stad = pakketVM.Stad.ToString(),
                    Kantine = pakketVM.Kantine.ToString(),
                    TijdOphalen = pakketVM.TijdOphalen,
                    TijdTotOphalen = pakketVM.TijdTotOphalen,
                    Meerderjarig = containAlcohol,
                    Prijs = pakketVM.Prijs,
                    TypeMaaltijd = pakketVM.TypeMaaltijd.ToString(),
                    
                };

                _pakketRepo.Add(pakket);
                return RedirectToAction("Index");
            }
            else
            {
                return View(pakketVM);
            }
            
      
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
            var student = await _userRepo.GetStudentByAppuserId();
           
            List<int> studentIds = new List<int>();
            if (userPakket.StudentenIds != null)
            {
                studentIds = userPakket.StudentenIds.Split(',').Select(int.Parse).ToList();
            }
            studentIds.Add(student.Id);
            var studentenlijst = string.Join(",", studentIds);

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
                var pakketUpdate = new Pakket
                {
                    Id = id,
                    BeschrijvendeNaam = pakketvm.BeschrijvendeNaam,
                    SelectedProductId = pakketvm.SelectedProductId,
                    Stad = pakketvm.Stad.ToString(),
                    Kantine = pakketvm.Kantine.ToString(),
                    TijdOphalen = pakketvm.TijdOphalen,
                    TijdTotOphalen = pakketvm.TijdTotOphalen,
                    Meerderjarig = pakketvm.Meerderjarig,
                    Prijs = pakketvm.Prijs,
                    TypeMaaltijd = pakketvm.TypeMaaltijd.ToString(),
                    StudentenIds = studentenlijst
                };
                
                IEnumerable<Pakket> PakketUser = _context.Pakkets.Where(r => r.AppUserId == curUserID);
                IEnumerable<Pakket> lijstTijdOphalen = PakketUser.Where(r => r.TijdOphalen == pakket.TijdOphalen);
                if(lijstTijdOphalen.Count() > 0)
                {
                    TempData["Error"] = "Je mag maximaal 1 pakket per afhaaldag reserveren";
                    return RedirectToAction("Index");
                }
                _pakketRepo.Add(pakket);
                _pakketRepo.Update(pakketUpdate);
                return RedirectToAction("Index");

            }
            else
            {
                return View(pakketvm);
            }
        }

        public async Task<IActionResult> Edit(int id)
        {
           


            var pakket = await _pakketRepo.GetByIdAsyncNoTracking(id);

            ViewData["TijdOphalen"] = pakket.TijdOphalen;
            ViewData["TijdTotOphalen"] = pakket.TijdTotOphalen;

            if (pakket == null) return View("Error");
            var pakketVM = new EditPakketViewModel
            {
                BeschrijvendeNaam = pakket.BeschrijvendeNaam,
                SelectedProductId = pakket.SelectedProductId,
                ProductCollectie  = _context.Products.ToList(),
            Stad = (Stad)Enum.Parse(typeof(Stad), pakket.Stad),
                Kantine = (Locatie)Enum.Parse(typeof(Locatie), pakket.Kantine),
                TijdOphalen =  pakket.TijdOphalen,
                TijdTotOphalen = pakket.TijdTotOphalen,
                Meerderjarig = pakket.Meerderjarig,
                Prijs = pakket.Prijs,
                TypeMaaltijd = (Maaltijd)Enum.Parse(typeof(Maaltijd), pakket.TypeMaaltijd),
            };
            return View(pakketVM);
        }

    //TODO Edit Post

        public async Task<IActionResult> Delete(int id)
        {
            var pakketDetails = await _pakketRepo.GetByIdAsyncNoTracking(id);
            if (pakketDetails == null) return View("Error");
            return View(pakketDetails);
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeletePakket(int id)
        {
            var pakketDetails = await _pakketRepo.GetByIdAsyncNoTracking(id);
            if (pakketDetails == null) return View("Error");

            _pakketRepo.Delete(pakketDetails);
            return RedirectToAction("Index");
        }
    }
}
