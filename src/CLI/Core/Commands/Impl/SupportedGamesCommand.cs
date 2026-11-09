using System;
using TTG_Tools.CLI.Core.Headless;

namespace TTG_Tools.CLI.Core.Commands.Impl
{
    [Command("--supported-games", "Shows supported games")]
    public class SupportedGamesCommand : ICommand
    {
        public void Run(string[] args)
        {
            var gameList = MainMenuHeadless.Instance.GetSupportedGames();
            foreach (var item in gameList) Console.WriteLine(item);
        }
    }
}