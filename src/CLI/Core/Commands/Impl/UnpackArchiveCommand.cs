using System;
using System.IO;
using TTG_Tools.CLI.Core.Headless;

namespace TTG_Tools.CLI.Core.Commands.Impl
{
    [Command("--unpack-archive", @"<archive-path> <output-dir> [<game-name>] | Unpack all assets from archive.
                <archive-path>: Absolute path to Archive file.
                <output-dir>: Absolute path to output directory.
                [<game-name>]: Game Name. Optional parameter.")]
    public class UnpackArchiveCommand : ICommand
    {
        private ICommand HelpCommand => CliRouter.Instance.Commands["--help"];
        public void Run(string[] args)
        {
            if (args.Length < 2 || !File.Exists(args[0]))
            {
                Console.WriteLine(@"Uncorrected arguments provided.");
                HelpCommand.Run(args);
            }

            var archivePath = args[0];
            var outputDir = args[1];

            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception)
            {
                Console.WriteLine(@"Invalid output directory path!");
                return;
            }
            
            
            var gameName = args.Length > 1 ? args[1] : null;
            Console.WriteLine(@"Unpacking archive...");
            ArchiveUnpackerHeadless.Instance.UnpackArchive(archivePath, outputDir,gameName);
            Console.WriteLine(@"Done!");
        }
    }
}