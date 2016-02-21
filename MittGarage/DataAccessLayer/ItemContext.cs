using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MittGarage.Models;

namespace MittGarage.DataAccessLayer
{
    public class ItemContext : DbContext
    {
        public ItemContext() : base("DefaultConnection") { }
        public DbSet<Vehicle> Item { get; set; }
        public DbSet<Owner> Owner { get; set; }
        public DbSet<
            VehicleType> Types { get; set; }
    }
}