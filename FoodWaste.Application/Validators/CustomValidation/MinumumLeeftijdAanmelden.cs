using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.Validators.CustomValidation
{
    public class MinumumLeeftijdAanmelden:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime propValue = Convert.ToDateTime(value);
            if ((propValue < DateTime.Now.AddYears(-16)))
                return true;
            else
                return false;
        }
    }
}
