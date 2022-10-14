using FoodWaste.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.Interfaces
{
    public interface IStudentRepo
    {
        Task<IEnumerable<Student>> GetAll();
        Task<IEnumerable<Student>> GetAllStudentByStudentnummer(string nummer);
        Task<Student> GetByIdAsync(int id);
        Task<Student> GetByIdAsyncNoTracking(int id);
        bool Add(Student student);
        bool Update(Student student);
        bool Delete(Student student);
        bool Save();
    }
}
