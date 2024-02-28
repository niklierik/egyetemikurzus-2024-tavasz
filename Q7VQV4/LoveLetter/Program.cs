using LoveLetter.Bootstrap;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder hostApplicationBuilder = Host.CreateApplicationBuilder(args);

hostApplicationBuilder.Services.AddHostedService<AppLifecycleHandler>();

using IHost host = hostApplicationBuilder.Build();
host.Run();
