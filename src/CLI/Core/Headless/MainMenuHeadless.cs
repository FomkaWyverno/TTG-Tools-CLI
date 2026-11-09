using System;

namespace TTG_Tools.CLI.Core.Headless
{
    public class MainMenuHeadless
    {
        public static void ShowSupportedGames()
        {
            MainMenu.InitializeGameList();

            foreach (var item in MainMenu.gamelist)
            {
                Console.WriteLine($@"- {item.gamename}");
            }
        }
    }
}