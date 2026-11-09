using System.Windows.Forms;

namespace TTG_Tools.CLI.Core
{
    public class UiAutomation
    {
        public static void Click(Button button)
        {
            button.PerformClick();
        }

        public static void Select(ComboBox comboBox, int index)
        {
            comboBox.SelectedIndex = index;
        }

        public static void SetText(TextBox textBox, string value)
        {
            textBox.Text = value;
        }

        public static void Check(CheckBox checkBox, bool value)
        {
            checkBox.Checked = value;
        }
    }
}