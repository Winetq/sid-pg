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
}
