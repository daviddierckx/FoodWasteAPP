using FoodWaste.Domain;
using FoodWaste.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.Interfaces
{
    public interface IPakketRepo
    {
        Task<IEnumerable<Pakket>> GetAllPackets();
        IEnumerable<Pakket> GetAll(string SortProperty, string SortPropertyKantine, SortOrder sortOrder, string sortOrderKantine,string sortpropertyStad,string sortOrderStad, string sortpropertyMaaltijd, string sortOrderMaaltijd);
        Pakket GetByIdAsync(int id);
        Pakket GetByIdAsyncNoTracking(int id);
        IEnumerable<Pakket> GetAllPaketsByProduct(string product);
        IEnumerable<Product> GetAllProductsFromPakket(string productId);
        IEnumerable<Pakket> GetPakketsEqualCurUserID(string curUserID);
        bool Add(Pakket pakket);
        bool Update(Pakket pakket);
        bool Delete(Pakket pakket);
        bool Save();

    }
}
