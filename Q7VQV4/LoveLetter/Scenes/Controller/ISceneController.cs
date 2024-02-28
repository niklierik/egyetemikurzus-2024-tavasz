using LoveLetter.Scenes.Abstract;

namespace LoveLetter.Scenes.Controller;

/// <summary>
/// This "service" will be responsible for the scene transitions
/// </summary>
public interface ISceneController
{
    public IView ActiveView { get; }

    public bool IsInitialized { get; }

    public void OpenMainMenu();

    public Task OnKeyPressed(ConsoleKeyInfo pressedKey, float deltaTimeSeconds);
}
