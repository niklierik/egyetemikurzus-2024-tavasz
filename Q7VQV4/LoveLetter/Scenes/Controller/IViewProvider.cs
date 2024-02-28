using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LoveLetter.Scenes.MainMenu;

namespace LoveLetter.Scenes.Controller;

public interface IViewProvider
{
    public IMainMenuView GetMainMenuView();
}
