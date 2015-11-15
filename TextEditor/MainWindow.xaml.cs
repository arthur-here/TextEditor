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
        private TextEditorFileManager fileManager = new TextEditorFileManager();
        private ListBox tipListBox = new ListBox()
        {
            Visibility = Visibility.Hidden,
            SelectionMode = SelectionMode.Single
        };

        private bool isAutocompleteListShown = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            this.codeArea.SnippetLibrary = new SnippetLibrary();
            this.codeArea.LibraryWordEnteredEvent += this.CodeArea_LibraryWordEnteredEvent;
            this.tipListBox.SelectionChanged += this.TipListBox_SelectionChanged;
            this.SetupUi();
        }

        private TextEditorDocument Document
        {
            get
            {
                return this.codeArea.Document;
            }

            set
            {
                this.codeArea.Document = value;
                this.TitleLabel.Text = "Text Editor - " + value.Title;
            }
        }

        /// <summary>
        /// Handles PreviewKeyDown event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            if (this.isAutocompleteListShown)
            {
                if (e.Key == Key.Up)
                {
                    if (this.tipListBox.SelectedIndex > 0)
                    {
                        this.tipListBox.SelectedIndex--;
                    }

                    e.Handled = true;
                }
                else if (e.Key == Key.Down)
                {
                    if (this.tipListBox.SelectedIndex < this.tipListBox.Items.Count - 1)
                    {
                        this.tipListBox.SelectedIndex++;
                    }

                    e.Handled = true;
                }
                else if (e.Key == Key.Enter)
                {
                    this.codeArea.InsertSnippet(this.tipListBox.SelectedItem as string);
                    this.isAutocompleteListShown = false;
                    this.tipListBox.Visibility = Visibility.Hidden;

                    e.Handled = true;
                }
                else
                {
                    this.isAutocompleteListShown = false;
                    this.tipListBox.Visibility = Visibility.Hidden;
                }
            }

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// Handles PreviewMouseDown event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (this.isAutocompleteListShown)
            {
                this.isAutocompleteListShown = false;
                this.tipListBox.Visibility = Visibility.Hidden;
            }

            base.OnPreviewMouseDown(e);
        }

        private void CodeArea_LibraryWordEnteredEvent(object sender, LibraryWordEnteredEventArgs e)
        {
            double rightMargin = this.codeArea.ActualWidth - 200 - e.CharacterRect.Left;
            double bottomMargin = this.codeArea.ActualHeight - 100 - e.CharacterRect.Top;
            this.tipListBox.Margin = new Thickness(this.codeArea.Margin.Left + e.CharacterRect.Left, 
                this.codeArea.Margin.Top + e.CharacterRect.Top, 
                rightMargin, 
                bottomMargin);
            this.tipListBox.ItemsSource = e.Names;
            this.tipListBox.Visibility = Visibility.Visible;
            this.isAutocompleteListShown = true;
        }

        private void TipListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0)
            {
                return;
            }

            var newSelectedItem = e.AddedItems[0];
            (sender as ListBox).ScrollIntoView(newSelectedItem);
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

        private void SetupUi()
        {
            this.tipListBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            this.tipListBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            this.RootGrid.Children.Add(this.tipListBox);
        }
    }
}
