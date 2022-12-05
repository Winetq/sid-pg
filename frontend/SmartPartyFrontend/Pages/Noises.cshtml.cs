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
}
