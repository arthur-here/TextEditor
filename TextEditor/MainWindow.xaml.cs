using System;
using System.Windows;
using System.Windows.Input;

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

        private void NewFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                string filename = ofd.FileName;
                Console.WriteLine(filename);
            }
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
