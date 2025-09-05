using backend.Models;
using backend.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
        {
            var cosmosConnection = context.Configuration["COSMOS_CONN"];

            services.AddSingleton(new CosmosClient(cosmosConnection));

            services.AddSingleton<VisitorService>();
        }
    )
    .Build();

host.Run();
