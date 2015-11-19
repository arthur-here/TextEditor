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
    /// Interaction logic for SnippetLibraryWindow
    /// </summary>
    public partial class SnippetLibraryWindow : Window
    {
        private SnippetLibrary snippetLibrary;

        /// <summary>
        /// Initializes a new instance of the <see cref="SnippetLibraryWindow"/> class.
        /// </summary>
        /// <param name="snippetLibrary">The library.</param>
        public SnippetLibraryWindow(SnippetLibrary snippetLibrary)
        {
            this.snippetLibrary = snippetLibrary;
            this.InitializeComponent();
            this.snippetsListBox.ItemsSource = this.snippetLibrary.Names;
        }

        private Snippet SelectedSnippet
        {
            get
            {
                string selectedSnippetName = this.snippetsListBox.SelectedItem as string;
                return this.snippetLibrary.GetByName(selectedSnippetName);
            }
        }

        private void snippetsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.SelectedSnippet != null)
            {
                this.snippetContentTextBox.Text = string.Join("\n", this.SelectedSnippet.Content);
            }
        }

        private void SaveChanges()
        {
            List<string> currentContent = this.snippetContentTextBox.Text.Split('\n').ToList();
            if (this.SelectedSnippet != null)
            {
                this.SelectedSnippet.Content = currentContent;
            }

            this.snippetLibrary.Save();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            this.SaveChanges();
        }

        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            SnippetLibraryAddNewWindow slanw = new SnippetLibraryAddNewWindow(this.snippetLibrary);
            slanw.ShowDialog();
            this.UpdateUi();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedSnippet != null)
            {
                this.snippetLibrary.Remove(this.SelectedSnippet.Name);
                this.snippetLibrary.Save();
                this.UpdateUi();
            }
        }

        private void UpdateUi()
        {
            this.snippetsListBox.ItemsSource = null;
            this.snippetsListBox.ItemsSource = this.snippetLibrary.Names;
            this.snippetContentTextBox.Text = string.Empty;
        }
    }
}
