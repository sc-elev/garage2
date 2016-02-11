using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


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

    public enum VehicleType
    {
        car, bike, airplane, oljetanker, bus, mc, none
    }

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


    /// <summary>
    /// The base of all items in the garage.
    /// </summary>
    [Serializable()]
    abstract public class Vehicle
    {
        protected static int LastId = 0;

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

      

        // Mandatory attributes without defaults.
        public string Owner { get; private set; }

        public string Id { get; private set; }

        public VehicleType Type { get; set; }

        public DateTime checkInDate { get; protected set; }

        // Searchable, optional attributes.
        public ColorType Color { get; protected set; }

        public int Wheels { get; protected set; }

        public string RegNr { get; set; }

        public string Brand { get; protected set; }

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
            if (attr.Count > 2)
            {
                Color = ParseColor(attr[2]);
            }
            return this;
        }

        public Vehicle(string owner, VehicleType type, Object now = null)
        {
            if (owner == null)
                throw new ArgumentNullException();
            LastId += 1;
            Owner = owner;
            this.Type = type;
            Id = LastId.ToString();
            Color = ColorType.none;
            RegNr = null;
            Wheels = -1;
            checkInDate = now == null ? DateTime.Now : (DateTime)now;
        }
    }

    #region Serializable Bus
    [Serializable()]
    class BusVehicle : Vehicle
    {

        public override string ToString()
        {
            return String.Format(
                "Bus, RegNr: {0}, Owner:{1}, id: {2}, wheels:  {3} ",
                RegNr, Owner, Id, Wheels);
        }

        public BusVehicle(string owner) : base(owner, VehicleType.bus) { }

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
                RegNr, Owner, Id, Wheels);
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
            if (attr.Count > 3)
            {
                Color = ParseColor(attr[3]);
            }
            return this;
        }

        public PlaneVehicle(string owner) : base(owner, VehicleType.airplane) { }
    }
    #endregion

    #region Seralizable MC
    [Serializable()]
    class McVehicle : Vehicle
    {

        public override string ToString()
        {
            return String.Format(
                "Motorcycle, RegNr: {0}, Owner:{1}, id: {2}", RegNr, Owner, Id);
        }

        public McVehicle(string owner) : base(owner, VehicleType.mc) { }

    }


    [Serializable()]
    class BoatVehicle : Vehicle
    {

        public override string ToString()
        {
            return String.Format(
                "Boat, brand: {0}, Owner:{1}, id: {2}", Brand, Owner, Id);
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

        public BoatVehicle(string owner) : base(owner, VehicleType.oljetanker) { }

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
                RegNr, Owner, Id, Color);
        }

        public CarVehicle(string owner) : base(owner, VehicleType.car) { }
    }
    #endregion

}