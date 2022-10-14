using FoodWaste.Application.Interfaces;
using FoodWaste.Domain;
using FoodWaste.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Infrastructure.Repository
{
    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserRepo(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor) //you can acces object directly from the webpage
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Pakket>> GetAllStudentPakkets()
        {
            var curUser = _httpContextAccessor.HttpContext?.User;
            var userPakket = _context.Pakkets.Where(r => r.GereserveerdDoor.AppUserId == curUser.ToString());
            return userPakket.ToList();
        }
    }
}
