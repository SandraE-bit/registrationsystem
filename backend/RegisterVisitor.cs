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
        logger.LogInformation($"COSMOS_CONN = {(connection ?? "NULL")}");

        if (string.IsNullOrWhiteSpace(connection))
        {
            var errorResp = req.CreateResponse(HttpStatusCode.InternalServerError);
            await errorResp.WriteStringAsync("Cosmos connection string is missing!");
            return errorResp;
        }

        var client = new CosmosClient(connection);
        var db = client.GetDatabase("usersdb");
        var container = db.GetContainer("users");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<string>(requestBody);

        if (string.IsNullOrWhiteSpace(data))
        {
            var errorResp = req.CreateResponse(HttpStatusCode.BadRequest);
            await errorResp.WriteStringAsync("Missing 'name' in request body.");
            return errorResp;
        }

        var visitor = new Visitor
        {
            Id = Guid.NewGuid().ToString(),
            Name = data,
            Timestamp = DateTime.UtcNow
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
