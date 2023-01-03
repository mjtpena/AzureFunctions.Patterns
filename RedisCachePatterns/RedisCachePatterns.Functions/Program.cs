using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        // Azure Redis Cache
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = Environment.GetEnvironmentVariable("RedisCache");
            options.InstanceName = "MyRedisCacheInstance";
        });
    })
    .Build();

host.Run();