using LoveLetter.Scenes.Abstract;
using LoveLetter.Utils;

namespace LoveLetter.Scenes.MainMenu;

/// <summary>
/// This class is responsible to render the Main Menu view
/// </summary>
public class MainMenuView(IMainMenuController mainMenuController) : AbstractView, IMainMenuView
{
    private readonly IMainMenuController _mainMenuController = mainMenuController;


    private readonly string[] _headerParts = ["-".Multiply(20), "Main Menu", "-".Multiply(20)];

    private int HeaderLength => _headerParts.Select(header => header.Length).Sum();

    public override void Render()
    {
        Console.Clear();

        int desiredLength = 10;

        DrawHeader();

        DrawKeybindInfo("N", "New Game", desiredLength);
        DrawKeybindInfo("L", "Load Game", desiredLength);
        DrawKeybindInfo("Esc", "Close application", desiredLength);

        DrawBottomLine();
    }

    private void DrawHeader()
    {
        Write(_headerParts[0], ConsoleColor.DarkGray);
        Write(_headerParts[1], ConsoleColor.Yellow);
        WriteLine(_headerParts[2], ConsoleColor.DarkGray);
    }

    private void DrawKeybindInfo(string key, string description, int leftColumnDesiredWidth)
    {
        string whitespacePrefix = "  ";

        string leftBracket = "[";
        string rightBracket = "]";

        int leftColumnWidth = whitespacePrefix.Length + leftBracket.Length + key.Length + rightBracket.Length;

        int whitespaceSeparatorLength = leftColumnDesiredWidth - leftColumnWidth;
        string whitespaceSeparator = " ".Multiply(whitespaceSeparatorLength);

        Write(whitespacePrefix);
        Write(leftBracket, ConsoleColor.Gray);
        Write(key, ConsoleColor.Yellow);
        Write(rightBracket, ConsoleColor.Gray);
        Write(whitespaceSeparator);

        WriteLine(description, ConsoleColor.White);
    }

    private void DrawBottomLine()
    {
        var bottomLine = string.Concat(Enumerable.Repeat("-", HeaderLength));
        WriteLine(bottomLine, ConsoleColor.DarkGray);
    }

    public override void Dispose() { }

    public override async Task OnKeyPressed(ConsoleKeyInfo pressedKey, float deltaTimeSeconds)
    {
        if (pressedKey.Key == ConsoleKey.Escape)
        {
            await _mainMenuController.ExitApp();
        }

        Render();
    }
}
