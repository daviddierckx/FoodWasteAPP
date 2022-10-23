using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.Validators.CustomValidation
{
    internal class DateGreaterThanTijdOphalen:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime propValue = Convert.ToDateTime(value);
            if (propValue > DateGreaterThan.dateTimeTijdOphalen)
                return true;
            else
                return false;
        }
    }
}
