using FoodWaste.Application;
using FoodWaste.Application.Interfaces;
using FoodWaste.Domain;
using FoodWaste.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        public List<Pakket> GetAllStudentPakkets()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userPakket = _context.Pakkets.Where(r => r.AppUserId == curUser);
            return userPakket.ToList();
        }

        public  Student GetStudentByAppuserId()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            Student student =  _context.Students.AsNoTracking().FirstOrDefault(r => r.AppUserId == curUser);
            return student;
        }
    }
}
