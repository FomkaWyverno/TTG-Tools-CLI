using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// Встановлює шлях до вхідної теки з ассетами, якщо вказана директорія існує на диску.
        /// </summary>
        /// <param name="inputFolderPath">Шлях до директорії, яку потрібно встановити як вхідну.</param>
        /// <returns>
        /// Повертає <see langword="true"/>, якщо тека існує та шлях успішно збережено в <c>MainMenu.settings.pathForInputFolder</c>; 
        /// інакше повертає <see langword="false"/>.
        /// </returns>
        public bool SetInputAssetsFolder(string inputFolderPath)
        {
            if (Directory.Exists(inputFolderPath))
            {
                MainMenu.settings.pathForInputFolder = inputFolderPath;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Створює вказану директорію (разом із усіма проміжними теками, якщо вони відсутні) 
        /// та встановлює її шлях як вихідну теку ассетів у налаштуваннях.
        /// </summary>
        /// <param name="outputFolderPath">Шлях до директорії, яка буде створена та призначена як вихідна.</param>
        public void SetOutputAssetsFolder(string outputFolderPath)
        {
            Directory.CreateDirectory(outputFolderPath);
            MainMenu.settings.pathForOutputFolder = outputFolderPath;
        }
        
        
        /// <summary>
        /// Зберігає поточні налаштування головного меню у файл конфігурації.
        /// </summary>
        public void SaveSettings()
        {
            Settings.SaveConfig(MainMenu.settings);
        }
        
        public Settings GetSettings()
        {
            return MainMenu.settings;
        }
    }
}