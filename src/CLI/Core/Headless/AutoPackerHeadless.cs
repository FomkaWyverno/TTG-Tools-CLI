using System;
using System.Collections.Generic;

namespace TTG_Tools.CLI.Core.Headless
{
    /// <summary>
    /// Singleton-клас для керування роботою з архівами в безголовому (headless) режимі без прямого використання повноцінного UI.
    /// </summary>
    public sealed class AutoPackerHeadless
    {
        // Потокобезпечний Lazy-ініціалізатор для Singleton
        private static readonly Lazy<AutoPackerHeadless> _instance = 
            new Lazy<AutoPackerHeadless>(() => new AutoPackerHeadless());

        /// <summary>
        /// Глобальна точка доступу до єдиного екземпляра класу.
        /// </summary>
        public static AutoPackerHeadless Instance => _instance.Value;

        private readonly AutoPacker _autoPacker;
        private AutoPackerHeadless()
        {
            MainMenu.InitializeGameList();
            _autoPacker = new AutoPacker();
            _autoPacker.AutoPackerLoad();
        }

        /// <summary>
        /// Розшифровує та експортує сирі ігрові аcсети у зручні для читання та редагування формати (наприклад, текстові файли, аудіо тощо).
        /// </summary>
        /// <param name="inputPath">Шлях до вхідної директорії або файлу з зашифрованими асетами.</param>
        /// <param name="outputPath">Шлях до вихідної директорії, куди будуть збережені розпаковані та розшифровані дані.</param>
        /// <param name="gameName">Назва гри з якої розпаковані ассети</param>
        /// <param name="reportHandlers">Слухач для репортів прогресу розшифрування</param>
        public void DecryptAndExportAssets(string inputPath, string outputPath, string gameName, List<ReportHandler> reportHandlers = null)
        {
            var menu = MainMenuHeadless.Instance;
            
            menu.SetInputAssetsFolder(inputPath);
            menu.SetOutputAssetsFolder(outputPath);
            menu.SaveSettings();

            if (!ChooseGame(gameName))
            {
                Console.WriteLine(@"You wrote game name uncorrected or unsupported. Check supported games by command: --supported-games");
                return;
            }
            _autoPacker.DecryptExportClickSync(reportHandlers);
        }


        /// <summary>
        /// Обирає гру для дешифрування ассетів
        /// </summary>
        /// <param name="gameName">назва гри</param>
        /// <returns>Повертає true якщо знайшло та встановило гру</returns>
        private bool ChooseGame(string gameName)
        {
            var menu = MainMenuHeadless.Instance;
            var gameList = menu.GetSupportedGames();
            var gameIdx = gameList.IndexOf(gameName);
            if (gameIdx != -1)
            {
                _autoPacker.BlowfishKeysComboBox.SelectedIndex = gameIdx;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}