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

        protected string _typename = "?";
        protected string _ownername;
        protected int? _ownerID = (int?)null;
        protected int? _VTID = (int?)null;

        [Required]
        public int? VTID
        {
            get { return VehicleType == null?  _VTID : VehicleType.VTID; }
            set { _VTID = value; }
        }

        public string Typename
        {
            get { return VehicleType != null ? VehicleType.VType : _typename; }
            set { _typename = value; }
        }

        [ForeignKey("VTID")]
        public VehicleType VehicleType { get; set; }

        [Required]
        public int? OwnerID
        {
            get { return Owner == null ? _ownerID : Owner.OwnerID; }
            set { _ownerID = value;  }
        }

        public string OwnerName
        {
            get { return Owner == null ? _ownername : Owner.Name; }
            set { _ownername = value; }
        }

        [ForeignKey("OwnerID")]
        public Owner Owner { get; set;}

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


        protected ColorType ParseColor(string s)
        {
            try
            {
                return (ColorType)Enum.Parse(typeof(ColorType), s.ToLower());
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Illegal color: " + s);
            }
        }


        private int GetIdByType(string type)
        {
            var db = new ItemContext();
            IList found = db.Types.Where(t => t.VType == type).ToList();
            if (found.Count == 0)
            {
                db.Types.Add(new VehicleType{VType = type});
                db.SaveChanges();
            }

            VehicleType vt = db.Types.Where(ty => ty.VType == type).First();
            return vt.VTID;
        }


        private int GetIdByName(string name)
        {
            var db = new ItemContext();
            IList found = db.Owner.Where(t => t.Name == name).ToList();
            if (found.Count == 0)
            {
                //db.Owner.Add(new Owner { Name = name }); FIXME
                db.SaveChanges();
            }

            Owner owner = db.Owner.Where(ty => ty.Name == name).First();
            return owner.OwnerID;
        }


        public Vehicle(string owner, DateTime? now = null)
        {
            this.OwnerID = GetIdByName(owner);
            Color = ColorType.none;
            RegNr = null;
            Wheels = -1;
            checkInDate = now == null ? DateTime.Now : (DateTime)now;
        }


        public Vehicle() : this("Unknown") { }
    }
}
