using backend.Models;
using Microsoft.Azure.Cosmos;

namespace backend.Services;

public class VisitorService
{
    private readonly Container _container;

    public VisitorService(CosmosClient client)
    {
        var database = client.GetDatabase("usersdb");
        _container = database.GetContainer("users");
    }

    public async Task<Visitor> AddVisitorAsync(string name)
    {
        var visitor = new Visitor { Id = Guid.NewGuid().ToString(), Name = name };

        await _container.CreateItemAsync(visitor, new PartitionKey(visitor.Id));

        return visitor;
    }
}

public class Visitor
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
}
