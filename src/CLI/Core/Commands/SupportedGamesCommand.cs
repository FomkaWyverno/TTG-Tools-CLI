using System;

namespace TTG_Tools.CLI.Core.Commands
{
    [Command("--supported-games", "Shows supported games")]
    public class SupportedGamesCommand : ICommand
    {
        public void Run(string[] args)
        {
            MainMenu.InitializeGameList();

            foreach (var item in MainMenu.gamelist)
            {
                Console.WriteLine(item.gamename);
            }
        }
    }
}