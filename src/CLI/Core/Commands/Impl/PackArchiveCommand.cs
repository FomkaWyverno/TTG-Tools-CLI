using System;
using System.IO;
using System.Linq;
using TTG_Tools.CLI.Core.Commands.Helpers;
using TTG_Tools.CLI.Core.Headless;
using TTG_Tools.Core.Enums;

namespace TTG_Tools.CLI.Core.Commands.Impl
{
    [Command("--pack-archive",
        @"Packing assets to archive.
                <input-folder> - Folder with assets
                <output-path-to-archive> - Path to archive
                <ttarch-type> TTARCH type. Choose one of 1 or 2
                <version-archive> Choose version of archive
                <game-name> - Name of game
                [<compression-type>] Choose compression type")]
    public class PackArchiveCommand : ICommand
    {
        private ICommand HelpCommand => CliRouter.Instance.Commands["--help"];

        public void Run(string[] args)
        {
            if (args.Length < 5)
            {
                HelpCommand.Run(args);
                return;
            }

            var packer = ArchivePackerHeadless.Instance;

            var inputFolder = args[0];
            var outputArchivePath = args[1];
            var ttarchType = args[2];
            var versionArchive = args[3];
            var gameName = args[4];
            var compressionType = args.Length > 5 ? args[5] : null;

            if (!Directory.Exists(inputFolder))
            {
                Console.WriteLine(@"Input folder does not exist.");
                return;
            }

            var ttarchTypeEnum = ttarchType == "1" ? TtrachTypeEnum.TTRACH :
                ttarchType == "2" ? TtrachTypeEnum.TTRACH2 :
                (TtrachTypeEnum?)null;

            if (ttarchTypeEnum == null)
            {
                Console.WriteLine(@"TTARCH type can only be 1 or 2");
                return;
            }

            packer.SetTypeArchive((TtrachTypeEnum)ttarchTypeEnum);
            if (!packer.SetVersionOfArchive(versionArchive))
            {
                Console.WriteLine($@"Version archive ""{versionArchive}"" does not exist.");
                return;
            }

            if (compressionType != null && !packer.SetTypeCompression(compressionType))
            {
                Console.WriteLine($@"Compression type ""{compressionType}"" chose uncorrected");
            }
            
            if (!packer.SetEncryptionKeyGame(gameName))
            {
                Console.WriteLine($@"Game name ""{gameName}"" does not supported.");
                return;
            }
            
            if (!packer.UpdateInputFolder(inputFolder))
            {
                Console.WriteLine(@"Input folder does not exist.");
                return;
            }
            
            if (!packer.UpdateOutputArchive(outputArchivePath))
            {
                Console.WriteLine(@"Uncorrected path to archive.");
                return;
            }

            packer.OnReportedAdded += Console.WriteLine;
            Console.WriteLine(@"Start build archive.");
            packer.RunBuild().WaitWithDoEvents();
            Console.WriteLine(@"Done!");
        }
    }
}