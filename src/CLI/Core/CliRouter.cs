using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TTG_Tools.CLI.Core.Commands;

namespace TTG_Tools.CLI.Core
{
    public class CliRouter
    {
        // 1. Статичний екземпляр з ледачою ініціалізацією (Thread-safe за замовчуванням)
        private static readonly Lazy<CliRouter> _instance = new Lazy<CliRouter>(() => new CliRouter());

        // 2. Публічна точка доступу до єдиного екземпляра
        public static CliRouter Instance => _instance.Value;
        
        
        private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>(StringComparer.OrdinalIgnoreCase);
        public IReadOnlyDictionary<string, ICommand> Commands => _commands;
        
        public CliRouter()
        {
            DiscoverCommands();
        }
        
        /// <summary>
        /// Автоматично сканує збірку (Assembly) на наявність класів з атрибутом CommandAttribute
        /// </summary>
        private void DiscoverCommands()
        {
            var commandType = typeof(ICommand);
            var assembly = Assembly.GetExecutingAssembly();

            var commandClasses = assembly.GetTypes()
                .Where(t => commandType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

            foreach (var type in commandClasses)
            {
                var attribute = type.GetCustomAttribute<CommandAttribute>();
                if (attribute != null)
                {
                    // Створюємо екземпляр команди через конструктор за замовчуванням
                    if (Activator.CreateInstance(type) is ICommand instance)
                    {
                        _commands[attribute.Name] = instance;
                    }
                }
            }
        }
        
        public void Route(string[] args)
        {
            if (args.Length == 0 || !Commands.ContainsKey(args[0]))
            {
                Console.WriteLine($@"Unknown command {args[0]}");
                var helpCommand = Commands["--help"];
                helpCommand.Run(args);
                return;
            }

            var cmd = Commands[args[0]];
            cmd.Run(args.Skip(1).ToArray());
        }
    }
}