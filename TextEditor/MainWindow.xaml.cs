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
        private TextEditorDocument document;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class..
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private TextEditorDocument Document
        {
            get
            {
                return this.document == null ? this.codeArea.Document as TextEditorDocument : this.document;
            }

            set
            {
                this.codeArea.Document = value;
                this.document = value;
            }
        }

        private void NewFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Document = this.fileManager.New();
        }

        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Document = this.fileManager.OpenFile();
        }

        private void SaveFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.fileManager.SaveDocument(this.Document);
        }

        private void SaveAsFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.fileManager.SaveAsDocument(this.Document);
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
