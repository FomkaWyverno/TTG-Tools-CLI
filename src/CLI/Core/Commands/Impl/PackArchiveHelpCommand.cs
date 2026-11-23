using System;
using System.Collections.Generic;
using TTG_Tools.CLI.Core.Headless;
using TTG_Tools.Core.Enums;

namespace TTG_Tools.CLI.Core.Commands.Impl
{
    [Command("--pack-archive-help",
        @"Print help information for archive packing
                Use one of the final following parameters
                --archive-type [1 or 2] 1 = TTARCH (old games) 2 = TTARCH2 (newer games)
                --version-archive Set archive version one of available. Example: --version-archive=2. IMPORTANT use it parameter after --archive-type
                --display-versions-archive This is the final parameter. It displays the available versions for selected parameters. Necessary parameters: archive-type
                --display-compression-types This is the final parameter. It displays the available compression types for selected parameters.")]
    public class PackArchiveHelpCommand : ICommand
    {
        private ICommand HelpCommand => CliRouter.Instance.Commands["--help"];

        private const string VERSIONS_ARCHIVE_PARAM = "--display-versions-archive";
        private const string COMPRESSION_TYPES_PARAM = "--display-compression-types";
        private const string ARCHIVE_TYPE_PARAM = "--archive-type";
        private const string VERSION_ARCHIVE_PARAM = "--version-archive";

        private readonly Dictionary<string, Func<string, bool>> argHandlers;

        public PackArchiveHelpCommand()
        {
            argHandlers =
                new Dictionary<string, Func<string, bool>>
                {
                    [ARCHIVE_TYPE_PARAM] = SetArchiveType,
                    [VERSION_ARCHIVE_PARAM] = SetVersionArchive
                };
        }

        public void Run(string[] args)
        {
            var hasArgs = args.Length > 0;
            var isVersionsArchive = hasArgs && args[args.Length - 1] == VERSIONS_ARCHIVE_PARAM;
            var isCompressionTypes = hasArgs && args[args.Length - 1] == COMPRESSION_TYPES_PARAM;
            var hasAnyFinalParam = isVersionsArchive || isCompressionTypes;

            if (!hasAnyFinalParam)
            {
                HelpCommand.Run(args);
                return;
            }

            var successApply = AppliesNotFinalParameters(args);
            if (!successApply) return;

            if (isVersionsArchive)
            {
                DisplayVersionsArchive();
                return;
            }

            DisplayCompressionTypes();
        }


        private bool AppliesNotFinalParameters(string[] args)
        {
            for (int i = 0; i < args.Length - 1; i++)
            {
                var parts = args[i].Split('=');
                if (parts.Length == 0)
                {
                    Console.WriteLine(@"Empty parameter found.");
                    return false;
                }

                var arg = parts[0];

                if (!argHandlers.TryGetValue(arg, out var handler))
                {
                    Console.WriteLine($@"Unknown parameter found >>> {arg}");
                    return false;
                }

                handler(args[i]);
            }

            return true;
        }

        private void DisplayVersionsArchive()
        {
            var packer = ArchivePackerHeadless.Instance;
            var versions = packer.GetAvailableVersionsArchive();

            Console.WriteLine(@"Versions:");
            foreach (var version in versions)
            {
                Console.WriteLine(version);
            }
        }

        private void DisplayCompressionTypes()
        {
            var packer = ArchivePackerHeadless.Instance;
            var compressionTypes = packer.GetAvailableTypeCompression();
            
            if (compressionTypes.Count == 0)
            {
                Console.WriteLine(@"No compression types available.");
                return;
            }
            
            Console.WriteLine(@"Compression types:");
            foreach (var compressionType in compressionTypes) Console.WriteLine(compressionType);
        }

        private bool SetArchiveType(string arg)
        {
            var packer = ArchivePackerHeadless.Instance;

            var parts = arg.Split('=');
            if (parts.Length != 2)
            {
                Console.WriteLine($@"Argument {ARCHIVE_TYPE_PARAM} uncorrected. Example: {ARCHIVE_TYPE_PARAM}=1");
                return false;
            }

            var archiveType = parts[1];

            if (archiveType == "1")
            {
                packer.SetTypeArchive(TtrachTypeEnum.TTRACH);
                return true;
            }

            if (archiveType != "2")
            {
                Console.WriteLine($@"Unknown archive type `{archiveType}` Print one of 1 or 2");
                return false;
            }
            packer.SetTypeArchive(TtrachTypeEnum.TTRACH2);
            return true;
        }

        private bool SetVersionArchive(string arg)
        {
            var packer = ArchivePackerHeadless.Instance;
            
            var parts = arg.Split('=');
            if (parts.Length != 2)
            {
                Console.WriteLine($@"Argument {VERSION_ARCHIVE_PARAM} uncorrected.  Example: {VERSION_ARCHIVE_PARAM}=1. To find out available version archive use {VERSIONS_ARCHIVE_PARAM}");
                return false;
            }
            
            var archVersion = parts[1];
            var availableVersions = packer.GetAvailableVersionsArchive();

            if (!availableVersions.Contains(archVersion))
            {
                Console.WriteLine($@"Uncorrected  version `{archVersion}` Print one of {string.Join(", ", availableVersions)}");
                return false;
            }

            var isChangedVersion = packer.SetVersionOfArchive(archVersion);

            if (isChangedVersion) return true;
            
            Console.WriteLine($@"Could not change version of archive `{archVersion}`");
            return false;

        }
    }
}