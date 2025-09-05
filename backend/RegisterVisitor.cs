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

        var connection = Environment.GetEnvironmentVariable("CosmosDbConnection");

        CosmosClient client = new CosmosClient(connection);
        Database db = client.GetDatabase("RegistrationDB");
        Container container = db.GetContainer("RegistrationContainer");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonConvert.DeserializeObject<Visitor>(requestBody);
        if (data?.Name == null)
{
            var resp = req.CreateResponse(HttpStatusCode.BadRequest);
            await resp.WriteStringAsync("Missing 'name' in request body.");
            return resp;
}
        var visitor = new Visitor
        {
            Id = Guid.NewGuid().ToString(),
            Name = data.Name,
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
