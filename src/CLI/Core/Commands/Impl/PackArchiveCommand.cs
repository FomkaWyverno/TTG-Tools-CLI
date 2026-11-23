namespace TTG_Tools.CLI.Core.Commands.Impl
{
    [Command("--pack-archive",
        @"Packing assets to archive.
                <input-folder> - Folder with assets
                <output-path-to-archive> - Path to archive
                <ttarch-type> TTARCH type. Choose one of 1 or 2
                <version-archive> Choose version of archive
                [<compression-type>] Choose compression type")]
    public class PackArchiveCommand : ICommand
    {
        public void Run(string[] args)
        {
            
        }
    }
}