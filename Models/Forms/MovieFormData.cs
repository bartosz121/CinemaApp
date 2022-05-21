using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaApp.Models.Forms
{
    public class MovieFormData
    {
        public MovieFormData(string title, int year)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException();
            }
            Title = title;
            Year = year;
        }

        public string Title { get; set; }
        public int Year { get; set; }
    }
}
