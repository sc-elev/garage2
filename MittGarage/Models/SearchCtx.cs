﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace MittGarage.Models
{
    public class SearchCtx
    {
        public string Searchstring {set; get; }
        public bool OnlyToday { set; get; }
        public string Typestring { set; get; }
        public VehicleType Type { set; get; }

        public SearchCtx(string search, string today, string type)
        {
            bool parsedToday;

            Searchstring = search;
            if (!Boolean.TryParse(today, out parsedToday))
            {
                throw new FormatException("Illegal bool string");
            }
            OnlyToday = parsedToday;
            Typestring = type;
        }

        public SearchCtx() {}
    }
}