using Microsoft.AspNetCore.Mvc;
using SmartPartyApi.Models;
using SmartPartyApi.Services;

namespace SmartPartyApi.Controllers;

[ApiController]
[Route("api/1/[controller]")]
public class MusicVolumeSensorController : ControllerBase {

    private readonly MusicVolumeSensorService _musicVolumeSensorService;
    private readonly ILogger<MusicVolumeSensorController> _logger;

    public MusicVolumeSensorController(ILogger<MusicVolumeSensorController> logger,
                                        MusicVolumeSensorService musicVolumeSensorService)
    {
        this._logger = logger;
        this._musicVolumeSensorService = musicVolumeSensorService;
    }

    [HttpGet]
    public async Task<List<MusicVolumeRecord>> Get() =>
        await _musicVolumeSensorService.GetAllMusicVolumeRecords();
}
