using LoveLetter.Scenes.Abstract;
using LoveLetter.Scenes.MainMenu;

using Microsoft.Extensions.DependencyInjection;

namespace LoveLetter.Scenes.Controller;

public class SceneController(IViewProvider viewProvider) : ISceneController
{
    private readonly IViewProvider _viewProvider = viewProvider;
    private IView? _activeView = null;

    public IView ActiveView
    {
        get
        {
            if (_activeView is null)
            {
                throw new InvalidOperationException(
                    "The Active View was accessed before one was initialized."
                );
            }

            return _activeView;
        }
        private set
        {
            _activeView = value;
            _activeView.Render();
        }
    }

    public bool IsInitialized => _activeView is not null;

    public void OpenMainMenu()
    {
        ActiveView = _viewProvider.GetMainMenuView();
    }

    public async Task OnKeyPressed(ConsoleKeyInfo pressedKey, float deltaTimeSeconds)
    {
        if (ActiveView is null)
        {
            return;
        }

        await ActiveView.OnKeyPressed(pressedKey, deltaTimeSeconds);
    }
}
