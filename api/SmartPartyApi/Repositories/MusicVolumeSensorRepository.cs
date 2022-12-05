namespace SmartPartyApi.Repositories;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Models;

public class MusicVolumeSensorRepository {
    private readonly IMongoCollection<MusicVolumeRecord> _musicVolumeCollection;

    public MusicVolumeSensorRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _musicVolumeCollection = mongoDb.GetCollection<MusicVolumeRecord>(databaseSettings.Value.MusicVolumeCollectionName);
    }

    public async Task<List<MusicVolumeRecord>> GetAsync() =>
        await _musicVolumeCollection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(MusicVolumeRecord record) =>
        await _musicVolumeCollection.InsertOneAsync(record);
}
