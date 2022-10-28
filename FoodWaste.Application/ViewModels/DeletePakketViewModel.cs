using FoodWaste.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.ViewModels
{
    public class DeletePakketViewModel
    {
        public int Id { get; set; }
        public string BeschrijvendeNaam { get; set; }
        public string? SelectedProductId { get; set; }
        public ICollection<Product>? ProductCollectie { get; set; }

        public string Stad { get; set; }
        public string Kantine { get; set; }
        public DateTime TijdOphalen { get; set; }
        public DateTime TijdTotOphalen { get; set; }
        public bool Meerderjarig { get; set; }
        public int Prijs { get; set; }
        public string TypeMaaltijd { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? GereserveerdDoor { get; set; }

        public string? StudentenIds { get; set; }
        public Student? Student { get; set; }
    }
}
