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
    public class MedewerkerRepo : IMedewerkerRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MedewerkerRepo(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public bool Add(KantineMedewerker kantineMedewerker)
        {
            _context.Add(kantineMedewerker);
            return Save();
        }

        public bool Delete(KantineMedewerker kantineMedewerker)
        {
            _context.Remove(kantineMedewerker);
            return Save();
        }

        public async Task<IEnumerable<KantineMedewerker>> GetAll()
        {
            return await _context.KantineMedewerkers.ToListAsync();
        }

        public async Task<KantineMedewerker> GetByIdAsync(int id)
        {
            return await _context.KantineMedewerkers.Include(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<KantineMedewerker> GetByIdAsyncNoTracking(int id)
        {
            return await _context.KantineMedewerkers.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(KantineMedewerker kantineMedewerker)
        {
            _context.Update(kantineMedewerker);
            return Save();
        }

        public async Task<KantineMedewerker> GetKantineMedewerkerByAppuserId()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            KantineMedewerker kantineMedewerker = await _context.KantineMedewerkers.AsNoTracking().FirstOrDefaultAsync(r => r.AppUserId == curUser);
            return kantineMedewerker;
        }
    }
}
