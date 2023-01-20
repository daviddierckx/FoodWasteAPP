using FoodWaste.Domain.Enums;
using FoodWaste.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using FoodWaste.Application.Validators.CustomValidation;

namespace FoodWaste.Application.ViewModels
{
    public class CreatePakketViewModel
    {

        public int Id { get; set; }
        public string BeschrijvendeNaam { get; set; }

        public string? SelectedProductId { get; set; }

        [NotMapped]
        public IEnumerable<Product>? ProductCollectie { get; set; }
        [NotMapped]
        public string[]? SelectIDArray { get; set; }
        public Stad Stad { get; set; }

        public Locatie Kantine { get; set; }

        [DisplayName("Tijd Ophalen")]
        [DataType(DataType.Date)]
        [DateGreaterThan(ErrorMessage = "Datum moet gelijk of groter zijn dan de huidige datum")]
        [DateMaxTwoDayAHead(ErrorMessage = "Maximaal 2 dagen vooruit plannen")]
        public DateTime TijdOphalen { get; set; }
        [DisplayName("Tijd Tot Ophalen")]
        [DataType(DataType.Date)]
        [DateEqualTijdOphalen(ErrorMessage = "Datum moet gelijk zijn aan de ophaaltijd")]
        public DateTime TijdTotOphalen { get; set; }
        public bool Meerderjarig { get; set; }
        public int Prijs { get; set; }
        public Maaltijd TypeMaaltijd { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? GereserveerdDoor { get; set; }

    }
}
