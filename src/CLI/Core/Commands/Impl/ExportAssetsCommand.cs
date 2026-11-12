using System;
using System.Collections.Generic;
using System.IO;
using TTG_Tools.CLI.Core.Commands.Helpers;
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
            var context = AssetCommandHelper.ValidateAndPrepare(args, HelpCommand);
            if (context == null) return;
            
            var autoPacker = AutoPackerHeadless.Instance;
            Console.WriteLine(@"Start decrypt and export assets!");
            var isDone = autoPacker.DecryptAndExportAssets(
                context.InputFolder,
                context.OutputFolder,
                context.GameName,
                new List<ReportHandler>()
                {
                    LogDecrypt
                }
            );

            Console.WriteLine(isDone
                ? @"Done!"
                : @"You wrote game name uncorrected or unsupported. Check supported games by command: --supported-games");
        }

        private void LogDecrypt(string report)
        {
            Console.WriteLine(report);
        }
    }
}