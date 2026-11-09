using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TTG_Tools.CLI.Core.Infrastructure;

namespace TTG_Tools.CLI.Core.Headless
{
    /// <summary>
    /// Singleton-клас для керування роботою з архівами в безголовому (headless) режимі без прямого використання повноцінного UI.
    /// </summary>
    public sealed class ArchiveUnpackerHeadless
    {
        // Потокобезпечний Lazy-ініціалізатор для Singleton
        private static readonly Lazy<ArchiveUnpackerHeadless> _instance = 
            new Lazy<ArchiveUnpackerHeadless>(() => new ArchiveUnpackerHeadless());

        /// <summary>
        /// Глобальна точка доступу до єдиного екземпляра класу.
        /// </summary>
        public static ArchiveUnpackerHeadless Instance => _instance.Value;

        private readonly ArchiveUnpacker _archiveUnpacker;
        
        
        /// <summary>
        /// Ініціалізує новий екземпляр класу <see cref="ArchiveUnpackerHeadless"/>, 
        /// завантажуючи список ігор та створюючи базовий розпакувальник.
        /// </summary>
        private ArchiveUnpackerHeadless()
        {
            MainMenu.InitializeGameList();
            _archiveUnpacker = new ArchiveUnpacker();
        }

        /// <summary>
        /// Відкриває вказаний архів, витягує дані про файли з внутрішньої таблиці та повертає їх у вигляді списку DTO-об'єктів.
        /// </summary>
        /// <param name="archivePath">Абсолютний або відносний шлях до файлу архіву.</param>
        /// <param name="gameName">Опціональна назва гри для ручного вибору структури архіву.</param>
        /// <returns>Список об'єктів <see cref="AssetInfo"/>, що містять інформацію про активи всередині архіву.</returns>
        public List<AssetInfo> GetAssetInfos(string archivePath, string gameName = null)
        {
            OpenArchive(archivePath, gameName);

            var grid = _archiveUnpacker.FilesDataGridView;

            var list = new List<AssetInfo>();
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;
                var cells = row.Cells;
                list.Add(new AssetInfo
                (
                    cells[1].Value?.ToString(),
                    cells[2].Value?.ToString(),
                    cells[3].Value?.ToString(),
                    cells[4].Value?.ToString()
                ));
            }

            return list;
        }
        
        /// <summary>
        /// Синхронно відкриває файл архіву в асинхронному середовищі розпакувальника.
        /// </summary>
        /// <param name="archivePath">Шлях до архіву.</param>
        /// <param name="gameName">Назва гри для автовибору (якщо передана).</param>
        /// <exception cref="AggregateException">Викидається, якщо під час відкриття архіву сталася помилка.</exception>
        private void OpenArchive(string archivePath, string gameName = null)
        {
            if (gameName != null)
            {
                ChooseGame(gameName);
            }
            
            var task = _archiveUnpacker.OpenArchiveFileAsync(archivePath);
            
            while(!task.IsCompleted)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }

            if (task.IsFaulted && task.Exception != null)
            {
                throw task.Exception;
            }
        }
        
        /// <summary>
        /// Знаходить та вибирає гру у випадаючому списку розпакувальника за її назвою.
        /// </summary>
        /// <param name="gameName">Назва гри для пошуку у глобальному списку.</param>
        private void ChooseGame(string gameName)
        {
            var gameList = MainMenu.gamelist;
            var idx = gameList.FindIndex(key => key.gamename ==  gameName);

            if (idx != -1)
            {
                _archiveUnpacker.GameListCB.SelectedIndex = idx;
            }
        }
        
        
    }
}