using backend.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(
        (context, services) =>
        {
            var cosmosConnection = context.Configuration["COSMOS_CONN"];

            var client = new CosmosClientBuilder(cosmosConnection)
                .WithCustomSerializer(new NewtonsoftCosmosSerializer())
                .Build();

            services.AddSingleton(client);
            services.AddSingleton<VisitorService>();
        }
    )
    .Build();

host.Run();
