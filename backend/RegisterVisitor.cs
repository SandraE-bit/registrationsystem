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
        Database db = client.GetDatabase("SampleDB");
        Container container = db.GetContainer("SampleContainer");

        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        dynamic data = JsonConvert.DeserializeObject<Visitor>(requestBody);
        string? name = data?.name;

        var registration = new Visitor
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            Timestamp = DateTime.UtcNow,
        };

        await container.CreateItemAsync(registration);

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
