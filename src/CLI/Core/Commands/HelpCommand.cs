using System;
using System.Reflection;

namespace TTG_Tools.CLI.Core.Commands
{
    [Command("--help", "Print help information.")]
    public class HelpCommand : ICommand
    {
        public void Run(string[] args)
        {
            var commands = CliRouter.Instance.Commands;

            foreach (var pair in commands)
            {
                var command = pair.Value;
                var type = command.GetType();
                var cmd = type.GetCustomAttribute<CommandAttribute>();
                if (cmd == null) continue;
                
                Console.WriteLine($@"{cmd.Name}: {cmd.Description}");
            }
        }
    }
}