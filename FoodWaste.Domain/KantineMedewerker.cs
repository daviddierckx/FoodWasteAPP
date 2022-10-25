using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodWaste.Domain.Enums;

namespace FoodWaste.Domain
{
    public class KantineMedewerker
    {
        [Key]
        public int Id { get; set; }
        public string Naam { get; set; }
        public string Personeelsnummer { get; set; }
        public string Locatie { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public AppUser User;
    }
   
}
