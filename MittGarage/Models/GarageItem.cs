using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MittGarage.Models
{
    public enum enumType
    {
        car, bike, airplane, oljetanker
    }
    public class GarageItem
    {
        [Key]
        public int ItemID { get; set; }
        public enumType Vehicle { get; set; }
        public string regNR { get; set; }
        public string Owner { get; set; }
        public DateTime Inchecked { get; set; }


    }
}