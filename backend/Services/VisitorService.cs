using backend.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;

namespace backend.Services;

public sealed class VisitorService
{
    private readonly CosmosClient _client;
    private readonly Settings.Configuration _config;

    public VisitorService(CosmosClient client, IOptions<Settings.Configuration> configOptions)
    {
        _client = client;
        _config = configOptions.Value;
    }

    public async Task<Visitor> AddVisitorAsync(string name)
    {
        var db = _client.GetDatabase(_config.CosmosDB.DatabaseName);
        var container = db.GetContainer(_config.CosmosDB.ContainerName);

        var visitor = new Visitor
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            Timestamp = DateTime.UtcNow
        };

        await container.CreateItemAsync(visitor, new PartitionKey(visitor.Id));
        return visitor;
    }
}
