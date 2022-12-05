namespace SmartPartyApi.Services.SensorListeners;

public class NoiseSensorListener : GenericSensorListener
{
    private readonly NoiseSensorService _noiseSensorService;

    public NoiseSensorListener(ILogger<NoiseSensorListener> logger,
                                        IConfiguration config,
                                        NoiseSensorService noiseSensorService)
        : base(logger, config)
    {
        this._noiseSensorService = noiseSensorService;
    }

    protected override Task HandleMessage(Message message)
    {
        return _noiseSensorService.AddNoiseRecord(message);
    }

    protected override string GetQueueNameProperty()
    {
        return "QueueNameNoise";
    }
}
