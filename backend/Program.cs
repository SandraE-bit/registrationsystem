using backend.Models;
using backend.Services;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults() 
    .ConfigureAppConfiguration(config =>
    {
        config.AddEnvironmentVariables();
    })
    .ConfigureServices((context, services) =>
        {
            services.Configure<Settings.Configuration>(context.Configuration);

            var cosmosConn = context.Configuration["COSMOS_CONN"];
            services.AddSingleton(new CosmosClient(cosmosConn));

            services.AddSingleton<VisitorService>();
    })
    .Build();

host.Run();
