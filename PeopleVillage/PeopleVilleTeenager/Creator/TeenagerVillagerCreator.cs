using PeopleVilleEngine.Locations;
using PeopleVilleEngine;
using PeopleVilleEngine.Villagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleVilleEngine.Villagers.Creators;

namespace PeopleVilleTeenager.Create
{
    public class TeenagerVillagerCreator : IVillagerCreator
    {

        public bool CreateTeenagerVillager(Village village)
        {


            if (village.Villagers.Count(v => v.IsTeenager()) > village.Villagers.Count* 0.3)
                return false; //No more the 30% can be Teenager

            var random = RNG.GetInstance();
            if (random.Next(1, 11) !=8)
                 return false; //1 of 10 chance to create a pension

            //Teenager start 12
            var Teenager = new TeenagerVillager(village, random.Next(12, 19));
            ////Add to village
            village.Villagers.Add(Teenager);
            return true;
        }
 

        public bool CreateVillager(Village village)
        {
            throw new NotImplementedException();
        }
    }
}
