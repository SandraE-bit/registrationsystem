using System.Net;
using System.Text.Json;
using backend.Models;
using backend.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace backend;

public class RegisterVisitor
{
    private readonly VisitorService _visitorService;
    private readonly ILogger _logger;

    public RegisterVisitor(VisitorService visitorService, ILoggerFactory loggerFactory)
    {
        _visitorService = visitorService;
        _logger = loggerFactory.CreateLogger<RegisterVisitor>();
    }

    [Function("RegisterVisitor")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req
    )
    {
        _logger.LogInformation("RegisterVisitor function processed a request.");

        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var json = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

        if (
            json == null
            || !json.TryGetValue("name", out var name)
            || string.IsNullOrWhiteSpace(name)
        )
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Missing 'name' in request body.");
            return badResponse;
        }

        var visitor = await _visitorService.AddVisitorAsync(name);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync($"You are now registered! Id={visitor.Id}");
        return response;
    }
}
