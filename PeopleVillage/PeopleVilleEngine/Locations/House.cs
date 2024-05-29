namespace PeopleVilleEngine.Locations;
public class House : IHouse
{
    public House()
    {
        var random = RNG.GetInstance();
        MaxPopulation = random.Next(1, 6);
    }
    private readonly List<BaseVillager> _villagers = new();
    public string Name => $"House, with a population of {Population}.";

    public List<BaseVillager> Villagers()
    {
        return _villagers;
    }

    public int Population => _villagers.Count;
    public int MaxPopulation { get; set; }
}