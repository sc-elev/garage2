namespace MittGarage.Migrations
{
    using MittGarage.Models;
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MittGarage.DataAccessLayer.ItemContext>
    {
        static Random _rnd = new Random();

        #region Migration Config
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
        #endregion Migration Config

        #region Seed
        protected override void Seed(MittGarage.DataAccessLayer.ItemContext context)
        {
            var car =  new CarVehicle("Teo");
            car.RegNr = createNummerplat();
            car.Type = VehicleType.car;
            context.Item.AddOrUpdate(r => r.Id, car);
            context.SaveChanges();

            car = new CarVehicle("Kalle");
            car.RegNr = createNummerplat();
            car.Type = VehicleType.car;
            context.Item.AddOrUpdate(r => r.Id, car);
            context.SaveChanges();

            var boat = new BoatVehicle("kapten Svartskägg");
            boat.RegNr = createNummerplat();
            boat.Type = VehicleType.oljetanker;
            context.Item.AddOrUpdate(r => r.Id, boat);
            context.SaveChanges();

            boat = new BoatVehicle("kapten Svartskägg");
            boat.RegNr = createNummerplat();
            boat.Type = VehicleType.airplane;
            context.Item.AddOrUpdate(r => r.Id, boat);
            context.SaveChanges();

            boat = new BoatVehicle("Röde Baron");
            boat.RegNr = createNummerplat();
            boat.Type = VehicleType.airplane;
            context.Item.AddOrUpdate(r => r.Id, boat);
            context.SaveChanges();

            boat = new BoatVehicle("knutten Knut");
            boat.RegNr = createNummerplat();
            boat.Type = VehicleType.mc;
            context.Item.AddOrUpdate(r => r.Id, boat);
            context.SaveChanges();

            boat = new BoatVehicle("Arne");
            boat.RegNr = createNummerplat();
            boat.Type = VehicleType.bus;
            context.Item.AddOrUpdate(r => r.Id, boat);
            context.SaveChanges();

            boat = new BoatVehicle("Prinsessan Madde");
            boat.RegNr = createNummerplat();
            boat.Type = VehicleType.oljetanker;
            context.Item.AddOrUpdate(r => r.Id, boat);
            context.SaveChanges();

            boat = new BoatVehicle("Major Major Major Major");
            boat.RegNr = createNummerplat();
            boat.Type = VehicleType.mc;
            context.Item.AddOrUpdate(r => r.Id, boat);
            context.SaveChanges();

            boat = new BoatVehicle("Bo Ek");
            boat.RegNr = createNummerplat();
            boat.Type = VehicleType.car;
            context.Item.AddOrUpdate(r => r.Id, boat);
            context.SaveChanges();
        }
        #endregion Seed

        #region CreateNummerPlat
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
        #endregion CreateNummerPlat

        #region GetChar
        public static char getChar(int position)
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return chars[position];
        }
        #endregion GetChar
    }
}
