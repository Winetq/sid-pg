namespace SmartPartyApi.Models;

public class DatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string TemperatureCollectionName { get; set; } = null!;

    public string PeopleCounterCollectionName { get; set; } = null!;

    public string NoiseCollectionName { get; set; } = null!;

    public string MusicVolumeCollectionName { get; set; } = null!;
}
