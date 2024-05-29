namespace PeopleVilleEngine.Locations;
public class Apartment : IHouse
{
    public Apartment()
    {
        var random = RNG.GetInstance();
        MaxPopulation = random.Next(1, 3);
    }
    private readonly List<BaseVillager> _villagers = new();
    public string Name => $"Apartment, with a population of {Population}.";

    public List<BaseVillager> Villagers()
    {
        return _villagers;
    }

    public int Population => _villagers.Count;
    public int MaxPopulation { get; set; }
}