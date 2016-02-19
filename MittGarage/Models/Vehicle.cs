using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections;
using MittGarage.DataAccessLayer;


namespace MittGarage.Models
{
    
    #region VehicleFactory Class
    public class VehicleFactory
    {
        public Vehicle Fabricate(string Type, List<string> cmdline)
        {
            if (cmdline.Count == 0) throw new ArgumentException("Missing owner");
            string owner = cmdline[0];
            cmdline.RemoveAt(0);
            Vehicle v;
            switch (Type)
            {
                case "car":
                    v = new CarVehicle(owner);
                    break;
                case "bus":
                    v = new BusVehicle(owner);
                    break;
                case "boat":
                    v = new BoatVehicle(owner);
                    break;
                case "mc":
                    v = new McVehicle(owner);
                    break;
                case "plane":
                    v = new PlaneVehicle(owner);
                    break;
                default:
                    throw new ArgumentException("Illegal vehicle type");
            }
            return v.Init(cmdline);
        }
    }
    #endregion VehicleFactory Class

    #region --Base of Items in Garage--
    /// <summary>
    /// The base of all items in the garage.
    /// </summary>
    [Serializable()]
    public class Vehicle
    {
        protected static int LastId = 0;

        #region --All Attributes--

        #region Mandatory Attributes without Defaults
        // Mandatory attributes without defaults.

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        [ForeignKey("OwnerID")]
        public int OwnerID { get; set; }
               
        [Required]
        [ForeignKey("VTID")]
        public int VTID {get; set;}

        public DateTime checkInDate { get; protected set; }
        #endregion Mandatory Attributes without Defaults

        #region Searchable Optional Attributes
        // Searchable, optional attributes.
        public string Color { get; protected set; }

        public int Wheels { get; protected set; }

        public string RegNr { get; set; }

        public string Brand { get; protected set; }
        #endregion Searchable Optional Attributes

        #endregion --All Attributes--

        #region Vehicle ID Compare
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
        #endregion Vehicle ID Compare

        #region Parse and Store Commandline Attributes
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
        #endregion Parse and Store Commandline Attributes

        #region Vehicle Constructor

        private int GetName (string name)
        {
            var db = new ItemContext();
            IList Found = db.Owner.Where(g => g.Name == name).ToList();
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

        public Vehicle( string regNr, string Name, Object now, int Wheels, string Brand. string Color) 
        {
            this.RegNr = regNr
        }


        #endregion Vehicle Constructor

    }
    #endregion Base of All Items in Garage

    #region --Serializeables--

    #region Serializable Bus
    [Serializable()]
    class BusVehicle : Vehicle
    {

        public override string ToString()
        {
            return String.Format(
                "Bus, RegNr: {0}, Owner:{1}, id: {2}, wheels:  {3} ",
                RegNr, OwnerID, Id, Wheels);
        }

        public BusVehicle(string owner) : base(owner, VehicleType.bus) { }

        public BusVehicle() : this("Unknown") { }

    }
    #endregion

    #region Serializable Plane
    [Serializable()]
    class PlaneVehicle : Vehicle
    {

        public override string ToString()
        {
            return String.Format(
                "Plane, RegNr: {0}, Owner:{1}, id: {2}, wheels: {3} ",
                RegNr, OwnerID, Id, Wheels);
        }

        public override Vehicle Init(List<string> attr)
        {
            if (attr.Count < 1)
            {
                throw new ArgumentException("Missing required regnr");
            }
            this.RegNr = attr[0];
            if (attr.Count > 1)
            {
                int w;
                if (!int.TryParse(attr[1], out w))
                {
                    throw new ArgumentException("Bad wheel number syntax");
                }
                Wheels = w;
            }
            //if (attr.Count > 2)
            //{
            //    Type = +attr[2];
            //}
            return this;
        }

        public PlaneVehicle(string ownerID) : base(ownerID, VehicleType.airplane) { }

        public PlaneVehicle(): this("Unknown") {}
    }
    #endregion

    #region Seralizable MC
    [Serializable()]
    class McVehicle : Vehicle
    {

        public override string ToString()
        {
            return String.Format(
                "Motorcycle, RegNr: {0}, Owner:{1}, id: {2}", RegNr, ownerID, Id);
        }

        public McVehicle(string ownerID) : base(ownerID, VehicleType.mc) { }

        public McVehicle() : this("Unknown") { }

    }


    [Serializable()]
    class BoatVehicle : Vehicle
    {

        public override string ToString()
        {
            return String.Format(
                "Boat, brand: {0}, Owner:{1}, id: {2}", Brand, ownerID, Id);
        }

        public override Vehicle Init(List<string> attr)
        {
            if (attr.Count == 0)
            {
                throw new ArgumentException("Missing required brand attribute");
            }
            this.Brand = attr[0];
            return this;
        }

        public BoatVehicle(string ownerID) : base(ownerID, VehicleType.oljetanker) { }

        public BoatVehicle() : this("Unknown") {}

    }
    #endregion

    #region Serializable Car
    [Serializable()]
    class CarVehicle : Vehicle
    {
        public override string ToString()
        {
            return String.Format(
                "Car, nr: {0}, Owner:{1}, id: {2}, color: {3}",
                RegNr, OwnerID, Id, Color);
        }

        public CarVehicle(string ownerID) : base(ownerID, VehicleType.car) { }

        public CarVehicle() : this ("Unknown") {}
    }
    #endregion

    #endregion --Serializeables--
}