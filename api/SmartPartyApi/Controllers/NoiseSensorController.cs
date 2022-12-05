using Microsoft.AspNetCore.Mvc;
using SmartPartyApi.Models;
using SmartPartyApi.Services;

namespace SmartPartyApi.Controllers;

[ApiController]
[Route("api/1/[controller]")]
public class NoiseSensorController : ControllerBase {

    private readonly NoiseSensorService _noiseSensorService;
    private readonly ILogger<NoiseSensorController> _logger;

    public NoiseSensorController(ILogger<NoiseSensorController> logger,
                                    NoiseSensorService noiseSensorService)
    {
        this._logger = logger;
        this._noiseSensorService = noiseSensorService;
    }

    [HttpGet]
    public async Task<List<NoiseRecord>> Get() =>
        await _noiseSensorService.GetAllNoiseRecords();
}
