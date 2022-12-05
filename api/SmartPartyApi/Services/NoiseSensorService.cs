using SmartPartyApi.Models;
using SmartPartyApi.Repositories;

namespace SmartPartyApi.Services;

public class NoiseSensorService {

    private readonly NoiseSensorRepository _noiseRepository;

    public NoiseSensorService(NoiseSensorRepository noiseRepository)
    {
        this._noiseRepository = noiseRepository;
    }

    public async Task AddNoiseRecord(Message noiseMessage)
    {
        var noise = new NoiseRecord
        {
            Value = noiseMessage.MeasureValue,
            MeasuredAt = DateTime.Now,
            SensorId = noiseMessage.SensorId
        };
        await _noiseRepository.CreateAsync(noise);
    }

    public async Task<List<NoiseRecord>> GetAllNoiseRecords()
    {
        return await _noiseRepository.GetAsync();
    }
}