using FoodWaste.Application;
using FoodWaste.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServiceStack.Messaging;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FoodWast.API.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public AccountController(
                UserManager<AppUser> userManager,
                SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor,
                 IConfiguration configuration
               )
             {
                         _userManager = userManager;
                         _signInManager = signInManager;
                        _httpContextAccessor = httpContextAccessor;
                         _configuration = configuration;
              }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var message = "";
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
               return BadRequest("Login is empty");


            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid Login and/or password");

            }

            if (!user.EmailConfirmed)
            {
                return BadRequest("Something went wrong");
            }

            var passwordSignInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!passwordSignInResult.Succeeded)
            {
                return BadRequest("Invalid Login and/or password");
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("userID",user.Id.ToString()),
                new Claim("email",user.UserName),

            };

            var token = this.getToken(authClaims);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });

        }


        [HttpGet]
        [Route("Users/current")] //HIER
        public async Task<IActionResult> getLoggedInUserID()
        {
            string id = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
           
            return Ok(new { userID = id });
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok("You have been successfully logged out");
        }


        private JwtSecurityToken getToken(List<Claim> authClaim)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(24),
                claims: authClaim,
                signingCredentials: new SigningCredentials(authSigningKey,SecurityAlgorithms.HmacSha256)
                );
            return token;
        }
    }
}
