using FoodWaste.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.Interfaces
{
    public interface IProductRepo
    {
        Task<IEnumerable<Product>> GetAll();
        Task<Product> GetByIdAsync(int id);
        Task<Product> GetByIdAsyncNoTracking(int id);

        Task<IEnumerable<Product>> GetProductByName(string name);
        bool Add(Product product);
        bool Update(Product product);
        bool Delete(Product product);
        bool Save();
        //Alle repositories krijge dezelfde soort pattern TODO
    }
}
