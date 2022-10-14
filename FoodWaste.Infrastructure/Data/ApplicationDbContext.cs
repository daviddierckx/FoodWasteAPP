using FoodWaste.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace FoodWaste.Infrastructure.Data
{
    public class ApplicationDbContext:IdentityDbContext<AppUser>
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Pakket> Pakkets { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<KantineMedewerker> KantineMedewerkers { get; set; }
        public DbSet<Kantine> Kantine { get; set; }



    }
}
