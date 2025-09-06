using System.Net;
using System.Text.Json;
using backend.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace backend;

public class RegisterVisitor
{
    private readonly VisitorService _visitorService;

    public RegisterVisitor(VisitorService visitorService)
    {
        _visitorService = visitorService;
    }

    [Function("RegisterVisitor")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req
    )
    {
        var body = await new StreamReader(req.Body).ReadToEndAsync();
        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(body);

        if (data == null || !data.ContainsKey("name"))
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Missing name in request body.");
            return badResponse;
        }

        var visitor = await _visitorService.AddVisitorAsync(data["name"]);

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(visitor);
        return response;
    }
}
