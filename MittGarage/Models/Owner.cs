using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MittGarage.Models
{
    public class Owner
    {
        [Key]
        public int OwnerID { get; set; }

        public string Name { get; set; }
        
        public Owner(string name)
        {
            Name = name;
        }
        public Owner(){}
    }
}