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
        _logger.LogInformation("RegisterVisitor function called.");

        var body = await new StreamReader(req.Body).ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(body))
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Send a name in request body.");
            return badResponse;
        }

        Dictionary<string, string>? data;
        try
        {
            data = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
        }
        catch
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Error JSON.");
            return badResponse;
        }

        if (!data.TryGetValue("name", out var name) || string.IsNullOrWhiteSpace(name))
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Name missing.");
            return badResponse;
        }

        var visitor = await _visitorService.AddVisitorAsync(name);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync($"You are registered! id={visitor.Id}");
        return response;
    }
}
