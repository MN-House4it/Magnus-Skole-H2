﻿using PeopleVilleEngine.Locations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeopleVilleEngine.Villagers.Creators
{
    public class VillagerCreatorPensionist : IVillagerCreator
    {
        public bool CreateVillager(Village village)
        {
            var home = FindHome(village);
            if (home == null) return false;

            var random = RNG.GetInstance();
            var pension = new PensionistVillager(village);
           // var child = new ChildVillager(village);

            //add pension pto adulthome
            var first = home.Villagers().First(v => v.GetType() == typeof(AdultVillager));
            pension.LastName = first.LastName;
            
            //add peånpsion to  home
            home.Villagers().Add(pension);
            pension.Home = home;
            village.Villagers.Add(pension);
            return true;
        }

        private IHouse? FindHome(Village village)
        {
            var random = RNG.GetInstance();

            var potentialHomes = village.Locations.Where(p => p.GetType().IsAssignableTo(typeof(IHouse)))
                // atlease one adult
               .Where(p => p.Villagers().Count(v => v.GetType() == typeof(AdultVillager)) >= 1)
               .Where(p => ((IHouse)p).Population < ((IHouse)p).MaxPopulation).ToList();

            if (potentialHomes.Count == 0)
                return null;

            return (IHouse)potentialHomes[random.Next(0, potentialHomes.Count)];
        }







        //public bool CreateVillager(Village village)
        //{
        //    throw new NotImplementedException();
        //}
    }
}