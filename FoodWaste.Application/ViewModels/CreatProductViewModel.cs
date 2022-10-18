using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.ViewModels
{
    public class CreatProductViewModel
    {
        public int Id { get; set; }
        public string BeschrijvendeNaam { get; set; }
        public bool Alcohol { get; set; }
        public IFormFile Foto { get; set; }
    }
}
