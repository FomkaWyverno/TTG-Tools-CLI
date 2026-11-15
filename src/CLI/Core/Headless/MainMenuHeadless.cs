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
        /// Перевіряє існування вказаної вхідної теки архіву та, якщо вона існує, 
        /// встановлює її шлях у налаштуваннях.
        /// </summary>
        /// <param name="inputArchiveFolderPath">Шлях до вхідної теки архіву.</param>
        /// <returns>Повертає <c>true</c>, якщо тека існує і шлях успішно збережено; інакше <c>false</c>.</returns>
        public bool SetInputArchiveFolder(string inputArchiveFolderPath)
        {
            if (!Directory.Exists(inputArchiveFolderPath)) return false;
            MainMenu.settings.inputDirPath = inputArchiveFolderPath;
            return true;
        }

        /// <summary>
        /// Перевіряє шлях до файлу архіву, створює проміжні теки за потреби 
        /// та оновлює шлях до архіву в налаштуваннях.
        /// </summary>
        /// <param name="archiveFilePath">Повний шлях до файлу архіву.</param>
        public void SetArchiveFilePath(string archiveFilePath)
        {
            if (!string.IsNullOrEmpty(archiveFilePath))
            {
                // Отримуємо шлях до теки з переданого шляху до файлу
                string directoryPath = Path.GetDirectoryName(archiveFilePath);
        
                // Якщо шлях містить теку і вона ще не існує — створюємо її
                if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
            }

            MainMenu.settings.archivePath = archiveFilePath;
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