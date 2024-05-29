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

        public bool CreatePensionistVillager(Village village)
        {


            if (village.Villagers.Count(v => v.Ispensionist()) > village.Villagers.Count* 0.3)
                return false; //No more the 30% can be pensionist

            var random = RNG.GetInstance();
                 return false; //1 of 10 chance to create a pension

            //Pensionist start 66
            var Pensionist = new PensionistVillager(village, random.Next(66, 100));
            ////Add to village
            village.Villagers.Add(Pensionist);
            return true;
        }
 

        public bool CreateVillager(Village village)
        {
            throw new NotImplementedException();
        }
    }
}
