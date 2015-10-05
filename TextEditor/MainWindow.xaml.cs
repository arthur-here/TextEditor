using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using TextEditor.FileManager;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextEditorFileManager fileManager = new FileManager.TextEditorFileManager();

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class..
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private FlowDocument Document
        {
            set
            {
                this.codeArea.Document = value;
                this.codeArea.Document.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            }
        }

        private void NewFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Document = this.fileManager.OpenFile();
        }

        private void SaveFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();

            if (sfd.ShowDialog() == true)
            {
                string filename = sfd.FileName;
                Console.WriteLine(filename);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TitleLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
