using FoodWaste.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.Interfaces
{
    public interface IMedewerkerRepo
    {
        Task<IEnumerable<KantineMedewerker>> GetAll();
        Task<KantineMedewerker> GetByIdAsync(int id);
        Task<KantineMedewerker> GetByIdAsyncNoTracking(int id);
        Task<KantineMedewerker> GetKantineMedewerkerByAppuserId();

        bool Add(KantineMedewerker student);
        bool Update(KantineMedewerker student);
        bool Delete(KantineMedewerker student);
        bool Save();
    }
}
