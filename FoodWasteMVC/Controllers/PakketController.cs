using FoodWaste.Application;
using FoodWaste.Application.Interfaces;
using FoodWaste.Application.Services;
using FoodWaste.Application.ViewModels;
using FoodWaste.Domain;
using FoodWaste.Domain.Enums;
using FoodWaste.Infrastructure.Data;
using FoodWaste.Infrastructure.Repository;
using FoodWasteMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IMedewerkerRepo _medewerkerRepo;

        public PakketController(IPakketRepo pakketRepo, IProductRepo productRepo ,IUserRepo userRepo, IHttpContextAccessor httpContextAccessor, IMedewerkerRepo medewerkerRepo, ApplicationDbContext context)
        {
            _pakketRepo = pakketRepo;
            _httpContextAccessor = httpContextAccessor;
            _productRepo = productRepo;
            _userRepo = userRepo;
            _context = context;
            _medewerkerRepo = medewerkerRepo;
        }
        public async Task<IActionResult> Index(string sortExpression="",string sortKantine="",string sortStad = "",string sortMaaltijd="")
        {
            ViewData["SortParamDate"] = "date";
            ViewData["SortParamKantine"] = "";
            ViewData["SortParamStad"] = "";
            ViewData["SortParamMaaltijd"] = "";

            ViewData["SortIconDate"] = "";
            ViewData["SortIconDesc"] = "";

            ViewData["CurrentDataParam"] = "";

            ViewData["Meerderjarig"] = "";

            SortOrder sortOrder;
            string sortOrderKantine;
            string sortOrderStad;
            string sortOrderMaaltijd;

            string sortproperty;
            string sortpropertyKantine;
            string sortpropertyStad;
            string sortpropertyMaaltijd;
            
            if (User.IsInRole("student"))
            {
                var student =  _userRepo.GetStudentByAppuserId();
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
            }
            else { }
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
                    sortpropertyKantine = "kantine"; 
                    ViewData["SortParamKantine"] = "lx";

                    break;

                case "la":
                    sortOrderKantine = "LA";
                    sortpropertyKantine = "kantine"; 
                    ViewData["SortParamKantine"] = "la";

                    break;
                case "hl":
                    sortOrderKantine = "HL";
                    sortpropertyKantine = "kantine";
                    ViewData["SortParamKantine"] = "hl";

                    break;
                case "ld":
                    sortOrderKantine = "LD";
                    sortpropertyKantine = "kantine";
                    ViewData["SortParamKantine"] = "ld";

                    break;
                default :
                    sortOrderKantine = "";
                    sortpropertyKantine = "kantine"; 
                    ViewData["SortParamKantine"] = "";

                    break;

            }
            switch (sortStad.ToLower())
            {
                case "breda":
                    sortOrderStad = "breda";
                    sortpropertyStad = "stad";
                    ViewData["SortParamStad"] = "breda";

                    break;

                case "denbosch":
                    sortOrderStad = "denbosch";
                    sortpropertyStad = "stad";
                    ViewData["SortParamStad"] = "denbosch";

                    break;
                case "mijnstad":
                    sortOrderStad = _userRepo.GetStudentByAppuserId().StudieStad.ToLower();
                    sortpropertyStad = "stad";
                    ViewData["SortParamStad"] = _userRepo.GetStudentByAppuserId().StudieStad.ToLower();
                    
                    break;
                default:
                    sortOrderStad = "";
                    sortpropertyStad = "stad";
                    ViewData["SortParamStad"] = "";
                    break;

            }
            switch (sortMaaltijd.ToLower())
            {
                case "brood":
                    sortOrderMaaltijd = "brood";
                    sortpropertyMaaltijd = "maaltijd";
                    ViewData["SortParamMaaltijd"] = "brood";

                    break;

                case "warm":
                    sortOrderMaaltijd = "warm";
                    sortpropertyMaaltijd = "maaltijd";
                    ViewData["SortParamMaaltijd"] = "warm";

                    break;
                case "drank":
                    sortOrderMaaltijd = "drank";
                    sortpropertyMaaltijd = "maaltijd";
                    ViewData["SortParamMaaltijd"] = "drank";

                    break;
                default:
                    sortOrderMaaltijd = "";
                    sortpropertyMaaltijd = "maaltijd";
                    ViewData["SortParamMaaltijd"] = "";
                    break;

            }

            IEnumerable<Pakket> pakkets =  _pakketRepo.GetAll(sortproperty,sortpropertyKantine,sortOrder,sortOrderKantine,sortpropertyStad,sortOrderStad,sortpropertyMaaltijd,sortOrderMaaltijd);
            return View(pakkets);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var pakket =  _pakketRepo.GetByIdAsyncNoTracking(id);
            if (pakket == null) return View("Error");

            var createPakketViewModel = new CreatePakketViewModel
            {
                BeschrijvendeNaam = pakket.BeschrijvendeNaam,
                SelectedProductId = pakket.SelectedProductId,
                ProductCollectie =  _pakketRepo.GetAllProductsFromPakket(pakket.SelectedProductId),
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
        [Authorize(Roles = "kantineMedewerker")]
        public async Task<IActionResult> Create()
        {
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var curKantineMedewerker = await _medewerkerRepo.GetKantineMedewerkerByAppuserId();
            var createPakketViewModel = new CreatePakketViewModel {ProductCollectie = _context.Products.ToList(), AppUserId = curUserID, Kantine = (Locatie)Enum.Parse(typeof(Locatie), curKantineMedewerker.Locatie) };
            return View(createPakketViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "kantineMedewerker")]
        public async Task<IActionResult> Create(CreatePakketViewModel pakketVM)
        {
            ViewData["Alcohol"] = "";
            bool containAlcohol = false;


            pakketVM.ProductCollectie =  _productRepo.GetAll();


            if (pakketVM.SelectIDArray == null)
            {
                pakketVM.ProductCollectie = new Product[] { };
                TempData["Error"] = "No products selected";
                pakketVM.ProductCollectie =  _productRepo.GetAll();
                return View(pakketVM);
            }
            if (pakketVM.ProductCollectie == null)
            {
                TempData["Error"] = "No products selected";
            }
            var producten = string.Join(",", pakketVM.SelectIDArray);
            for (int i = 0; i < pakketVM.SelectIDArray.Length; i++)
            {
                foreach (var item in  _pakketRepo.GetAllProductsFromPakket(pakketVM.SelectIDArray[i]))
                {
                    if(item.Alcohol == true)
                    {
                        containAlcohol = true;
                        ViewData["Alcohol"] = "Alcohol";

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
        [Authorize(Roles = "student")]
        public async Task<IActionResult> Reserve(int id)
        {
           var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var pakket =  _pakketRepo.GetByIdAsyncNoTracking(id);
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
        [Authorize(Roles = "student")]
        public async Task<IActionResult> Reserve(int id, CreatePakketViewModel pakketvm)
        {

            var userPakket =  _pakketRepo.GetByIdAsyncNoTracking(id);
            var curUserID = _httpContextAccessor.HttpContext?.User.GetUserId();
            var student =  _userRepo.GetStudentByAppuserId();
           
            List<int> studentIds = new List<int>();
            if (userPakket.StudentenIds != null)
            {
                studentIds = userPakket.StudentenIds.Split(',').Select(int.Parse).ToList();
            }
            if(studentIds.Count == 0)
            {
                studentIds.Add(student.Id);
            }
            else
            {
                TempData["Error"] = "Dit pakket is al gereserveerd.";
                return RedirectToAction("Index");
            }
            var studentenlijst = string.Join(",", studentIds);

            if (_pakketRepo != null)
            {
              
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
                    StudentenIds = studentenlijst,
                    AppUserId = curUserID,

                };


                var bday = student.Geboortedatum;
                DateTime now = DateTime.Today;
                int age = now.Year - bday.Year;
                if (age < 18 && pakketvm.Meerderjarig)
                {
                    TempData["Error"] = "Reservatie geweigerd. Gebruiker is minderjarig en mag dit pakket niet reserveren.";
                    return RedirectToAction("Index");
                }

                IEnumerable<Pakket> PakketUser = _pakketRepo.GetPakketsEqualCurUserID(curUserID);
                IEnumerable<Pakket> lijstTijdOphalen = PakketUser.Where(r => r.TijdOphalen == pakketUpdate.TijdOphalen);
                if(lijstTijdOphalen.Count() > 0)
                {
                    TempData["Error"] = "Je mag maximaal 1 pakket per afhaaldag reserveren";
                    return RedirectToAction("Index");
                }

               
                _pakketRepo.Update(pakketUpdate);
                return RedirectToAction("Index");

            }
            else
            {
                return View(pakketvm);
            }
        }
        [Authorize(Roles = "kantineMedewerker")]
        public async Task<IActionResult> Edit(int id)
        {
           


            var pakket =  _pakketRepo.GetByIdAsyncNoTracking(id);
            if (pakket.StudentenIds == null)
            {
                ViewData["TijdOphalen"] = pakket.TijdOphalen.ToString("yyyy-MM-ddTHH:mm");
                ViewData["TijdTotOphalen"] = pakket.TijdTotOphalen;

                if (pakket == null) return View("Error");
                var pakketVM = new EditPakketViewModel
                {
                    BeschrijvendeNaam = pakket.BeschrijvendeNaam,
                    SelectedProductId = pakket.SelectedProductId,
                    ProductCollectie = _context.Products.ToList(),
                    Stad = (Stad)Enum.Parse(typeof(Stad), pakket.Stad),
                    Kantine = (Locatie)Enum.Parse(typeof(Locatie), pakket.Kantine),
                    TijdOphalen = pakket.TijdOphalen,
                    TijdTotOphalen = pakket.TijdTotOphalen,
                    Meerderjarig = pakket.Meerderjarig,
                    Prijs = pakket.Prijs,
                    TypeMaaltijd = (Maaltijd)Enum.Parse(typeof(Maaltijd), pakket.TypeMaaltijd),
                };
                return View(pakketVM);
            }
            else {
                TempData["Error"] = "Wijzigen van een pakket, mag alleen als er nog geen reserveringen voor zijn";
                return RedirectToAction("Index");


            }
        }

        //TODO Edit Post



        [Authorize(Roles = "kantineMedewerker")]
        public async Task<IActionResult> Delete(int id)
        {
            var pakketDetails =  _pakketRepo.GetByIdAsyncNoTracking(id);
            if (pakketDetails == null) return View("Error");
            if (pakketDetails.StudentenIds == null)
            {
                return View(pakketDetails);
            }
            else
            {
                TempData["Error"] = "Verwijderen van een pakket, mag alleen als er nog geen reserveringen voor zijn";
                return RedirectToAction("Index");
            }
        }

        [HttpPost,ActionName("Delete")]
        public async Task<IActionResult> DeletePakket(int id)
        {
            var pakketDetails =  _pakketRepo.GetByIdAsyncNoTracking(id);
            if (pakketDetails == null) return View("Error");

            _pakketRepo.Delete(pakketDetails);
            return RedirectToAction("Index");
        }
    }
}
