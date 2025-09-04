using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace backend;

public static class RegisterVisitor
{
    private static readonly CosmosClient _cosmosClient;
    private static readonly Container _container;
    private static string connectionString;

    static RegisterVisitor()
    {
        string connectionString = Environment.GetEnvironmentVariable("CosmosDbConnection");
        _cosmosClient = new CosmosClient(connectionString);
        _container = _cosmosClient.GetContainer("SampleDB", "SampleContainer");
    }

    [Function("RegisterVisitor")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req,
        ILogger log
    )
    {
        log.LogInformation("C# HTTP trigger function processed a request.");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject(requestBody);
        string? name = data?.name;

        var visitor = new { id = Guid.NewGuid().ToString() };

        await _container.CreateItemAsync(visitor);

        return new OkObjectResult("You are now registered");
    }
}
