namespace TTG_Tools.CLI.Core.Headless
{
    public class ArchiveUnpackerHeadless
    {
        public static void ExtractArchive(string archivePath, string outputPath, string gameName)
        {
            MainMenu.InitializeGameList();
            var unpacker = new ArchiveUnpacker();

            foreach (var item in unpacker.GameListCB.Items)
            {
                
            }
        }
    }
}