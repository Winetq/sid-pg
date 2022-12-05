using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPartyFrontend.Models;

namespace SmartPartyFrontend.Pages;

public class NoisesModel : PageModel
{
    private readonly HttpClient _client = new();
    private readonly ILogger<NoisesModel> _logger;

    public NoisesModel(ILogger<NoisesModel> logger)
    {
        _logger = logger;
        Measurements = new List<NoiseRecordModel>();
    }

    public List<NoiseRecordModel> Measurements { get; set; }

    public void OnGet()
    {
        var response = _client.GetAsync("http://api/api/1/noiseSensor").Result;
        var body = response.Content.ReadFromJsonAsync<List<NoiseRecordModel>>().Result;
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
                YValues = t.ToList().Select(m => m.Value).ToList()
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
