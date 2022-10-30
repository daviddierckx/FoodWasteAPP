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
        public async Task<IEnumerable<Pakket>> GetAllPackets()
        {

            return await _context.Pakkets.ToListAsync();
        }
        public IEnumerable<Pakket> GetAll(string SortProperty, string SortPropertyKantine, SortOrder sortOrder, string sortOrderKantine,string sortpropertyStad,string sortOrderStad,string sortpropertyMaaltijd,string sortOrderMaaltijd)
        {
            IEnumerable<Pakket> pakkets =  _context.Pakkets.ToList();
           
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
            if (sortpropertyStad.ToLower() == "stad")
            {
                if (sortOrderStad == "breda")
                    pakkets = pakkets.Where(p => p.Stad == "Breda");
                else if (sortOrderStad == "denbosch")
                    pakkets = pakkets.Where(p => p.Stad == "DenBosch");
                else if (sortOrderStad == "tilburg")
                    pakkets = pakkets.Where(p => p.Stad == "Tilburg");
               
            }
            if (sortpropertyMaaltijd.ToLower() == "maaltijd")
            {
                if (sortOrderMaaltijd == "brood")
                    pakkets = pakkets.Where(p => p.TypeMaaltijd == "Brood");
                else if (sortOrderMaaltijd == "warm")
                    pakkets = pakkets.Where(p => p.TypeMaaltijd == "Warm");
                else if (sortOrderMaaltijd == "drank")
                    pakkets = pakkets.Where(p => p.TypeMaaltijd == "Drank");

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

        public  IEnumerable<Pakket>GetAllPaketsByProduct(string product)
        {
            //TODO
            return  _context.Pakkets.Where(c => c.Stad.ToString() == product).ToList();
        }

        public Pakket GetByIdAsync(int id)
        {
            return _context.Pakkets.SingleOrDefault(c => c.Id == id);
        }
        public  Pakket GetByIdAsyncNoTracking(int id)
        {
            //Product product = _context.Products.Include(a => a.Address).FirstOrDefault(c => c.Id == id); TODO

            return _context.Pakkets.AsNoTracking().FirstOrDefault(i => i.Id == id);
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

        public IEnumerable<Product> GetAllProductsFromPakket(string productId)
        {
            List<int> productIds = productId.Split(',').Select(int.Parse).ToList();
           
            
            
            return  _context.Products.Where(c => productIds.Contains(c.Id)).ToList();
        }

        public IEnumerable<Pakket> GetPakketsEqualCurUserID(string curUserID)
        {
            return _context.Pakkets.Where(r => r.AppUserId == curUserID);
        }
    }
}
