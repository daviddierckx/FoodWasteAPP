using FoodWaste.Application.Interfaces;
using FoodWaste.Domain;
using FoodWaste.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodWast.API.Controllers
{
    public class ReserveController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IPakketRepo _pakketRepo;
        private readonly IUserRepo _userRepo;


        public ReserveController(
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor,
                 IConfiguration configuration,IPakketRepo pakketRepo,IUserRepo userRepo
               )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _pakketRepo = pakketRepo;
            _userRepo = userRepo;
        }

        [HttpPut]
        [Route("api/Reserve/{id}")]
        public async Task<IActionResult> Reserve(int id)
        {
            string userID = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID == null)
            {
                return BadRequest("User not logged in");
            }
            if (User.IsInRole("student"))
            {
                var userPakket = _pakketRepo.GetByIdAsyncNoTracking(id);
                if (userPakket == null)
                    return BadRequest("Pakket not found");
                var student = _userRepo.GetStudentByAppuserId();
                List<int> studentIds = new List<int>();
                if (userPakket.StudentenIds != null)
                {
                    studentIds = userPakket.StudentenIds.Split(',').Select(int.Parse).ToList();
                }
                if (studentIds.Count == 0)
                {
                    studentIds.Add(student.Id);
                }
                else
                {
                    return BadRequest("Dit pakket is al gereserveerd.");
                }
                var studentenlijst = string.Join(",", studentIds);

                var pakketUpdate = new Pakket
                {
                    Id = id,
                    BeschrijvendeNaam = userPakket.BeschrijvendeNaam,
                    SelectedProductId = userPakket.SelectedProductId,
                    Stad = userPakket.Stad.ToString(),
                    Kantine = userPakket.Kantine.ToString(),
                    TijdOphalen = userPakket.TijdOphalen,
                    TijdTotOphalen = userPakket.TijdTotOphalen,
                    Meerderjarig = userPakket.Meerderjarig,
                    Prijs = userPakket.Prijs,
                    TypeMaaltijd = userPakket.TypeMaaltijd.ToString(),
                    StudentenIds = studentenlijst,
                    AppUserId = userID,

                };
                _pakketRepo.Update(pakketUpdate);

                return Ok(new { userID = pakketUpdate });
            }
            else
            {
                return BadRequest("Enkel een student kan een reservatie plaatsen.");
            }
        }
    }
}
