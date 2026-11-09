using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using TTG_Tools.CLI.Core;
using TTG_Tools.CLI.Core.Headless;

namespace TTG_Tools
{
    static class Program
    {
        public static bool FirstTime = true;
        
        [STAThread]
        static void Main(string[] args)
        {
            InitializationConfig();
            CliRouter.Instance.Route(args);
        }

        private static void InitializationConfig()
        {
            // 1. Ensure the presence of a configuration
            string xmlPath = Settings.EnsureConfigAvailable();
            if (File.Exists(xmlPath))
            {
                FirstTime = true;
            }

            // Load the saved config early so the interface language is known before any form is
            // constructed. MainMenu_Load re-reads config.xml afterwards (harmless); we only need
            // the language here. On first run there is no config, so the UI stays English.
            Settings earlyConfig = Settings.LoadConfigOrNull();
            if (earlyConfig != null)
            {
                MainMenu.settings = earlyConfig;
                //A recoverable backup/profile is still an existing configuration even when the
                //primary config.xml was missing or damaged.
                FirstTime = true;
            }
        }
        
        // .NET Framework 4.8 has no Application.SetDefaultFont, so we override
        // Control.defaultFont (the lazy backing field of Control.DefaultFont)
        // via reflection. Every Form built after this point inherits the font
        // we set; controls that don't override it pick it up automatically.
        private static byte[] HexToKey(string hex)
        {
            byte[] key = new byte[hex.Length / 2];
            for (int i = 0; i < key.Length; i++) key[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return key;
        }

        private static void ApplyDefaultFont()
        {
            try
            {
                Font font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
                FieldInfo fi = typeof(Control).GetField(
                    "defaultFont",
                    BindingFlags.Static | BindingFlags.NonPublic);
                if (fi != null)
                {
                    fi.SetValue(null, font);
                }
            }
            catch
            {
                // If the runtime renamed the field this fallback is harmless;
                // forms simply revert to the OS default and the cosmetic bug
                // returns instead of crashing the app.
            }
        }
    }
}
