namespace PeopleVilleEngine.Locations;
public class NursingHome : IHouse
{
    public NursingHome()
    {
        var random = RNG.GetInstance();
        MaxPopulation = random.Next(50, 200);
        houseType = HouseType.NursingHome;
    }
    private readonly List<BaseVillager> _villagers = new();
    public string Name => $"Nursing home, with a population of {Population}.";

    public List<BaseVillager> Villagers()
    {
        return _villagers;
    }

    public int Population => _villagers.Count;
    public int MaxPopulation { get; set; }
    public HouseType houseType { get; set; }
}