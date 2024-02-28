using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoveLetter.Scenes.MainMenu;

public class MainMenuController : IMainMenuController
{
    public Task NewGame()
    {
        return Task.CompletedTask;
    }

    public Task ExitApp()
    {
        Environment.Exit(0);
        return Task.CompletedTask;
    }
}
