using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace backend;

public class RegisterVisitor
{
    [Function("RegisterVisitor")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
        FunctionContext context
    )
    {
        var logger = context.GetLogger("RegisterVisitor");
        logger.LogInformation("function processed a request.");

        var connection = Environment.GetEnvironmentVariable("cosmosdb");

        if (string.IsNullOrWhiteSpace(connection))
        {
            var resp = req.CreateResponse(HttpStatusCode.InternalServerError);
            await resp.WriteStringAsync("CosmosDbConnection is not set!");
            return resp;
        }

        CosmosClient client = new CosmosClient(connection);
        Database db = client.GetDatabase("usersdb");
        Container container = db.GetContainer("users");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<string>(requestBody);
        if (data == null)
        {
            var resp = req.CreateResponse(HttpStatusCode.BadRequest);
            await resp.WriteStringAsync("Missing 'name' in request body.");
            return resp;
        }
        var visitor = new Visitor
        {
            Id = Guid.NewGuid().ToString(),
            Name = data,
            Timestamp = DateTime.UtcNow,
        };

        await container.CreateItemAsync(visitor, new PartitionKey(visitor.Id));

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync("You are now registered!");
        return response;
    }
}

public class Visitor
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public DateTime Timestamp { get; set; }
}
