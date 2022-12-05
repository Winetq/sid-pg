namespace SmartPartyApi.Repositories;

using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Models;

public class NoiseSensorRepository {
    private readonly IMongoCollection<NoiseRecord> _noiseCollection;

    public NoiseSensorRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDb = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _noiseCollection = mongoDb.GetCollection<NoiseRecord>(databaseSettings.Value.NoiseCollectionName);
    }

    public async Task<List<NoiseRecord>> GetAsync() =>
        await _noiseCollection.Find(_ => true).ToListAsync();

    public async Task CreateAsync(NoiseRecord record) =>
        await _noiseCollection.InsertOneAsync(record);
}
