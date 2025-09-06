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

        var connection = Environment.GetEnvironmentVariable("COSMOS_CONN");
        if (string.IsNullOrWhiteSpace(connection))
        {
            var errorResp = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResp.WriteStringAsync("Cosmos connection string is missing!");
            return errorResp;
        }

        string body = await new StreamReader(req.Body).ReadToEndAsync();
        logger.LogInformation($"Received body: {body}");

        var payload = JsonConvert.DeserializeObject<VisitorPayload>(body);

        if (payload == null || string.IsNullOrWhiteSpace(payload.Name))
        {
            var badResp = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResp.WriteStringAsync("Missing 'name' in request body.");
            logger.LogWarning("Bad request: missing 'name'");
            return badResp;
        }

        logger.LogInformation($"Registering visitor: {payload.Name}");

        var client = new CosmosClient(connection);
        var container = client.GetContainer("usersdb", "users");

        var visitor = new Visitor
        {
            Id = Guid.NewGuid().ToString(),
            Name = payload.Name,
            Timestamp = DateTime.UtcNow,
        };

        await container.CreateItemAsync(visitor, new PartitionKey(visitor.Id));
        logger.LogInformation($"Visitor registered: {visitor.Name} ({visitor.Id})");

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync($"Registered visitor: {visitor.Name}");
        return response;
    }
}

public class VisitorPayload
{
    [JsonProperty("name")]
    public string Name { get; set; } = "";
}

public class Visitor
{
    [JsonProperty("id")]
    public string Id { get; set; } = "";

    [JsonProperty("name")]
    public string Name { get; set; } = "";

    [JsonProperty("timestamp")]
    public DateTime Timestamp { get; set; }
}
