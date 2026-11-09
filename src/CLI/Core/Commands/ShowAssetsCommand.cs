using System;
using System.IO;
using System.Windows.Forms;
using Spectre.Console;

namespace TTG_Tools.CLI.Core.Commands
{
    /// <summary>
    /// Команда для виведення всіх активів (файлів) з архіву у вигляді форматованої таблиці в консолі.
    /// </summary>
    [Command("--show-assets", 
        @"<archive-path> [<game-name>] | Show all assets in archive.
                    <archive-path>: Absolute path to Archive file.
                    [<game-name>]: Game Name. Optional parameter.")]
    public class ShowAssetsCommand : ICommand
    {
        
        /// <summary>
        /// Посилання на допоміжну команду довідки (Help).
        /// </summary>
        private ICommand HelpCommand => CliRouter.Instance.Commands["--help"];

        /// <summary>
        /// Головний метод виконання команди, який перевіряє вхідні аргументи та запускає процес зчитування архіву.
        /// </summary>
        /// <param name="args">Масив аргументів командного рядка, де args[0] — шлях до архіву, args[1] (опціонально) — назва гри.</param>
        public void Run(string[] args)
        {
            if (args.Length < 1 || !File.Exists(args[0]))
            {
                HelpCommand.Run(args);
                return;
            }

            var archivePath = args[0];
            var gameName = args.Length > 1 ? args[1] : null;
            
            MainMenu.InitializeGameList();
            ShowAssets(archivePath, gameName);
        }
        

        /// <summary>
        /// Асинхронно відкриває файл архіву, витягує дані та виводить їх у вигляді кольорової таблиці через Spectre.Console.
        /// </summary>
        /// <param name="archivePath">Абсолютний шлях до файлу архіву.</param>
        /// <param name="gameName">Назва гри для ідентифікації структури (може бути null).</param>
        private void ShowAssets(string archivePath, string gameName)
        {
            var archiveUnpacker = new ArchiveUnpacker();
            
            
            
            var task = archiveUnpacker.OpenArchiveFileAsync(archivePath);
            
            while(!task.IsCompleted)
            {
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }

            if (task.IsFaulted)
            {
                ReportArchiveErrorOrThrow(task.Exception);
                return;
            }
            
            
            var grid = archiveUnpacker.FilesDataGridView;

            var table = new Table();
            table.Border(TableBorder.Ascii);
            const int fileNameIdx = 1;
            const int fileTypeIdx = 2;
            const int fileSizeIdx = 3;
            
            table.AddColumns("FileName", "FileType", "FileSize");
            
            foreach (DataGridViewRow row in grid.Rows)
            {
                if (row.IsNewRow) continue;

                var fileName = row.Cells[fileNameIdx].Value.ToString();
                var fileType = row.Cells[fileTypeIdx].Value.ToString();
                var fileSize = row.Cells[fileSizeIdx].Value.ToString();
                
                table.AddRow(fileName, fileType, fileSize);
            }
            
            AnsiConsole.Write(table);
        }

        
        /// <summary>
        /// Вибирає гру у випадаючому списку розпакувальника за її назвою.
        /// </summary>
        /// <param name="archUnpacker">Екземпляр розпакувальника архівів.</param>
        /// <param name="gameName">Назва гри для пошуку.</param>
        private void ChooseGame(ArchiveUnpacker archUnpacker, string gameName)
        {
            var gameList = MainMenu.gamelist;
            var idx = gameList.FindIndex(key => key.gamename ==  gameName);

            if (idx != -1)
            {
                archUnpacker.GameListCB.SelectedIndex = idx;
            }
        }
        
        
        /// <summary>
        /// Обробляє помилки, що виникли під час відкриття архіву, і перевіряє їх на наявність винятків типу ArgumentOutOfRangeException.
        /// </summary>
        /// <param name="aggEx">Обгортка агрегованих винятків асинхронної задачі.</param>
        /// <exception cref="AggregateException">Кидає виняток далі, якщо він не належить до очікуваних.</exception>
        private void ReportArchiveErrorOrThrow(AggregateException aggEx)
        {
            // Шукаємо серед внутрішніх винятків наш ArgumentOutOfRangeException
            bool isOutOfRange = false;
            foreach (var inner in aggEx.InnerExceptions)
            {
                if (inner is ArgumentOutOfRangeException rangeEx)
                {
                    isOutOfRange = true;
                    Console.WriteLine(@"Program cannot auto-detect game type game name manual");
                    break;
                }
            }

            if (!isOutOfRange)
            {
                // Якщо це була якась інша помилка — кидаємо її далі
                throw aggEx;
            }
        }
    }
}