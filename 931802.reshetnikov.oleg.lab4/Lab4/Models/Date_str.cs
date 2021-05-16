using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    
    public static class Date_str
    {
        public static string[] Days()
        {
            string[]  Days = new string[31];
            for (int i = 0; i < 31; i++)
                Days[i] = (i + 1).ToString();
            return Days;
        }

        public static string[] Years()
        {
            string[] Years = new string[120];
            int year = 2020;
            for (int i = 0; i < 120; i++)
                Years[i] = (year--).ToString();
            return Years;
        }

        public static string[] Months()
        { 
             return new string[]  {"January", "February", "March", "April",
                "May", "June", "July", "August", "September", "October", "November", "December" };
        }
    }
}
