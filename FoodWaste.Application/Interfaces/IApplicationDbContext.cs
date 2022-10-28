using FoodWaste.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.Interfaces
{
    public interface IApplicationDbContext
    {
        public DbSet<Pakket> Pakkets { get; set; }

    }
}
