using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MittGarage.Models;

namespace MittGarage.DataAccessLayer
{
    public class GarageDbContext : DbContext
    {
        public GarageDbContext() : base("DefaultConnection") { }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<VehicleType> Types { get; set; }
    }
}