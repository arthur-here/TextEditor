using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MacroNameWindow.
    /// </summary>
    public partial class MacroNameWindow : Window
    {
        private MacroLibrary macroLibrary;
        private Macro macro;

        /// <summary>
        /// Initializes a new instance of the <see cref="MacroNameWindow"/> class.
        /// </summary>
        /// <param name="macroLibrary">Library of macros.</param>
        /// <param name="macro">Recorded macro.</param>
        public MacroNameWindow(MacroLibrary macroLibrary, Macro macro)
        {
            this.macroLibrary = macroLibrary;
            this.macro = macro;
            this.InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.snippetNameTextBox.Text) && !string.IsNullOrWhiteSpace(this.snippetNameTextBox.Text))
            {
                this.macro.Name = this.snippetNameTextBox.Text;
                this.macroLibrary.Library.Add(this.macro);
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
