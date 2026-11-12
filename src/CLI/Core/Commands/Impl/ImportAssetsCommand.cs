using System;
using System.Collections.Generic;
using System.IO;
using TTG_Tools.CLI.Core.Commands.Helpers;
using TTG_Tools.CLI.Core.Headless;

namespace TTG_Tools.CLI.Core.Commands.Impl
{
    [Command("--import-assets",
        @"<input-folder> <output-folder> <game-name> | Import game assets from input to output directory. To import assets, it is important to have the original asset files nearby.
  <input-folder>: Path to the directory containing to import assets.
  <output-folder>: Path to the directory where imported assets will be saved.
  <game-name>: Source game name.")]
    public class ImportAssetsCommand : ICommand
    {
        private ICommand HelpCommand => CliRouter.Instance.Commands["--help"];

        public void Run(string[] args)
        {
            var context = AssetCommandHelper.ValidateAndPrepare(args, HelpCommand);
            if (context == null) return;

            var autoPacker = AutoPackerHeadless.Instance;
            Console.WriteLine(@"Start encrypt and import assets!");
            var isDone = autoPacker.EncryptAndImportAssets(
                context.InputFolder,
                context.OutputFolder,
                context.GameName,
                new List<ReportHandler>()
                {
                    LogImport
                }
            );
            
            Console.WriteLine(isDone
                ? @"Done!"
                : @"You wrote game name uncorrected or unsupported. Check supported games by command: --supported-games");
        }

        private void LogImport(string report)
        {
            Console.WriteLine(report);
        }
    }
}