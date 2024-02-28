using System.Diagnostics;

using LoveLetter.Scenes.Controller;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace LoveLetter.Bootstrap;

public class AppLifecycleHandler : IHostedService
{
    private readonly IHostApplicationLifetime _appLifeTime;
    private readonly ISceneController _sceneController;

    public AppLifecycleHandler(
        IHostApplicationLifetime appLifeTime,
        ISceneController sceneController
    )
    {
        this._appLifeTime = appLifeTime;
        this._sceneController = sceneController;
        this._appLifeTime.ApplicationStarted.Register(OnStart);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        OnStart();
        return Task.Run(() =>
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (true)
            {
                if (_sceneController.IsInitialized)
                {
                    ConsoleKeyInfo key = Console.ReadKey();
                    stopwatch.Stop();
                    _sceneController.ActiveView.OnKeyPressed(
                        key,
                        stopwatch.ElapsedMilliseconds / 1000.0f
                    );
                    stopwatch.Restart();
                }
            }
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private void OnStart()
    {
        Console.WriteLine("Loading main menu...");
        if (Console.LargestWindowWidth < 80)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(
                "Please set the console's width so it can fit at least 80 character."
            );
            return;
        }
        this._sceneController.OpenMainMenu();
    }
}
