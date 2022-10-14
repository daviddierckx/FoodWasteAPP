using FoodWaste.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Domain
{
    public class Pakket
    {
        [Key]
        public int Id { get; set; }
        public string BeschrijvendeNaam { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        //TODO Change to ICOLLECTION?
       // public List<Product> Producten { get; set; }
        public ICollection<Product> Producten { get; set; }

        public Stad Stad { get;set; }
        public Locatie Kantine { get; set; }
        public DateTime TijdOphalen { get; set; }
        public DateTime TijdTotOphalen { get; set; }
        public bool Meerderjarig { get; set; }
        public int Prijs {get; set; }
        public Maaltijd TypeMaaltijd { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student? GereserveerdDoor { get; set; }

    }
  
}
