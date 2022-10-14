using FoodWaste.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Domain
{
    public class Kantine
    {
        [Key]
        public int Id { get; set; }
        public Stad Stad { get; set; }
        public Locatie Locatie { get; set; }
        public bool WarmeMaaltijd { get; set; }
    }
}
