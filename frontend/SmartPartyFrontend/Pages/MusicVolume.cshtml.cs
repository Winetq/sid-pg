using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPartyFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

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

    public async Task<IActionResult> OnPostDownloadJson() 
    {
        var response = _client.GetAsync("http://api/api/1/musicVolumeSensor").Result;
        var musicVolumeRecords = response.Content.ReadFromJsonAsync<List<MusicVolumeRecordModel>>().Result;
        if (!string.IsNullOrEmpty(Request.Form["startDateTime"]) && !string.IsNullOrEmpty(Request.Form["endDateTime"])) 
        {
            var startDateTime = Convert.ToDateTime(Request.Form["startDateTime"]);
            var endDateTime = Convert.ToDateTime(Request.Form["endDateTime"]);
            musicVolumeRecords = musicVolumeRecords.FindAll(record => record.MeasuredAt >= startDateTime && record.MeasuredAt <= endDateTime);
        }
        var jsonstr = System.Text.Json.JsonSerializer.Serialize(musicVolumeRecords);
        byte[] byteArray = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonstr);
        return File(byteArray, "application/force-download", "musicVolumeRecords.json");
    }

    public async Task<IActionResult> OnPostDownloadCsv() 
    {
        var response = _client.GetAsync("http://api/api/1/musicVolumeSensor").Result;
        var musicVolumeRecords = response.Content.ReadFromJsonAsync<List<MusicVolumeRecordModel>>().Result;
        if (!string.IsNullOrEmpty(Request.Form["startDateTime"]) && !string.IsNullOrEmpty(Request.Form["endDateTime"])) 
        {
            var startDateTime = Convert.ToDateTime(Request.Form["startDateTime"]);
            var endDateTime = Convert.ToDateTime(Request.Form["endDateTime"]);
            musicVolumeRecords = musicVolumeRecords.FindAll(record => record.MeasuredAt >= startDateTime && record.MeasuredAt <= endDateTime);
        }

        StringBuilder csv = new StringBuilder();

        string[] columnNames = new string[] { "Id", "MusicVolume", "MeasuredAt", "SensorId" };
        csv.AppendLine(string.Join(",", columnNames));

        foreach (MusicVolumeRecordModel record in musicVolumeRecords) 
        {
            csv.AppendLine(string.Join(",", new string[] { 
                record.Id,
                record.MusicVolume.ToString(),
                record.MeasuredAt.ToString(),
                record.SensorId }));
        }

        return File(Encoding.ASCII.GetBytes(csv.ToString()), "application/force-download", "musicVolumeRecords.csv");
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
