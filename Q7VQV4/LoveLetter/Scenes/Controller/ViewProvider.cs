using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LoveLetter.Scenes.Abstract;
using LoveLetter.Scenes.MainMenu;

using Microsoft.Extensions.DependencyInjection;

namespace LoveLetter.Scenes.Controller;

public class ViewProvider(IServiceProvider serviceProvider) : IViewProvider
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private T GetView<T>() where T : IView
    {
        T? view = _serviceProvider.GetService<T>();
        if (view is null)
        {
            throw new InvalidOperationException($"{typeof(T).Name} cannot be provided.");
        }
        return view;
    }

    public IMainMenuView GetMainMenuView()
    {
        return GetView<IMainMenuView>();
    }
}
