namespace SmartPartyApi.Services.SensorListeners;

public class MusicVolumeSensorListener : GenericSensorListener
{
    private readonly MusicVolumeSensorService _musicVolumeSensorService;

    public MusicVolumeSensorListener(ILogger<MusicVolumeSensorListener> logger,
                                        IConfiguration config,
                                        MusicVolumeSensorService musicVolumeSensorService)
        : base(logger, config)
    {
        this._musicVolumeSensorService = musicVolumeSensorService;
    }

    protected override Task HandleMessage(Message message)
    {
        return _musicVolumeSensorService.AddMusicVolumeRecord(message);
    }

    protected override string GetQueueNameProperty()
    {
        return "QueueNameMusicVolume";
    }
}
