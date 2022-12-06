using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartPartyFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace SmartPartyFrontend.Pages;

public class PeopleCounterModel : PageModel
{
    private readonly HttpClient _client = new();
    private readonly ILogger<PeopleCounterModel> _logger;

    public PeopleCounterModel(ILogger<PeopleCounterModel> logger)
    {
        _logger = logger;
        Measurements = new List<PeopleCounterRecord>();
    }

    public List<PeopleCounterRecord> Measurements { get; set; }

    public void OnGet()
    {
        var response = _client.GetAsync("http://SI_175132_api/api/1/peopleCounter").Result;
        var body = response.Content.ReadFromJsonAsync<List<PeopleCounterRecord>>().Result;
        if (body != null) Measurements = body;
    }

    public async Task<IActionResult> OnPostDownloadJson() 
    {
        var response = _client.GetAsync("http://SI_175132_api/api/1/peopleCounter").Result;
        var peopleCounterRecords = response.Content.ReadFromJsonAsync<List<PeopleCounterRecord>>().Result;
        if (!string.IsNullOrEmpty(Request.Form["startDateTime"]) && !string.IsNullOrEmpty(Request.Form["endDateTime"])) 
        {
            var startDateTime = Convert.ToDateTime(Request.Form["startDateTime"]);
            var endDateTime = Convert.ToDateTime(Request.Form["endDateTime"]);
            peopleCounterRecords = peopleCounterRecords.FindAll(record => record.MeasuredAt >= startDateTime && record.MeasuredAt <= endDateTime);
        }
        var jsonstr = System.Text.Json.JsonSerializer.Serialize(peopleCounterRecords);
        byte[] byteArray = System.Text.ASCIIEncoding.ASCII.GetBytes(jsonstr);
        return File(byteArray, "application/force-download", "peopleCounterRecords.json");
    }

    public async Task<IActionResult> OnPostDownloadCsv() 
    {
        var response = _client.GetAsync("http://SI_175132_api/api/1/peopleCounter").Result;
        var peopleCounterRecords = response.Content.ReadFromJsonAsync<List<PeopleCounterRecord>>().Result;
        if (!string.IsNullOrEmpty(Request.Form["startDateTime"]) && !string.IsNullOrEmpty(Request.Form["endDateTime"])) 
        {
            var startDateTime = Convert.ToDateTime(Request.Form["startDateTime"]);
            var endDateTime = Convert.ToDateTime(Request.Form["endDateTime"]);
            peopleCounterRecords = peopleCounterRecords.FindAll(record => record.MeasuredAt >= startDateTime && record.MeasuredAt <= endDateTime);
        }

        StringBuilder csv = new StringBuilder();

        string[] columnNames = new string[] { "Id", "NumberOfPeople", "MeasuredAt", "SensorId" };
        csv.AppendLine(string.Join(",", columnNames));

        foreach (PeopleCounterRecord record in peopleCounterRecords) 
        {
            csv.AppendLine(string.Join(",", new string[] { 
                record.Id,
                record.NumberOfPeople.ToString(),
                record.MeasuredAt.ToString(),
                record.SensorId }));
        }

        return File(Encoding.ASCII.GetBytes(csv.ToString()), "application/force-download", "peopleCounterRecords.csv");
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
                YValues = t.ToList().Select(m => m.NumberOfPeople).ToList()
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