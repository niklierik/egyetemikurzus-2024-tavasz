using LoveLetter.Bootstrap;
using LoveLetter.Scenes.Controller;
using LoveLetter.Scenes.MainMenu;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

HostApplicationBuilder hostApplicationBuilder = Host.CreateApplicationBuilder(args);

hostApplicationBuilder.Services.AddHostedService<AppLifecycleHandler>();
hostApplicationBuilder.Services.AddSingleton<ISceneController, SceneController>();
hostApplicationBuilder.Services.AddSingleton<IViewProvider, ViewProvider>();

hostApplicationBuilder.Services.AddScoped<IMainMenuView, MainMenuView>();
hostApplicationBuilder.Services.AddScoped<IMainMenuController, MainMenuController>();

// Remove default logs
hostApplicationBuilder.Logging.ClearProviders();

using IHost host = hostApplicationBuilder.Build();
host.Run();
