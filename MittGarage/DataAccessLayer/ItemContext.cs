using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MittGarage.DataAccessLayer
{
    public class ItemContext : DbContext
    {
        public ItemContext() : base("DefaultConnection") { }
        public DbSet<Models.Vehicle> Item { get; set; }
    }
}