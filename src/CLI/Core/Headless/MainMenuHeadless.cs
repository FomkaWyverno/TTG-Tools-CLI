using System;
using System.Collections.Generic;
using System.Linq;

namespace TTG_Tools.CLI.Core.Headless
{
    public sealed class MainMenuHeadless
    {
        // Потокобезпечний Lazy-ініціалізатор для Singleton
        private static readonly Lazy<MainMenuHeadless> _instance = 
            new Lazy<MainMenuHeadless>(() => new MainMenuHeadless());

        /// <summary>
        /// Глобальна точка доступу до єдиного екземпляра класу.
        /// </summary>
        public static MainMenuHeadless Instance => _instance.Value;

        private MainMenuHeadless()
        {
            MainMenu.InitializeGameList();
        }
        
        public List<string> GetSupportedGames()
        {
            return MainMenu.gamelist
                .Select(item => item.gamename)
                .ToList();
        }
    }
}