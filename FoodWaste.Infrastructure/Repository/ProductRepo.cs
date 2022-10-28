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
    public class ProductRepo : IProductRepo
    {
        private readonly ApplicationDbContext _context;
        public ProductRepo(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool Add(Product product)
        {
            //generates all the SQL but does not send to database
            _context.Add(product);
            //Save() sends SQL to the database and creates entity 
            return Save();
        }

        public bool Delete(Product product)
        {
            _context.Remove(product);
            return Save();
        }
        //TASK => Returning something when its , so can be nothing
        public IEnumerable<Product> GetAll()
        {
           return  _context.Products.ToList();
        }
        
        public async Task<Product> GetByIdAsync(int id)
        {
            //Product product = _context.Products.Include(a => a.Address).FirstOrDefault(c => c.Id == id); TODO

            return await _context.Products.FirstOrDefaultAsync(i => i.Id == id);
        }
        public async Task<Product> GetByIdAsyncNoTracking(int id)
        {
            //Product product = _context.Products.Include(a => a.Address).FirstOrDefault(c => c.Id == id); TODO

            return await _context.Products.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            return await _context.Products.Where(c => c.BeschrijvendeNaam.Contains(name)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Product product)
        {
            _context.Update(product);
            return Save();
        }
    }
}
