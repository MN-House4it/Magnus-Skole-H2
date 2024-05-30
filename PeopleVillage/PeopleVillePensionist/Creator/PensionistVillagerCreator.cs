using PeopleVilleEngine.Locations;
using PeopleVilleEngine;
using PeopleVilleEngine.Villagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleVilleEngine.Villagers.Creators;

namespace PeopleVillePensionist.Create
{
    public class PensionistVillagerCreator : IVillagerCreator
    {
        public bool CreateVillager(Village village)
        {
            if (village.Villagers.Count(v => v.IsPensionist()) > village.Villagers.Count * 0.3)
                return false; //No more the 30% can be pensionist

            var random = RNG.GetInstance();
            if(random.Next(1, 11) != 7)
                return false; //1 of 10 chance to create a pension

            //Pensionist start 66
            var Pensionist = new AdultVillager(village, random.Next(66, 100));
            ////Add to village
            village.Villagers.Add(Pensionist);
            return true;
        }
    }
    //public class PensionistVillager : BaseVillager
    //{
    //    public PensionistVillager(Village village) : base(village)
    //    {
    //        //random age pesion
    //        Age = RNG.GetInstance().Next(66, 100);


    //    }
    //    public PensionistVillager(Village village, int age) : base(village)
    //    {
    //        Age = age;
    //    }
    //}
}
