namespace PeopleVilleEngine.Villagers;
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
    public TeenagerVillager(Village village, int age, bool isMale) :this(village, age)
    {
        IsMale=isMale;
    }
}
