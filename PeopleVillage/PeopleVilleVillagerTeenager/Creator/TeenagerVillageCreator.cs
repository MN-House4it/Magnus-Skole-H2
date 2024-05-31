using PeopleVilleEngine.Locations;
using PeopleVilleEngine;
using PeopleVilleEngine.Villagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PeopleVilleEngine.Villagers.Creators;

namespace PeopleVilleVillagerHomeless.Creator;
public class TeenagerVillageCreator : IVillagerCreator
{
    public bool CreateVillager(Village village)
    {
        //if (village.Villagers.Count(v => v.IsTeenager()) > village.Villagers.Count * 0.3)
        //    return false; //No more the 30% can be Teenager

        var home = FindHome(village);
        var random = RNG.GetInstance();
        //if (random.Next(1, 11) != 8)
        //    return false; //1 of 10 chance to create a Teenager

        //Teenager start 12
        var Teenager = new TeenagerVillager(village, random.Next(12, 19));

        if (home.Villagers().Count() > 0)
        {
            var first = home.Villagers().First(v => v.GetType() == typeof(AdultVillager));
            Teenager.LastName = first.LastName;
        }
        
        ////Add to village
        ///home.Villagers().Add(teenager);
        Teenager.Home = home;
        village.Villagers.Add(Teenager);
        home.Villagers().Add(Teenager);
        return true;
    }

    private IHouse? FindHome(Village village)
    {
        var random = RNG.GetInstance();

        var potentialHomes = village.Locations.Where(p => p.GetType().IsAssignableTo(typeof(IHouse)))
           .Where(p => p.Villagers().Count(v => v.GetType() == typeof(AdultVillager)) >= 2)
           .Where(p => ((IHouse)p).Population < ((IHouse)p).MaxPopulation && (((IHouse)p).houseType == HouseType.House || ((IHouse)p).houseType == HouseType.Apartment)).ToList();


        if (random.Next(1, 10) < 8 && potentialHomes.Count > 0)
        {
            return (IHouse)potentialHomes[random.Next(0, potentialHomes.Count)];
        }
        else
        {
            IHouse newHouse = null;
            if (random.Next(1, 10) < 6)
            {
                newHouse = new House();
            }
            else
            {
                newHouse = new Apartment();
            }
            village.Locations.Add(newHouse);
            return newHouse;
        }        
    }
}

public class TeenagerVillager : BaseVillager
{
    public TeenagerVillager(Village village) : base(village)
    {
        Age = RNG.GetInstance().Next(12, 19);
    }

    public TeenagerVillager(Village village, int age) : this(village)
    {
        Age = age;
    }
    public TeenagerVillager(Village village, int age, bool isMale) : this(village, age)
    {
        IsMale = isMale;
    }
}
