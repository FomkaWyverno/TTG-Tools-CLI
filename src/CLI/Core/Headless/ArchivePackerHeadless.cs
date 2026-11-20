using System;
using System.IO;

namespace TTG_Tools.CLI.Core.Headless
{
    public sealed class ArchivePackerHeadless
    {
        // Потокобезпечний Lazy-ініціалізатор для Singleton
        private static readonly Lazy<ArchivePackerHeadless> _instance = 
            new Lazy<ArchivePackerHeadless>(() => new ArchivePackerHeadless());

        /// <summary>
        /// Глобальна точка доступу до єдиного екземпляра класу.
        /// </summary>
        public static ArchivePackerHeadless Instance => _instance.Value;

        private readonly ArchivePacker _archivePacker;
        private ArchivePackerHeadless()
        {
            _archivePacker = new ArchivePacker();
        }
        
        
    }
}