using PeopleVilleEngine.Locations;
using PeopleVilleEngine;
using PeopleVilleEngine.Villagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleVilleEngine.Villagers.Creators;

namespace PeopleVilleVillagerPensionist.Creator;
public class PensionistVillageCreator : IVillagerCreator
{
    public bool CreateVillager(Village village)
    {
        var home = FindHome(village);
        var random = RNG.GetInstance();
        if (random.Next(1, 10) > 3)
            return false; //1 of 10 chance to create a pension

        //Pensionist start 66
        var Pensionist = new PensionistVillager(village, random.Next(66, 100));
        ////Add to village
        Pensionist.Home = home;
        village.Villagers.Add(Pensionist);
        home.Villagers().Add(Pensionist);
        return true;
    }

    private IHouse? FindHome(Village village)
    {
        var random = RNG.GetInstance();

        var potentialHomes = village.Locations.Where(p => p.GetType().IsAssignableTo(typeof(IHouse)))
           .Where(p => p.Villagers().Count(v => v.GetType() == typeof(AdultVillager)) >= 2)
           .Where(p => ((IHouse)p).Population < ((IHouse)p).MaxPopulation && (((IHouse)p).houseType == HouseType.House || ((IHouse)p).houseType == HouseType.NursingHome)).ToList();


        if (random.Next(1, 10) < 9 && potentialHomes.Count > 0)
        {
            return (IHouse)potentialHomes[random.Next(0, potentialHomes.Count)];
        }
        else
        {
            IHouse newHouse = null;
            if (random.Next(1, 10) < 8)
            {
                newHouse = new House();
            }
            else
            {
                newHouse = new NursingHome();
            }
            village.Locations.Add(newHouse);
            return newHouse;
        }
    }
}
public class PensionistVillager : BaseVillager
{
    public PensionistVillager(Village village) : base(village)
    {
        //random age pesion
        Age = RNG.GetInstance().Next(66, 100);


    }
    public PensionistVillager(Village village, int age) : base(village)
    {
        Age = age;
    }
}
