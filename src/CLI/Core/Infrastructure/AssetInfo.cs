namespace TTG_Tools.CLI.Core.Infrastructure
{
    /// <summary>
    /// Представляє інформацію про один файл (ассет) всередині архіву.
    /// </summary>
    public class AssetInfo
    {
        public readonly string FileName;
        public readonly string FileType;
        public readonly string FileSize;
        public readonly string FileOffset;

        public AssetInfo(string fileName, string fileType, string fileSize, string fileOffset)
        {
            FileName = fileName;
            FileType = fileType;
            FileSize = fileSize;
            FileOffset = fileOffset;
        }
    }
}