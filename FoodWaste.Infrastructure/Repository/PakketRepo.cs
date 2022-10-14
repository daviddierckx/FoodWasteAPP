using FoodWaste.Application.Interfaces;
using FoodWaste.Domain;
using FoodWaste.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Infrastructure.Repository
{
    public class PakketRepo : IPakketRepo
    {
        private readonly ApplicationDbContext _context;
        public PakketRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Pakket pakket)
        {
            _context.Add(pakket);
            return Save();
        }

        public bool Delete(Pakket pakket)
        {
            _context.Remove(pakket);
            return Save();
        }

        public async Task<IEnumerable<Pakket>> GetAll()
        {
            return await _context.Pakkets.ToListAsync();
        }

        public async Task<IEnumerable<Pakket>> GetAllPaketsByProduct(string product)
        {
            //TODO
            return await _context.Pakkets.Where(c => c.Stad.ToString() == product).ToListAsync();
        }

        public async Task<Pakket> GetByIdAsync(int id)
        {
            return await _context.Pakkets.Include(i => i.Producten).FirstOrDefaultAsync();
        }
        public async Task<Pakket> GetByIdAsyncNoTracking(int id)
        {
            //Product product = _context.Products.Include(a => a.Address).FirstOrDefault(c => c.Id == id); TODO

            return await _context.Pakkets.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Pakket pakket)
        {
            _context.Update(pakket);
            return Save();
        }
    }
}
