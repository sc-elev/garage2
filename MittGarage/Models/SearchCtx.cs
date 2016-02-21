using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MittGarage.Models
{
    public class SearchCtx
    {
        [Display(Name = "Registreringsnummer/ägare")]
        public string Searchstring {set; get; }
        
        [Display(Name = "Endast idag")]
        public bool OnlyToday { set; get; }
       
        public string Typestring { set; get; }
        [Display(Name = "Fordonstyp")]
        public VehicleType Type { set; get; }

        public SearchCtx(string search, string today, VehicleType type)
        {
            bool parsedToday;

            Searchstring = search;
            if (!Boolean.TryParse(today, out parsedToday))
            {
                throw new FormatException("Illegal bool string");
            }
            OnlyToday = parsedToday;
        }

        public SearchCtx() {}
    }
}
