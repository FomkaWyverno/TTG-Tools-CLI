using System;
using System.IO;
using Spectre.Console;
using TTG_Tools.CLI.Core.Commands.Helpers;
using TTG_Tools.CLI.Core.Headless;

namespace TTG_Tools.CLI.Core.Commands.Impl
{
    /// <summary>
    ///     Команда для виведення всіх активів (файлів) з архіву у вигляді форматованої таблиці в консолі.
    /// </summary>
    [Command("--show-assets",
        @"<archive-path> [<game-name>] | Show all assets in archive.
                    <archive-path>: Absolute path to Archive file.
                    [<game-name>]: Game Name. Optional parameter.")]
    public class ShowAssetsCommand : ICommand
    {
        /// <summary>
        ///     Посилання на допоміжну команду довідки (Help).
        /// </summary>
        private ICommand HelpCommand => CliRouter.Instance.Commands["--help"];

        /// <summary>
        ///     Головний метод виконання команди, який перевіряє вхідні аргументи та запускає процес зчитування архіву.
        /// </summary>
        /// <param name="args">Масив аргументів командного рядка, де args[0] — шлях до архіву, args[1] (опціонально) — назва гри.</param>
        public void Run(string[] args)
        {
            if (args.Length < 1 || !File.Exists(args[0]))
            {
                Console.WriteLine(@"Uncorrected arguments provided.");
                HelpCommand.Run(args);
                return;
            }

            var archivePath = args[0];
            var gameName = args.Length > 1 ? args[1] : null;

            MainMenu.InitializeGameList();
            ShowAssets(archivePath, gameName);
        }


        /// <summary>
        ///     Асинхронно відкриває файл архіву, витягує дані та виводить їх у вигляді кольорової таблиці через Spectre.Console.
        /// </summary>
        /// <param name="archivePath">Абсолютний шлях до файлу архіву.</param>
        /// <param name="gameName">Назва гри для ідентифікації структури (може бути null).</param>
        private void ShowAssets(string archivePath, string gameName)
        {
            try
            {
                var assetInfos = ArchiveUnpackerHeadless.Instance.GetAssetInfos(archivePath, gameName);

                var table = new Table();
                table.Border(TableBorder.Ascii);


                table.AddColumns("FileName", "FileType", "FileSize");

                foreach (var item in assetInfos) table.AddRow(item.FileName, item.FileType, item.FileSize);

                AnsiConsole.Write(table);
            }
            catch (AggregateException aggEx)
            {
                ArchiveErrorHelper.HandleArchiveError(aggEx);
            }
        }
    }
}