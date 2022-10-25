using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Domain.Enums
{
    public enum Stad
    {
        
            Breda,
            [Display(Name = "Den Bosch")]
            DenBosch,
            Tilburg
        
    }
}
