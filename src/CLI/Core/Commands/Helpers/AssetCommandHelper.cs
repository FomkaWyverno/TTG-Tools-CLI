using System;
using System.IO;
using TTG_Tools.CLI.Core.Headless;

namespace TTG_Tools.CLI.Core.Commands.Helpers
{
    public static class AssetCommandHelper
    {
        public class ValidationResult
        {
            public string InputFolder  { get; set; }
            public string OutputFolder { get; set; }
            public string GameName { get; set; }
        }
        
        /// <summary>
        /// Валідує вхідні аргументи командного рядка для команд асетів (імпорт/експорт).
        /// </summary>
        public static ValidationResult ValidateAndPrepare(string[] args, ICommand helpCommand)
        {
            // 1. Перевірка кількості аргументів та існування вхідної директорії
            if (args.Length < 3 || !Directory.Exists(args[0]))
            {
                Console.WriteLine(@"Uncorrected receive arguments!");
                helpCommand.Run(args);
                return null;
            }

            // 2. Створення вихідної директорії
            try
            {
                Directory.CreateDirectory(args[1]);
            }
            catch (Exception)
            {
                Console.WriteLine(@"Invalid output folder!");
                helpCommand.Run(args);
                return null;
            }

            // 3. Перевірка підтримки гри
            var gameName = args[2];
            var menu = MainMenuHeadless.Instance;
            if (!menu.GetSupportedGames().Exists(k => k.Equals(gameName)))
            {
                Console.WriteLine($@"Unsupported game assets! You wrote: ""{gameName}""");
                return null;
            }

            // 4. Повертаємо готові та перевірені дані
            return new ValidationResult
            {
                InputFolder = args[0],
                OutputFolder = args[1],
                GameName = gameName
            };
        }
    }
}