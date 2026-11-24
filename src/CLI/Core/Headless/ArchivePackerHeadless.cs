using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TTG_Tools.Core.Enums;

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
            MainMenu.InitializeGameList();
            _archivePacker = new ArchivePacker();
            _archivePacker.ManualLoad();
        }

        /// <summary>
        /// Встановлює певний тип архіву який потрібно пакувати
        /// </summary>
        /// <param name="type">Тип архіву .ttarch / .ttarch2</param>
        public void SetTypeArchive(TtrachTypeEnum type)
        {
            _archivePacker.SetTypeArchive(type);
        }

        /// <summary>
        /// Повертає список доступних версій архіву з випадаючого списку вибору версії.
        /// </summary>
        /// <returns>
        /// Список текстових представлень елементів із <c>versionSelection.Items</c>.
        /// Якщо елементів немає, повертає порожній список.
        /// </returns>
        public List<string> GetAvailableVersionsArchive()
        {
            return _archivePacker.GetAvailableVersionsOfArchive();
        }

        /// <summary>
        /// Обирає вказану версію архіву у спадному списку <c>versionSelection</c>, якщо вона є серед доступних.
        /// </summary>
        /// <param name="version">Рядкове представлення версії архіву, яке потрібно встановити.</param>
        /// <returns>
        /// <see langword="true"/>, якщо версію знайдено і <c>SelectedIndex</c> оновлено;
        /// інакше — <see langword="false"/>.
        /// </returns>
        public bool SetVersionOfArchive(string version)
        {
            return _archivePacker.SetVersionOfArchive(version);
        }

        /// <summary>
        /// Оновлює шлях до інпут теки де знаходяться асети для пакування у архів.
        /// </summary>
        /// <param name="inputFolderPath"></param>
        /// <returns></returns>
        public bool UpdateInputFolder(string inputFolderPath)
        {
            return _archivePacker.UpdateInputFolder(inputFolderPath);
        }
        
        /// <summary>
        /// Створює вихідну теку (за потреби) та оновлює шлях збереження архіву в інтерфейсі й конфігурації.
        /// </summary>
        /// <param name="outputFolderPath">Шлях до цільової теки виводу.</param>
        /// /// <returns>true - якщо шлях до архіву було встановлено, та за потреби створено дочірню теку, інакше false</returns>
        public bool UpdateOutputArchive(string outputFolderPath)
        {
            return _archivePacker.UpdateOutputArchive(outputFolderPath);
        }

        /// <summary>
        /// Встановлює стан прапорця увімкнення стиснення, якщо він видимий на формі.
        /// </summary>
        /// <param name="value">Бажаний стан прапорця (<c>checkCompress.Checked</c>).</param>
        /// <returns>
        /// <see langword="true"/>, якщо прапорець видимий і його стан було оновлено;
        /// <see langword="false"/>, якщо елемент прихований і значення не застосовано.
        /// </returns>
        public bool SetCompressionCheckBox(bool value)
        {
            return _archivePacker.SetCompressionCheckBox(value);
        }

        /// <summary>
        /// Повертає список доступних типів стиснення зі спадного списку <c>compressionCB</c>.
        /// </summary>
        /// <returns>
        /// Список текстових представлень елементів із <c>compressionCB.Items</c>.
        /// Якщо елементів немає, повертає порожній список.
        /// </returns>
        public List<string> GetAvailableTypeCompression()
        {
            return _archivePacker.GetAvailableTypeCompression();
        }

        /// <summary>
        /// Обирає вказаний тип стиснення у спадному списку <c>compressionCB</c>, якщо він доступний.
        /// </summary>
        /// <param name="compressionType">Назва типу стиснення, яку потрібно встановити.</param>
        /// <returns>
        /// <see langword="true"/>, якщо тип знайдено у доступних варіантах і <c>SelectedIndex</c> оновлено;
        /// <see langword="false"/>, якщо такий тип стиснення відсутній у списку.
        /// </returns>
        public bool SetTypeCompression(string compressionType)
        {
            return _archivePacker.SetTypeCompression(compressionType);
        }

        /// <summary>
        /// Обирає гру зі списку ключів шифрування за її назвою та викликає обробник зміни вибраного елемента.
        /// </summary>
        /// <param name="gameName">Назва гри (<c>gamename</c>), яку потрібно знайти у <c>MainMenu.gamelist</c>.</param>
        /// <returns>
        /// <see langword="true"/>, якщо гру знайдено і <c>SelectedIndex</c> оновлено;
        /// інакше — <see langword="false"/>.
        /// </returns>
        public bool SetEncryptionKeyGame(string gameName)
        {
            return _archivePacker.SetEncryptionKeyGame(gameName);
        }

        public event ReportEventHandler OnReportedAdded
        {
            add =>  _archivePacker.OnReportedAdded += value;
            remove => _archivePacker.OnReportedAdded -= value;
        }
        
        /// <summary>
        /// Запускає створення архіву
        /// </summary>
        public Task RunBuild()
        {
            return _archivePacker.RunBuild();
        }
    }
}