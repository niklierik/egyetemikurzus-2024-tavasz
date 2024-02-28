using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoveLetter.Bootstrap;

public class AppLifecycleHandler : IHostedService
{
    private readonly IHostApplicationLifetime _appLifeTime;

    public AppLifecycleHandler(IHostApplicationLifetime appLifeTime)
    {
        this._appLifeTime = appLifeTime;
        this._appLifeTime.ApplicationStarted.Register(OnStart);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnStart()
    {
        Console.WriteLine("App has started.");
    }
}
