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
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class..
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
        }

        private void DisplayText(string[] text, bool clearWindow = true)
        {
            if (clearWindow)
            {
                this.codeListBox.Items.Clear();
                this.LineNumberListBox.Items.Clear();
            }

            int index = this.LineNumberListBox.Items.Count + 1;

            foreach (string line in text)
            {
                TextBlock block = new TextBlock();
                block.Text = line;
                int charactersPerLine = (int)(this.codeListBox.RenderSize.Width / 7);
                if (line.Length > charactersPerLine)
                {
                    block.Text = block.Text.Insert(charactersPerLine, "\n");
                }

                this.codeListBox.Items.Add(block);
                this.LineNumberListBox.Items.Add(index);
                index++;
            }
        }

        private void NewFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            if (ofd.ShowDialog() == false)
            {
                return;
            }

            FileReaderStrategy fileReader;
            string filename = ofd.FileName;
            using (StreamReader streamReader = new StreamReader(filename))
            {
                streamReader.Peek();
                if (streamReader.CurrentEncoding == UTF8Encoding.Default)
                {
                    fileReader = new UTF8FileReader(filename);
                }
                else if (streamReader.CurrentEncoding == ASCIIEncoding.Default)
                {
                    fileReader = new ASCIIFileReader(filename);
                }
                else
                {
                    fileReader = new DefaultFileReader(filename);
                }
            }

            this.DisplayText(fileReader.Read());
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
