namespace MittGarage.Migrations
{
    using MittGarage.DataAccessLayer;
    using MittGarage.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Linq;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<MittGarage.DataAccessLayer.ItemContext>
    {
        static Random _rnd = new Random();


        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


        private void SaveChanges(DbContext context)
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException(
                    "Entity Validation Failed - errors follow:\n" +
                    sb.ToString(), ex
                ); // Add the original exception as the innerException
            }
        }

        protected override void Seed(MittGarage.DataAccessLayer.ItemContext context)
        {
            using (var db = new ItemContext())
            {
                Owner owner;
                owner = new Owner { OwnerID = 0, Name = "Adam Trädgårdh"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 1, Name = "Orvar Persson"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 2, Name = "Per Orvarsson"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 3, Name = "Hugo Svensson"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 4, Name = "Sven Hugosson"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 5, Name = "Glada draken"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 6, Name = "Sura draken"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 7, Name = "Någon Okänd"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 8, Name = "Bror Ivarsson"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 9, Name = "Ivar Brorsson"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 10, Name = "Ivar Brorsson"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 11, Name = "Eva Trädgårdh"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);
                owner = new Owner { OwnerID = 12, Name = "Hans Brädgård"};
                db.Owner.AddOrUpdate(o => o.OwnerID, owner);

                VehicleType type;
                type = new VehicleType { VTID = 1, VType = "car"};
                db.Types.AddOrUpdate(t => t.VTID, type);
                type = new VehicleType { VTID = 2, VType = "motorcycle"};
                db.Types.AddOrUpdate(t => t.VTID, type);
                type = new VehicleType { VTID = 3, VType = "boat"};
                db.Types.AddOrUpdate(t => t.VTID, type);
                type = new VehicleType { VTID = 4, VType = "bicycle"};
                db.Types.AddOrUpdate(t => t.VTID, type);
                type = new VehicleType { VTID = 5, VType = "airplane"};
                db.Types.AddOrUpdate(t => t.VTID, type);

                var car = new Vehicle {
                    OwnerID = 1,
                    VTID = 1,
                    RegNr = Configuration.createNummerplat(),
                };

                db.Item.AddOrUpdate(r => r.Id, car);
                car = new Vehicle
                {
                    OwnerID = 2,
                    VTID = 1,
                    RegNr = Configuration.createNummerplat(),
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                car = new Vehicle
                {
                    OwnerID = 3,
                    VTID = 3,
                    Brand = "Shark-24"
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                car = new Vehicle
                {
                    OwnerID = 4,
                    VTID = 3
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                car = new Vehicle
                {
                    OwnerID = 5,
                    VTID = 3,
                    Brand = "Yamaha"
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                car = new Vehicle
                {
                    OwnerID = 5,
                    VTID = 2,
                    RegNr = Configuration.createNummerplat(),
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                car = new Vehicle
                {
                    OwnerID = 6,
                    VTID = 3,
                    Brand = "Tailwind-33"
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                car = new Vehicle
                {
                    OwnerID = 7,
                    VTID = 4,
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                car = new Vehicle
                {
                    OwnerID = 8,
                    VTID = 5,
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                car = new Vehicle
                {
                    OwnerID = 9,
                    VTID = 1,
                    RegNr = Configuration.createNummerplat(),
                    Brand = "Saab"
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                car = new Vehicle
                {
                    OwnerID = 10,
                    VTID = 1,
                    RegNr = Configuration.createNummerplat(),
                    Brand = "Volvo",
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                car = new Vehicle
                {
                    OwnerID = 11,
                    VTID = 1,
                    RegNr = Configuration.createNummerplat(),
                    Brand = "Subaru",
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                car = new Vehicle
                {
                    OwnerID = 12,
                    VTID = 1,
                    RegNr = Configuration.createNummerplat(),
                    Brand = "Datsun",
                };
                db.Item.AddOrUpdate(r => r.Id, car);

                SaveChanges(db);
            }
        }

        //Randomizes new numberplates
        static public string createNummerplat()
        {
            string reg_nr = "";
            for (int i = 0; i < 3; i++)
            {
                reg_nr = reg_nr + getChar(_rnd.Next(0, 20)).ToString();
            }
            int randomIn = _rnd.Next(100, 999);
            reg_nr = reg_nr + randomIn.ToString();
            return reg_nr;
        }


        public static char getChar(int position)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return chars[position];
        }
    }
}
