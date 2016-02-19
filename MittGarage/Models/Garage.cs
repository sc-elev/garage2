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

        private ItemContext db = new ItemContext();

        public string Id { get; private set; }

        public uint Capacity { get; protected set; }

        #region FreeSpace
        public uint FreeSpace
        {
            get { return Capacity - (uint)vehicles.Count; }
        }
        #endregion FreeSpace

        #region Enumerators
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
        #endregion Enumerators

        #region --Queries--

        #region Find Destinct

        public List<string> FindDestinct()
        {
           List<string> destinctlist = new List<string>();

            var SelectedOwner = vehicles.Select(x => x.Owner).OrderBy(x => x).ToList();
            foreach (var a in SelectedOwner)
            {
                destinctlist.Add(a);
            }
            return destinctlist;
        }

        #endregion Find Destinct

        #region Find Size
        public int FindSize()
        {
            return vehicles.Count;
            
        }
        #endregion Find Size

        #region Add Vehicle
        public void Add(Vehicle vehicle)
        {
            if (vehicles.Count >= Capacity)
                throw new InvalidOperationException();
            vehicles.Add(vehicle);
            db.Item.Add(vehicle);
            db.SaveChanges();
        }
        #endregion Add Vehicle

        #region Remove Vehicle
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
        #endregion

        #region Reg Query Summary
        /// <summary>
        /// Find vehicle with given registration number
        /// </summary>
        /// <returns>
        /// Found Vehicle or null if no match.
        /// </returns>
        /// <exception cref='InvalidOperationException'>
        /// Thrown if there are more than one vehicle with given number.
        /// </exception>
        #endregion

        #region Registration Query
        public Vehicle FindByRegNr(string regNr)
        {
            return vehicles
                        .Where(v => v.RegNr == regNr)
                        .SingleOrDefault();
        }
        #endregion

        #region Reg Query
        public Vehicle FindById(uint id)
        {
            return vehicles
                        .Where(v => v.Id == id)
                        .Single();
        }
        #endregion Reg Query

        #region CountByType Query
        public IDictionary<VehicleType, int> CountByType()
        {
            return vehicles
                        .GroupBy(v => v.Type)
                        .ToDictionary(g => g.Key, g => g.Count());
        }
        #endregion ´CountByType Query

        #region Search Query Summery
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
        #endregion Search Query Summary

        #region Specific Search Queries
        public Vehicle[] Search(string owner = null,
                                string regNr = null,
                                VehicleType type = VehicleType.none,
                                ColorType color = ColorType.none)
        {
            return vehicles
                      .Where(v => owner == null || v.Owner == owner)
                      .Where(v => regNr == null || v.RegNr == regNr)
                      .Where(v => type == VehicleType.none || v.Type == type)
                      .Where(v => color == ColorType.none || v.Color == color)
                      .OrderBy (v => v.checkInDate)
                      .ToArray();
        }

        public Vehicle[] Search(SearchCtx ctx)
        {
            return vehicles
                .Where(v => ctx.Type == VehicleType.none || ctx.Type == v.Type)
                .Where(v => !ctx.OnlyToday 
                            || DateTime.Now.Day == v.checkInDate.Day)
                .Where(v => ctx.Searchstring == v.RegNr 
                            || ctx.Searchstring == v.Owner)
                .ToArray();  
        }

        #endregion Specific Search Queries

        #endregion --Queries--

        #region --Save & Load Functions-- 

        public Garage(string id, uint capacity)
           
        {
            db = new ItemContext();
            vehicles = db.Item.ToList();
            this.Id = id;
            this.Capacity = capacity;
        }
        #endregion
        //public IEnumerator<Vehicle> GetEnumerator()
        //{
        //    foreach (T index in Vehicle)
        //    {
        //        yield return index;
        //    }
        //}

        //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
