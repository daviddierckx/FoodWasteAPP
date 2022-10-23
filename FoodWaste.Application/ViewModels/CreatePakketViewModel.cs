using FoodWaste.Domain.Enums;
using FoodWaste.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using FoodWaste.Application.Validators.CustomValidation;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

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
        [DateGreaterThan(ErrorMessage = "Date should be equal or greater than current date")]
        [DateMaxTwoDayAHead(ErrorMessage = "Maximaal 2 dagen vooruit plannen")]
        public DateTime TijdOphalen { get; set; }
        [DisplayName("Tijd Tot Ophalen")]
        [DataType(DataType.Date)]
        [DateGreaterThanTijdOphalen(ErrorMessage = "Date should be greater than tijd ophalen")]
        public DateTime TijdTotOphalen { get; set; }
        public bool Meerderjarig { get; set; }
        public int Prijs { get; set; }
        public Maaltijd TypeMaaltijd { get; set; }
        public string? AppUserId { get; set; }
        public AppUser? GereserveerdDoor { get; set; }

    }
}
