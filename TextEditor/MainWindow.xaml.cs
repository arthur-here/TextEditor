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
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
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
                this.TitleLabel.Text = "Text Editor - " + value.Title;
            }
        }

        private void NewFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TextEditorDocument newDocument = this.fileManager.New();
            if (newDocument != null)
            {
                this.Document = newDocument;
            }
        }

        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TextEditorDocument newDocument = this.fileManager.OpenFile();
            if (newDocument != null)
            {
                this.Document = newDocument;
            }
        }

        private void SaveFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.Document != null)
            {
                this.fileManager.SaveDocument(this.Document);
            }
        }

        private void SaveAsFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (this.Document != null)
            {
                this.fileManager.SaveAsDocument(this.Document);
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

        private void EncodingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Encoding encoding;
            MenuItem senderMenuItem = sender as MenuItem;
            switch (senderMenuItem.Header.ToString())
            {
                case "Auto":
                    encoding = Encoding.Default;
                    break;
                case "UTF8":
                    encoding = Encoding.UTF8;
                    break;
                case "ASCII":
                    encoding = Encoding.ASCII;
                    break;
                case "Unicode":
                    encoding = Encoding.Unicode;
                    break;
                default:
                    throw new ArgumentException("Unknown encoding");
            }

            this.Document = this.fileManager.OpenFileUsingEncoding(this.Document.FileName, encoding);
        }
    }
}
