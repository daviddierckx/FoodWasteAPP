using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Domain
{
    public class AppUser: Microsoft.AspNetCore.Identity.IdentityUser
    {
        public string Naam { get; set; }
        //Minimaal 16 TODO
        public DateTime Geboortedatum { get; set; }
        public string EmailAdress { get; set; }
        public string TelefoonNummer { get; set; }
     


    }
}
