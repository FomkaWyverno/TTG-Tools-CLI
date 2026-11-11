using System;
using TTG_Tools.CLI.Core.Headless;

namespace TTG_Tools.CLI.Core.Commands.Impl
{
    [Command("--edit-settings", @"Set program settings via arguments. Usage: --set-settings [--format] [--importNames] [--sortSame] [--exportId] [--ignoreEmpty]
                    Example: --set-settings --importNames=True --sortSame=True
                    --format: Text asset type: tsv, new, telltale, plain or other text switch to plain format.
                    --importNames: Import actor names to text assets
                    --sortSame: Sort by identical strings
                    --exportId: Export real ID
                    --ignoreEmpty: Ignore empty strings")]
    public class EditSettingsCommand : ICommand
    {
        public void Run(string[] args)
        {
            var menu = MainMenuHeadless.Instance;
            var settings = menu.GetSettings();

            bool isUpdated = false;
            
            foreach (var arg in args)
            {
                if (arg.StartsWith("--format="))
                {
                    string format = arg.Substring("--format=".Length);
                    
                    settings.tsvFormat = false;
                    settings.newTxtFormat = false;
                    settings.telltaleExplorerFormat = false;

                    switch (format.ToLowerInvariant())
                    {
                        case "tsv": settings.tsvFormat = true; break;
                        case "new": settings.newTxtFormat = true; break;
                        case "telltale": settings.telltaleExplorerFormat = true; break;
                    }
                    isUpdated = true;
                }
                else if (arg.StartsWith("--import-names="))
                {
                    if (bool.TryParse(arg.Substring("--import-names=".Length), out bool val))
                    {
                        settings.importingOfName = val;
                        isUpdated = true;
                    }
                }
                else if (arg.StartsWith("--sort-same="))
                {
                    if (bool.TryParse(arg.Substring("--sort-same=".Length), out bool val))
                    {
                        settings.sortSameString = val;
                        isUpdated = true;
                    }
                }
                else if (arg.StartsWith("--export-id="))
                {
                    if (bool.TryParse(arg.Substring("--export-id=".Length), out bool val))
                    {
                        settings.exportRealID = val;
                        isUpdated = true;
                    }
                }
                else if (arg.StartsWith("--ignore-empty="))
                {
                    if (bool.TryParse(arg.Substring("--ignore-empty=".Length), out bool val))
                    {
                        settings.ignoreEmptyStrings = val;
                        isUpdated = true;
                    }
                }
            }

            if (isUpdated)
            {
                menu.SaveSettings();
                Console.WriteLine(@"Settings updated.");
            }
            else
            {
                Console.WriteLine(@"Don't received any valid arguments for update settings.");
                Console.WriteLine(@"Example: --set-settings --format=tsv --import-names=false");
            }
        }
    }
}