using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace backend;

public class RegisterVisitor
{
    private readonly ILogger<RegisterVisitor> _logger;

    public RegisterVisitor(ILogger<RegisterVisitor> logger)
    {
        _logger = logger;
    }

    [Function("RegisterVisitor")]
    public IActionResult Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req
    )
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        return new OkObjectResult("You are now registered");
    }
}
