namespace SmartPartyFrontend.Models;

public class NoiseRecordModel
{
    public string? Id { get; set; }
    public decimal Value { get; set; }
    public DateTime MeasuredAt { get; set; }
    public string? SensorId { get; set; }
}