using System;
using System.Linq;
using System.Reflection;

namespace TTG_Tools.CLI.Core.Commands.Impl
{
    [Command("--help", "Print help information.")]
    public class HelpCommand : ICommand
    {
        public void Run(string[] args)
        {
            var items = CliRouter.Instance.Commands.Values
                .Select(cmd => cmd.GetType().GetCustomAttribute<CommandAttribute>())
                .Where(attr => attr != null)
                .OrderBy(attr => attr.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (items.Count == 0) return;

            int maxNameLength = items.Max(attr => attr.Name.Length);

            foreach (var cmd in items)
            {
                Console.WriteLine($@"{cmd.Name} {cmd.Description}");
            }
        }
    }
}