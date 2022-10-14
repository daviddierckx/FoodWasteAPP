using FoodWaste.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.Interfaces
{
    public interface IUserRepo
    {
        Task<List<Pakket>> GetAllStudentPakkets();
    }
}
