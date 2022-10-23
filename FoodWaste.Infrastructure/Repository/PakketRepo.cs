using FoodWaste.Application.Interfaces;
using FoodWaste.Domain;
using FoodWaste.Domain.Enums;
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

        public async Task<IEnumerable<Pakket>> GetAll(string SortProperty, string SortPropertyKantine, SortOrder sortOrder, string sortOrderKantine)
        {
            IEnumerable<Pakket> pakkets = await _context.Pakkets.ToListAsync();

            if (SortPropertyKantine.ToLower() == "kantine"){
                if (sortOrderKantine == "LA")
                    pakkets = pakkets.Where(p => p.Kantine == "LA");
                else if (sortOrderKantine == "LX")
                    pakkets = pakkets.Where(p => p.Kantine == "LX");
                else if (sortOrderKantine == "HL")
                    pakkets = pakkets.Where(p => p.Kantine == "HL");
                else if (sortOrderKantine == "LD")
                    pakkets = pakkets.Where(p => p.Kantine == "LD");
                
            }
            if (SortProperty.ToLower() == "date")
            {
                if(sortOrder == SortOrder.Ascending)
                {
                    pakkets = pakkets.OrderBy(n => n.TijdOphalen).ToList();
                }
                else
                {
                    pakkets = pakkets.OrderByDescending(n => n.TijdOphalen).ToList(); //lambda
                }
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                {
                    pakkets = pakkets.OrderBy(k => k.Kantine).ToList();
                }
                else
                {
                    pakkets = pakkets.OrderByDescending(k => k.Kantine).ToList(); //lambda
                }
            }
            return pakkets;
        }

        public async Task<IEnumerable<Pakket>> GetAllPaketsByProduct(string product)
        {
            //TODO
            return await _context.Pakkets.Where(c => c.Stad.ToString() == product).ToListAsync();
        }

        public async Task<Pakket> GetByIdAsync(int id)
        {
            return await _context.Pakkets.Include(i => i.ProductCollectie).FirstOrDefaultAsync();
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

        public async Task<IEnumerable<Product>> GetAllProductsFromPakket(string productId)
        {
            List<int> productIds = productId.Split(',').Select(int.Parse).ToList();
           
            
            
            return await _context.Products.Where(c => productIds.Contains(c.Id)).ToListAsync();
        }

       


    }
}
