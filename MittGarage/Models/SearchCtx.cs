using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PagedList;


namespace MittGarage.Models
{
    public class SearchCtx
    {
        [Display(Name = "Registreringsnummer/ägare")]
        public string Searchstring {set; get; }

        [Display(Name = "Endast idag")]
        public bool OnlyToday { set; get; }

        [Display(Name = "Fordonstyp")]
        public string Typestring { set; get; }

        public IPagedList<Vehicle> PagedResults { set; get; }

        public IList<Vehicle> Results { set; get; }

        public bool IsPristine()
        {
            return String.IsNullOrEmpty(Searchstring)
                   && !OnlyToday
                   && string.IsNullOrEmpty(Typestring);
        }


        public bool IsEmpty()
        {
            return string.IsNullOrEmpty(Searchstring)
                    && !OnlyToday
                    && string.IsNullOrEmpty(Typestring);
        }

        public void Sort(string sortKey)
        {
            switch (sortKey)
            {
                case "id":
                    Results = Results.OrderByDescending(s => s.Id).ToList();
                    break;
                case "owner":
                    Results = Results.OrderBy(s => s.Owner).ToList();
                    break;
                case "regnr":
                    Results = Results.OrderBy(s => s.RegNr).ToList();
                    break;
                case "checkintime":
                case "time":
                    Results = Results.OrderBy(s => s.checkInDate).ToList();
                    break;
                case "type":
                    Results = Results.OrderByDescending(s => s.Typename).ToList();
                    break;
                default:
                    throw new ArgumentException("Bad sort key: " + sortKey);
            }
            PagedResults = Results.ToPagedList(1, 10);
        }

        public SearchCtx(string search, string today, VehicleType type)
        {
            bool parsedToday;

            Searchstring = search;
            if (!Boolean.TryParse(today, out parsedToday))
            {
                throw new FormatException("Illegal bool string");
            }
            OnlyToday = parsedToday;
            Results = new List<Vehicle>();
            PagedResults = Results.ToPagedList(1, 10);
        }

        public SearchCtx() : this ("", "false", new VehicleType{ VTID = 0 }) {}
    }
}
