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

    #region --Base of Items in Garage--
    /// <summary>
    /// The base of all items in the garage.
    /// </summary>
    [Serializable()]
    public class Vehicle
    {
        protected static int LastId = 0;

        // Mandatory attributes without defaults.

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        [ForeignKey("OwnerID")]
        public int OwnerID { get; set; }
        public string Owner { get; set; }

        [Required]
        [ForeignKey("VTID")]
        public int VTID {get; set;}
        public string VehicleType { get; set; }

        public DateTime checkInDate { get; protected set; }

        // Searchable, optional attributes.
        public string Color { get; protected set; }

        public int Wheels { get; protected set; }

        public string RegNr { get; set; }

        public string Brand { get; protected set; }
        #endregion Searchable Optional Attributes

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

        // Parse and store commandline attributes.
        // Default implementation takes <reg nr> [type [color]]
        public virtual Vehicle Init(List<string> attr)
        {
            if (attr.Count == 0)
            {
                throw new ArgumentException("Missing required regnr");
            }
            this.RegNr = attr[0];
            if (attr.Count > 1)
            {
                this.Brand = attr[1];
            }
            return this;
        }

        private int GetName (string name)
        {
            var db = new ItemContext();
            IList<Owner> Found = db.Owner.Where(g => g.Name == name).ToList();
            if (Found.Count == 0)
            {
                db.Owner.Add(new Owner(name));
                db.SaveChanges();
            }

            Owner o = db.Owner.Where(ow => ow.Name == name).First();
            return o.OwnerID;
        }

        private int GetType(string type)
        {
            var db = new ItemContext();
            IList found = db.Types.Where(t => t.VType == type).ToList();
            if (found.Count == 0)
            {
                db.Types.Add(new VehicleType(type));
                db.SaveChanges();
            }

            VehicleType vt = db.Types.Where(ty => ty.VType == type).First();
            return vt.VTID;
        }

        //public Vehicle( string owner , string type, Object now = null)
        //{
        //    if (owner == null || type == null)
        //        throw new ArgumentNullException();
        //    LastId += 1;
        //    OwnerID = GetName(owner);
        //    VTID = GetType(type);
        //    Id = LastId;
        //    Color = ColorType.none;
        //    RegNr = null;
        //    Wheels = -1;
        //    checkInDate = now == null ? DateTime.Now : (DateTime)now;
        //}

        public Vehicle( string regNr, string Name, Object now, int Wheels, string Brand, string Color, DateTime checkinTime)
        {
            this.RegNr = regNr;
            this.Owner = Name;
            this.Wheels = Wheels;
            this.Brand = Brand;
            this.Color = Color;
            this.checkInDate = checkinTime;
        }

        public Vehicle(string owner)
        {
            this.Owner = owner;
        }
    }
}