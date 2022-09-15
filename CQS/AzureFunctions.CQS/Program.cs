using Microsoft.Extensions.Hosting;
using MediatR;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>{
        services.AddMediatR(typeof(Program));
    })
    .Build();

host.Run();
