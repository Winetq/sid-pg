using SmartPartyApi.Models;
using SmartPartyApi.Repositories;

namespace SmartPartyApi.Services;

public class MusicVolumeSensorService {

    private readonly MusicVolumeSensorRepository _musicVolumeRepository;

    public MusicVolumeSensorService(MusicVolumeSensorRepository musicVolumeRepository)
    {
        this._musicVolumeRepository = musicVolumeRepository;
    }

    public async Task AddMusicVolumeRecord(Message musicVolumeMessage)
    {
        var musicVolume = new MusicVolumeRecord
        {
            MusicVolume = musicVolumeMessage.MeasureValue,
            MeasuredAt = DateTime.Now,
            SensorId = musicVolumeMessage.SensorId
        };
        await _musicVolumeRepository.CreateAsync(musicVolume);
    }

    public async Task<List<MusicVolumeRecord>> GetAllMusicVolumeRecords()
    {
        return await _musicVolumeRepository.GetAsync();
    }
}
