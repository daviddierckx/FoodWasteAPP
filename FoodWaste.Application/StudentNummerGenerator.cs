using FoodWaste.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWaste.Application
{
    public static  class StudentNummerGenerator
    {

        static Random random = new Random();
        public static string GenerateStudentNumber()
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < 7; i++)
            {
                int number = random.Next(10);
                builder.Append(number);
            }

            return builder.ToString();
        }

    }
}
