using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(
        (context, services) =>
        {
            var cosmosClient = new CosmosClient(Environment.GetEnvironmentVariable("COSMOS_CONN"));
            services.AddSingleton(cosmosClient);
        }
    )
    .Build();

host.Run();
