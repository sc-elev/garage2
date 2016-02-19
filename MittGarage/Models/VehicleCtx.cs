using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MittGarage.Models
{
    public class VehicleCtx
    {
        public string RegNr { get; set; }
        public string Owner { get; set; }
        public string CheckInDate { get; set; }
        public string Descriminator { get; set; }
    }
}