using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using MittGarage.DataAccessLayer;
using MittGarage.Models;


namespace MittGarage.Models
{
    public enum ColorType
    {
        black,
        white,
        red,
        green,
        yellow,
        blue,
        cyan,
        none
    };


    /// <summary>
    /// The base of all items in the garage.
    /// </summary>
    [Serializable()]
    public class Vehicle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int? VTID {get; set; }

        public string Typename { get; set; }

        [Required]
        public int? OwnerID {get; set; }

        public string OwnerName { get; set; }

        public DateTime checkInDate { get; set; }

        public ColorType Color { get; set; }

        public int Wheels { get; set; }

        public string RegNr { get; set; }

        public string Brand { get; set; }


        // Two Vehicles are the same if they have the same Id.
        public override bool Equals(System.Object other)
        {
            if (other == null) return false;
            Vehicle v = other as Vehicle;
            if ((System.Object)v == null) return false;
            return Id == v.Id;
        }


        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }


        public TimeSpan TimeParked()
        {
            return DateTime.Now - checkInDate;
        }


        public Vehicle(string owner, DateTime? now = null)
        {
            this.OwnerName = owner;
            Color = ColorType.none;
            RegNr = null;
            Wheels = -1;
            checkInDate = now == null ? DateTime.Now : (DateTime)now;
        }

        public Vehicle() : this("Unknown") { }
    }
}
