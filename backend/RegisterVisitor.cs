using System.Net;
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

        if (string.IsNullOrWhiteSpace(body))
        {
            var badResponse = req.CreateResponse(HttpStatusCode.BadRequest);
            await badResponse.WriteStringAsync("Missing name in request body.");
            return badResponse;
        }

        var visitor = await _visitorService.AddVisitorAsync(body.Trim());

        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteStringAsync($"Visitor registered: {visitor.Name}");
        return response;
    }
}
