using Spectre.Console;
using TTG_Tools.CLI.Core.Headless;

namespace TTG_Tools.CLI.Core.Commands.Impl
{
    [Command("--show-settings", "Show settings program.")]
    public class ShowSettingsCommand : ICommand
    {
        public void Run(string[] args)
        {
            var menu = MainMenuHeadless.Instance;
            var settings = menu.GetSettings();
            
            var table = new Table();
            table.Border(TableBorder.Ascii);

            table.AddColumns("Param", "Value");
            table.AddRow("Configuration folder (config file and profiles)", Settings.ConfigPath);
            table.AddEmptyRow();
            table.AddRow("Text file format", GetTextFormatName(settings));
            table.AddRow("Import actor names", settings.importingOfName.ToString());
            table.AddRow("Sort identical strings", settings.sortSameString.ToString());
            table.AddRow("Export real ID", settings.exportRealID.ToString());
            table.AddRow("Ignore empty strings", settings.ignoreEmptyStrings.ToString());
            
            AnsiConsole.Write(table);
        }
        
        private string GetTextFormatName(Settings settings)
        {
            if (settings.tsvFormat) return "tsv";
            if (settings.newTxtFormat) return "New txt format (langid / actor / speech)";
            if (settings.telltaleExplorerFormat) return Markup.Escape("Telltale Explorer Style ([id] / Category / Speech)");
            return "Plain text (.txt)";
        }
    }
}