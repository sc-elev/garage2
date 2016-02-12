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

        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(MittGarage.DataAccessLayer.ItemContext context)
        {
            var car =  new CarVehicle(createNummerplat());
            car.Type = VehicleType.oljetanker;
            context.Item.AddOrUpdate(r => r.Id, car);
        }

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
