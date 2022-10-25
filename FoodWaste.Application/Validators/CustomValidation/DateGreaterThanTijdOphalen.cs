using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application.Validators.CustomValidation
{
    internal class DateEqualTijdOphalen:ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime propValue = Convert.ToDateTime(value);
            if (propValue.Date == DateGreaterThan.dateTimeTijdOphalen.Date)
                return true;
            else
                return false;
        }
    }
}
