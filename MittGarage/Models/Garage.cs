using System;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using MittGarage.DataAccessLayer;

namespace MittGarage.Models
{
    [Serializable()]
    public class Garage : IEnumerable<Vehicle>
    {
        protected List<Vehicle> vehicles;

        private GarageDbContext db = new GarageDbContext();

        public string Id { get; private set; }

        public uint Capacity { get; protected set; }

        public uint FreeSpace
        {
            get { return Capacity - (uint)vehicles.Count; }
        }

        public IEnumerator<Vehicle> GetEnumerator()
        {
            return vehicles.GetEnumerator();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }


        public IEnumerator<Vehicle> ListVehicles()
        {
            return GetEnumerator();
        }


        public void Add(Vehicle vehicle)
        {
            if (vehicles.Count >= Capacity)
                throw new InvalidOperationException();
            vehicles.Add(vehicle);
            db.Vehicles.Add(vehicle);
            db.SaveChanges();
        }


        public void Remove(Vehicle v)
        {
            Remove(v.Id);
        }


        public void Remove(int key)
        {
            int ix;
            try
            {
                ix = vehicles.FindIndex(v => v.Id == key);
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
            vehicles.RemoveAt(ix);
        }


        /// <summary>
        /// Find vehicle with given registration number
        /// </summary>
        /// <returns>
        /// Found Vehicle or null if no match.
        /// </returns>
        /// <exception cref='InvalidOperationException'>
        /// Thrown if there are more than one vehicle with given number.
        /// </exception>
        public Vehicle FindByRegNr(string regNr)
        {
            return vehicles
                        .Where(v => v.RegNr == regNr)
                        .SingleOrDefault();
        }


        public Vehicle FindById(uint id)
        {
            return vehicles
                        .Where(v => v.Id == id)
                        .Single();
        }


        public IDictionary<int?, int> CountByType()
        {
            return vehicles
                        .GroupBy(v => v.VTID)
                        .ToDictionary(g => g.Key, g => g.Count());
        }


        protected List<Vehicle> JoinVehicles()
        {
            var query = db.Vehicles
                .GroupJoin(db.Owners,
                           v => v.OwnerID,
                           o => o.OwnerID,
                           (v, o) => new { v, o })
                //Counts-as LEFT JOIN
                .SelectMany(v => v.o.DefaultIfEmpty(),
                            (v, o) => new { v.v, o })
                //Joins Vehicle & VehicleType on VTID
                .GroupJoin(db.Types,
                           vo => vo.v.VTID,
                           t => t.VTID,
                           (vo, t) => new { vo, t })
                //Counts-as LEFT JOIN
                .SelectMany(vot => vot.t.DefaultIfEmpty(),
                            (vot, t) => new
                            {
                                Id = vot.vo.v.Id,
                                RegNr = vot.vo.v.RegNr,
                                OwnerName = vot.vo.o.Name,
                                OwnerID = vot.vo.o.OwnerID,
                                Typename = t.VType,
                                VTID = t.VTID,
                                Wheels = vot.vo.v.Wheels,
                                Brand = vot.vo.v.Brand,
                                Color = vot.vo.v.Color,
                                checkInDate = vot.vo.v.checkInDate
                            })
                .ToList();


            List<Vehicle> list = new List<Vehicle>();
            foreach (var x in query)
            {
                list.Add(new Vehicle
                {
                    Id = x.Id,
                    RegNr = x.RegNr,
                    Typename = x.Typename,
                    VTID = x.VTID,
                    OwnerName = x.OwnerName,
                    OwnerID = x.OwnerID,
                    Wheels = x.Wheels,
                    Brand = x.Brand,
                    Color = x.Color,
                    checkInDate = x.checkInDate
                });
            }
            return list;
        }


        /// <summary>
        /// Search for vehicles matching given attributes, return possibly
        /// empty array of matches.
        /// </summary>
        /// <param name='owner'>
        /// Optional: owner to match, or null for don't care.
        /// </param>
        /// <param name='regNr'>
        /// Optional: registration nr to match, or null for don't care.
        /// </param>
        /// <param name='type'>
        /// Optional: vehicle type or null for don't care.
        /// </param>
        /// <param name='color'>
        /// Optional: color to match, or Color.None for any color.
        /// </param>
        public Vehicle[] Search(string owner = null,
                                string regNr = null,
                                string vehicleType = null,
                                ColorType color = ColorType.none)
        {
            var Found = JoinVehicles()
                .Where(a => owner == null || a.OwnerName == owner)
                .Where(a => regNr == null || a.RegNr == regNr)
                .Where(a => vehicleType == null
                            || a.Typename == vehicleType)
                .Where(v => color == ColorType.none || v.Color == color)
                .OrderBy(v => v.checkInDate);
            return Found.ToArray();
        }


        public Vehicle[] Search(SearchCtx ctx)
        {
            //Joins Vehicle & Owner on OwnerID
            var found = JoinVehicles()
                .Where(a => string.IsNullOrEmpty(ctx.Typestring)
                            || a.Typename == ctx.Typestring)
                .Where(a => !ctx.OnlyToday
                            || DateTime.Now.Day == a.checkInDate.Day)
                .Where(a => string.IsNullOrEmpty(ctx.Searchstring)
                            || ctx.Searchstring == a.RegNr
                            || ctx.Searchstring == a.OwnerName);
            return found.ToArray();
        }


        public Garage(string id, uint capacity)
        {
            db = new GarageDbContext();
            vehicles = db.Vehicles.ToList();
            this.Id = id;
            this.Capacity = capacity;
        }
    }
}
