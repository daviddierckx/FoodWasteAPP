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
        List<Pakket> GetAllStudentPakkets();
        Student GetStudentByAppuserId();

    }

}
