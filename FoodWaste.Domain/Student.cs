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
    public class Student
    {
        [Key]
        public int Id { get; set; }
        public string Naam { get; set; }
        //Minimaal 16 TODO
        public DateTime Geboortedatum { get; set; }
        public string EmailAdress { get; set; }
        public string TelefoonNummer { get; set; }
        public string Studentnummer { get; set; }
        public string StudieStad { get; set; }
        [ForeignKey("AppUser")]
        public string AppUserId { get; set; }
        public AppUser User;
        public ICollection<Pakket>? Pakketen { get; set; }

    }
}
