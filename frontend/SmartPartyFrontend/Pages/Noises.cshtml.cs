using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPartyFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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
        var response = _client.GetAsync("http://SI_175132_api/api/1/noiseSensor").Result;
        var body = response.Content.ReadFromJsonAsync<List<NoiseRecordModel>>().Result;
        if (body != null) Measurements = body;
    }

    public async Task<IActionResult> OnPostDownloadJson() 
    {
        var response = _client.GetAsync("http://SI_175132_api/api/1/noiseSensor").Result;
        var noiseRecords = response.Content.ReadFromJsonAsync<List<NoiseRecordModel>>().Result;
        if (!string.IsNullOrEmpty(Request.Form["startDateTime"]) && !string.IsNullOrEmpty(Request.Form["endDateTime"])) 
        {
            var startDateTime = Convert.ToDateTime(Request.Form["startDateTime"]);
            var endDateTime = Convert.ToDateTime(Request.Form["endDateTime"]);
            noiseRecords = noiseRecords.FindAll(record => record.MeasuredAt >= startDateTime && record.MeasuredAt <= endDateTime);
        }
        var jsonstr = System.Text.Json.JsonSerializer.Serialize(noiseRecords);
        byte[] byteArray = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonstr);
        return File(byteArray, "application/force-download", "noiseRecords.json");
    }

    public async Task<IActionResult> OnPostDownloadCsv() 
    {
        var response = _client.GetAsync("http://SI_175132_api/api/1/noiseSensor").Result;
        var noiseRecords = response.Content.ReadFromJsonAsync<List<NoiseRecordModel>>().Result;
        if (!string.IsNullOrEmpty(Request.Form["startDateTime"]) && !string.IsNullOrEmpty(Request.Form["endDateTime"])) 
        {
            var startDateTime = Convert.ToDateTime(Request.Form["startDateTime"]);
            var endDateTime = Convert.ToDateTime(Request.Form["endDateTime"]);
            noiseRecords = noiseRecords.FindAll(record => record.MeasuredAt >= startDateTime && record.MeasuredAt <= endDateTime);
        }

        StringBuilder csv = new StringBuilder();

        string[] columnNames = new string[] { "Id", "Value", "MeasuredAt", "SensorId" };
        csv.AppendLine(string.Join(",", columnNames));

        foreach (NoiseRecordModel record in noiseRecords) 
        {
            csv.AppendLine(string.Join(",", new string[] { 
                record.Id,
                record.Value.ToString(),
                record.MeasuredAt.ToString(),
                record.SensorId }));
        }

        return File(Encoding.ASCII.GetBytes(csv.ToString()), "application/force-download", "noiseRecords.csv");
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
