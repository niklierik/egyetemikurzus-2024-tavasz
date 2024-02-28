namespace LoveLetter.Scenes.Abstract;

public abstract class AbstractView : IView
{
    private void SetColor(
        ConsoleColor? foregroundColor = null,
        ConsoleColor? backgroundColor = null
    )
    {
        if (foregroundColor.HasValue)
        {
            Console.ForegroundColor = foregroundColor.Value;
        }
        if (backgroundColor.HasValue)
        {
            Console.BackgroundColor = backgroundColor.Value;
        }
    }

    public void Write(
        object message,
        ConsoleColor? foregroundColor = null,
        ConsoleColor? backgroundColor = null
    )
    {
        SetColor(foregroundColor, backgroundColor);
        Console.Write(message);
        Console.ResetColor();
    }

    public void WriteLine(
        object message,
        ConsoleColor? foregroundColor = null,
        ConsoleColor? backgroundColor = null
    )
    {
        SetColor(foregroundColor, backgroundColor);
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public abstract void Render();
    public abstract Task OnKeyPressed(ConsoleKeyInfo pressedKey, float deltaTimeSeconds);
    public abstract void Dispose();
}
