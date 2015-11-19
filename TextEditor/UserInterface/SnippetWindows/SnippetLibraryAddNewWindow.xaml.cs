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
    /// Interaction logic for SnippetLibrary_AddNewWindow
    /// </summary>
    public partial class SnippetLibraryAddNewWindow : Window
    {
        private SnippetLibrary snippetLibrary;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnippetLibraryAddNewWindow"/> class.
        /// </summary>
        /// <param name="snippetLibrary">SnippetLibrary to add new snippet.</param>
        public SnippetLibraryAddNewWindow(SnippetLibrary snippetLibrary)
        {
            this.snippetLibrary = snippetLibrary;
            this.InitializeComponent();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(this.snippetNameTextBox.Text) && !string.IsNullOrWhiteSpace(this.snippetNameTextBox.Text))
            {
                Snippet newSnippet = new Snippet(this.snippetNameTextBox.Text, new List<string>());
                this.snippetLibrary.Add(newSnippet);
                this.Close();
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
