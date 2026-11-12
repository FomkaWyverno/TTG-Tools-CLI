using System;
using System.Collections.Generic;
using System.IO;
using TTG_Tools.CLI.Core.Headless;

namespace TTG_Tools.CLI.Core.Commands.Impl
{
    [Command("--export-assets",
        @"<input-folder> <output-folder> <game-name> | Export game assets from input to output directory.
  <input-folder>: Path to the directory containing source game assets.
  <output-folder>: Path to the directory where extracted assets will be saved.
  <game-name>: Source game name.")]
    public class ExportAssetsCommand : ICommand
    {
        private ICommand HelpCommand => CliRouter.Instance.Commands["--help"];

        public void Run(string[] args)
        {
            if (args.Length < 3 || !Directory.Exists(args[0]))
            {
                Console.WriteLine(@"Uncorrected receive arguments!");
                HelpCommand.Run(args);
                return;
            }

            try
            {
                Directory.CreateDirectory(args[1]);
            }
            catch (Exception)
            {
                Console.WriteLine(@"Invalid output folder!");
                HelpCommand.Run(args);
            }

            var menu = MainMenuHeadless.Instance;
            if (!menu.GetSupportedGames().Exists(k => k.Equals(args[2])))
            {
                Console.WriteLine($@"Unsupported game assets! You wrote: ""{args[2]}""");
                return;
            }

            var inputAssetsFolder = args[0];
            var outputAssetsFolder = args[1];
            var gameName = args[2];

            var autoPacker = AutoPackerHeadless.Instance;

            Console.WriteLine(@"Start decrypt and export assets!");
            autoPacker.DecryptAndExportAssets(
                inputAssetsFolder,
                outputAssetsFolder,
                gameName,
                new List<ReportHandler>()
                {
                    LogDecrypt
                }
            );
            Console.WriteLine(@"Done!");
        }

        private void LogDecrypt(string report)
        {
            Console.WriteLine(report);
        }
    }
}