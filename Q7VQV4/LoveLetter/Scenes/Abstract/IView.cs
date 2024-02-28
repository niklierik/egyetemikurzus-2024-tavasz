namespace LoveLetter.Scenes.Abstract;

/// <summary>
/// This interface describes common functionalities between views
///
/// Each view instance should represent a currently rendered
/// </summary>
public interface IView : IDisposable
{
    void Write(
        object message,
        ConsoleColor? foregroundColor = null,
        ConsoleColor? backgroundColor = null
    );

    void WriteLine(
        object message,
        ConsoleColor? foregroundColor = null,
        ConsoleColor? backgroundColor = null
    );

    void Render();

    /// <summary>
    /// Event handler for keyboard input handling
    /// </summary>
    /// <param name="pressedKey">The pressed key</param>
    /// <param name="deltaTimeSeconds">Time between the last handled input</param>
    Task OnKeyPressed(ConsoleKeyInfo pressedKey, float deltaTimeSeconds);
}
