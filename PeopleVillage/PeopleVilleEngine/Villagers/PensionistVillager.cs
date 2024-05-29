
namespace PeopleVilleEngine.Villagers;

    public class PensionistVillager:BaseVillager

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

