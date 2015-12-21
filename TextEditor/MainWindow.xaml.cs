using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using TextEditor.FileManager;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.
    /// </summary>
    public partial class MainWindow : Window
    {
        private TextEditorFileManager fileManager = new TextEditorFileManager();
        private SnippetLibrary snippetLibrary = new SnippetLibrary();
        private MacroLibrary macroLibrary = new MacroLibrary();

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
            this.codeArea.SnippetLibrary = this.snippetLibrary;
            this.codeArea.MacroLibrary = this.macroLibrary;
            this.codeArea.LibraryWordEnteredEvent += this.CodeArea_LibraryWordEnteredEvent;
            this.codeArea.UpdateLineNumbersEvent += this.CodeArea_UpdateLineNumbersEvent;
            this.tipListBox.SelectionChanged += this.TipListBox_SelectionChanged;
            this.SetupUi();
        }

        private ITextEditorDocument Document
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
                else if (e.Key == Key.Enter || e.Key == Key.Tab)
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
            this.tipListBox.SelectedIndex = 0;
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
            ITextEditorDocument newDocument = this.fileManager.New();
            if (newDocument != null)
            {
                this.Document = newDocument;
            }
        }

        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ITextEditorDocument newDocument = this.fileManager.OpenFile();
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
            numberLabel.FontFamily = codeArea.FontFamily;
            numberLabel.FontSize = codeArea.FontSize + 0.02;
            this.tipListBox.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            this.tipListBox.SetValue(ScrollViewer.VerticalScrollBarVisibilityProperty, ScrollBarVisibility.Disabled);
            this.RootGrid.Children.Add(this.tipListBox);

            Style s = new Style(typeof(MenuItem));
            Setter setter = new Setter { Property = MenuItem.BackgroundProperty, Value = new SolidColorBrush(Color.FromRgb(16, 18, 23)) };
            s.Setters.Add(setter);
            this.MacrosMenuItem.ItemContainerStyle = s;
            this.MacrosMenuItem.ItemsSource = this.macroLibrary.Library.Select(m => m.Name);
        }

        private void SnippetLibraryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SnippetLibraryWindow slw = new SnippetLibraryWindow(this.snippetLibrary);
            slw.Show();
        }

        private void RecordMacroMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!this.macroLibrary.IsRecording)
            {
                this.macroLibrary.StartRecording();
                this.RecordMacroMenuItem.Header = new TextBlock(new Run("Stop Recording"));
            }
            else
            {
                this.macroLibrary.StopRecording();
                this.RecordMacroMenuItem.Header = new TextBlock(new Run("Start Recording"));
                this.MacrosMenuItem.ItemsSource = this.macroLibrary.Library.Select(m => m.Name);
                this.MacrosMenuItem.Click += this.MacroMenuItem_Click;
            }
        }

        private void MacroMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string itemHeader = (e.OriginalSource as MenuItem).Header.ToString();
            Macro selectedMacro = this.macroLibrary.Library.Where(m => m.Name == itemHeader).FirstOrDefault();
            if (selectedMacro != null)
            {
                this.codeArea.ExecuteMacro(selectedMacro);
            }
        }

        private void CodeArea_UpdateLineNumbersEvent(object sender, UpdateLineNumbersEventArgs e)
        {
            this.UpdateNumberLabel(e.FirstLineNumber, e.LastLineNumber);
        }

        private void UpdateNumberLabel(int firstLine, int lastLine)
        {
            Point pos = new Point(0, 0);

            pos.X = this.codeArea.ActualWidth;
            pos.Y = this.codeArea.ActualHeight;
            int lastIndex = codeArea.GetCharacterIndexFromPoint(pos, true);

            pos = this.codeArea.GetRectFromCharacterIndex(lastIndex).Location;

            numberLabel.Content = string.Empty;
            for (int i = firstLine; i <= lastLine + 1; i++)
            {
                numberLabel.Content += i + 1 + "\n";
            }
        }
    }
}
