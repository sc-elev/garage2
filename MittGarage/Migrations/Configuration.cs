namespace MittGarage.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MittGarage.DataAccessLayer.ItemContext>
    {
        static Random _rnd = new Random();

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MittGarage.DataAccessLayer.ItemContext context)
        {
            context.Item.AddOrUpdate(r => r.ItemID, new MittGarage.Models.GarageItem { regNR = createNummerplat(), ItemID = _rnd.Next(100, 999), Owner = "Anders Andersson", Vehicle = MittGarage.Models.enumType.oljetanker, Inchecked = DateTime.Now });

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
