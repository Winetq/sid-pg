using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPartyFrontend.Models;

namespace SmartPartyFrontend.Pages;

public class MusicVolumeModel : PageModel
{
    private readonly HttpClient _client = new();
    private readonly ILogger<MusicVolumeModel> _logger;

    public MusicVolumeModel(ILogger<MusicVolumeModel> logger)
    {
        _logger = logger;
        Measurements = new List<MusicVolumeRecordModel>();
    }

    public List<MusicVolumeRecordModel> Measurements { get; set; }

    public void OnGet()
    {
        var response = _client.GetAsync("http://api/api/1/musicVolumeSensor").Result;
        var body = response.Content.ReadFromJsonAsync<List<MusicVolumeRecordModel>>().Result;
        if (body != null) Measurements = body;
    }

    public List<ChartDataset> GetValues()
    {
        var datasets = (from i in Measurements
            group i by i.SensorId
            into t
            select new ChartDataset
            {
                Label = t.Key,
                XLabels = t.ToList().Select(m => m.MeasuredAt).ToList(),
                YValues = t.ToList().Select(m => m.MusicVolume).ToList()
            }).ToList();
        return datasets;
    }

    public class ChartDataset
    {
        public List<DateTime>? XLabels { get; set; }
        public List<decimal>? YValues { get; set; }
        public string? Label { get; set; }
    }
}
