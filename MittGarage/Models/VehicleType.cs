using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MittGarage.Models
{
    public class VehicleType
    {
        [Key]
        public int VTID { get; set; }

        public string VType { get; set; }

        public VehicleType(string type)
        {
            VType = type;
        }
        public VehicleType(){}
    }
}