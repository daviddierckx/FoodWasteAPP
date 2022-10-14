using FoodWaste.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.Interfaces
{
    public interface IPakketRepo
    {
        Task<IEnumerable<Pakket>> GetAll();
        Task<Pakket> GetByIdAsync(int id);
        Task<Pakket> GetByIdAsyncNoTracking(int id);
        Task<IEnumerable<Pakket>> GetAllPaketsByProduct(string product);
        bool Add(Pakket pakket);
        bool Update(Pakket pakket);
        bool Delete(Pakket pakket);
        bool Save();

    }
}
