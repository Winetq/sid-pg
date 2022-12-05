namespace SmartPartyFrontend.Models;

public class MusicVolumeRecordModel
{
    public string? Id { get; set; }
    public decimal MusicVolume { get; set; }
    public DateTime MeasuredAt { get; set; }
    public string? SensorId { get; set; }
}
