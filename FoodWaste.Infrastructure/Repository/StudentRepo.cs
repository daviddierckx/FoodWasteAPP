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
    public class StudentRepo : IStudentRepo
    {
        private readonly ApplicationDbContext _context;
        public StudentRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Student student)
        {
            _context.Add(student);
            return Save();
        }

        public bool Delete(Student student)
        {
            _context.Remove(student);
            return Save();
        }

        public async Task<IEnumerable<Student>> GetAll()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<IEnumerable<Student>> GetAllStudentByStudentnummer(string student)
        {
            return await _context.Students.Where(c => c.Studentnummer.ToString() == student).ToListAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _context.Students.Include(i => i.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Student> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Students.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Student student)
        {
            _context.Update(student);
            return Save();
        }
    }
}
