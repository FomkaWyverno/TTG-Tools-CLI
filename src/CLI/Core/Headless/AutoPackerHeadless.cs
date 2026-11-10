using System;

namespace TTG_Tools.CLI.Core.Headless
{
    /// <summary>
    /// Singleton-клас для керування роботою з архівами в безголовому (headless) режимі без прямого використання повноцінного UI.
    /// </summary>
    public sealed class AutoPackerHeadless
    {
        // Потокобезпечний Lazy-ініціалізатор для Singleton
        private static readonly Lazy<AutoPackerHeadless> _instance = 
            new Lazy<AutoPackerHeadless>(() => new AutoPackerHeadless());

        /// <summary>
        /// Глобальна точка доступу до єдиного екземпляра класу.
        /// </summary>
        public static AutoPackerHeadless Instance => _instance.Value;

        private AutoPackerHeadless()
        {
            MainMenu.InitializeGameList();
        }
    }
}