using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TextEditor.Commands;
using TextEditor.FileManager;
using TextEditor.Utilities;

namespace TextEditor
{
    /// <summary>
    /// EventArgs for LibraryWordEnteredEvent.
    /// </summary>
    public class LibraryWordEnteredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LibraryWordEnteredEventArgs"/> class.
        /// </summary>
        /// <param name="names">Names form library which was entered.</param>
        /// <param name="charRect">Rectangle for character.</param>
        public LibraryWordEnteredEventArgs(List<string> names, Rect charRect)
        {
            this.Names = names;
            this.CharacterRect = charRect;
        }

        /// <summary>
        /// Gets word form library which was entered.
        /// </summary>
        public List<string> Names { get; private set; }

        /// <summary>
        /// Gets rectangle of index caret.
        /// </summary>
        public Rect CharacterRect { get; private set; }
    }

    /// <summary>
    /// Interaction logic for SourceTextBox.
    /// </summary>
    public partial class SourceTextBox : TextBox
    {
        private Brush fontBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));

        private TextEditorDocument document;
        private TextEditorCommandManager commandManager;
        private SnippetLibrary snippetLibrary;
        private MacroLibrary macroLibrary;
        private ISyntaxAnalyzer syntaxAnalyzer = new SwiftSyntaxAnalyzer();
        private int lastCarretIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceTextBox"/> class.
        /// </summary>
        public SourceTextBox()
        {
            this.AcceptsReturn = true;
            this.AcceptsTab = true;
            this.InitializeComponent();
            this.Text = string.Empty;
        }

        /// <summary>
        /// Event fires when user entered word from library.
        /// </summary>
        public event System.EventHandler<LibraryWordEnteredEventArgs> LibraryWordEnteredEvent;

        /// <summary>
        /// Gets or sets <see cref="TextEditorDocument"/>, which displayed by <see cref="SourceTextBox"/>.
        /// </summary>
        public TextEditorDocument Document
        {
            get
            {
                return this.document;
            }

            set
            {
                if (value == null)
                {
                    return;
                }

                this.document = value;
                this.commandManager = new TextEditorCommandManager(this.document);
                this.UpdateUi();
            }
        }

        /// <summary>
        /// Gets or sets SnippetLibrary.
        /// </summary>
        public SnippetLibrary SnippetLibrary
        {
            get { return this.snippetLibrary; }
            set { this.snippetLibrary = value; }
        }

        /// <summary>
        /// Gets or sets SnippetLibrary.
        /// </summary>
        public MacroLibrary MacroLibrary
        {
            get { return this.macroLibrary; }
            set { this.macroLibrary = value; }
        }

        /// <summary>
        /// Inserts snippet at caret index replacing it's name.
        /// </summary>
        /// <param name="snippetName">Name of snippet.</param>
        public void InsertSnippet(string snippetName)
        {
            if (snippetName == null)
            {
                return;
            }

            Snippet snippet = this.snippetLibrary.GetByName(snippetName);
            if (snippet == null)
            {
                return;
            }

            string word = this.document.GetWordByCaretIndex(this.CaretIndex);
            int index = this.CaretIndex - word.Length;

            this.lastCarretIndex = index + string.Join("\n", snippet.Content).Length;
            InsertSnippetCommand insertCommand = new InsertSnippetCommand(snippet, index);
            this.commandManager.AddCommand(insertCommand);
            this.commandManager.Run();
            this.UpdateUi();
        }

        /// <summary>
        /// Executes macro.
        /// </summary>
        /// <param name="macro">Macro to execute.</param>
        public void ExecuteMacro(Macro macro)
        {
            if (macro == null)
            {
                return;
            }

            this.lastCarretIndex = this.CaretIndex;
            macro.Run(this.document, this.CaretIndex);
            this.UpdateUi();
        }

        /// <summary>
        /// Handles KeyDown.
        /// </summary>
        /// <param name="e">Key event arguments.</param>
        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e == null)
            {
                return;
            } 
            else if (this.Document == null)
            {
                e.Handled = true;
                return;
            }

            char pressedChar = e.Key.GetChar();

            // CTRL + Z
            if (e.Key == Key.Z && Keyboard.Modifiers == ModifierKeys.Control)
            {
                this.lastCarretIndex = this.CaretIndex - this.document.CaretPositionInLineByIndex(this.CaretIndex);
                this.commandManager.Undo();
                this.UpdateUi();
                e.Handled = true;
            }

            // Backspace
            else if (e.Key == Key.Back)
            {
                RemoveRangeCommand removeCommand;
                if (this.SelectionLength > 0)
                {
                    this.lastCarretIndex = this.SelectionStart;
                    removeCommand = new RemoveRangeCommand(this.SelectionStart, this.SelectionLength);
                }
                else
                {
                    if (this.CaretIndex == 0)
                    {
                        return;
                    }

                    this.lastCarretIndex = this.CaretIndex - 1;
                    removeCommand = new RemoveRangeCommand(this.CaretIndex - 1, 1);
                }

                this.RunCommand(removeCommand);
                e.Handled = true;
            }

            // Tab
            else if (e.Key == Key.Tab)
            {
                this.lastCarretIndex = this.CaretIndex + 2;
                InsertStringCommand insertCommand = new InsertStringCommand("  ", this.CaretIndex);
                this.RunCommand(insertCommand);

                e.Handled = true;
            }

            // Enter
            else if (e.Key == Key.Return)
            {
                string paragraph = this.document.Lines[this.document.LineNumberByIndex(this.CaretIndex)];
                int spaceCount = 0;
                while (spaceCount < paragraph.Length && paragraph[spaceCount] == ' ')
                {
                    spaceCount++;
                }

                NewLineCommand newLineCommand = new NewLineCommand(spaceCount, this.CaretIndex);
                this.lastCarretIndex = this.CaretIndex + 1 + spaceCount;
                this.RunCommand(newLineCommand);
                e.Handled = true;
            }

            // Ctrl+V
            else if (e.Key == Key.V && Keyboard.Modifiers == ModifierKeys.Control)
            {
                string clipboardText = Clipboard.GetText();
                List<string> clipboardLines = clipboardText.Split('\n').Select(l => l.Replace("\r", "")).ToList();
                this.lastCarretIndex += clipboardText.Length;

                InsertLinesCommand command = new InsertLinesCommand(clipboardLines, this.CaretIndex);
                this.RunCommand(command);
            }

            // Some symbol 
            else if ((!char.IsControl(pressedChar) && !pressedChar.Equals(' ')) || e.Key == Key.Space)
            {
                this.lastCarretIndex = this.CaretIndex + 1;
                InsertStringCommand insertCommand = new InsertStringCommand(e.Key.GetChar().ToString(), this.CaretIndex);
                this.RunCommand(insertCommand);

                // Autocompletion analyzing
                string currentWord = this.document.GetWordByCaretIndex(this.CaretIndex);

                List<string> snippetsNames = SnippetLibrary.Names.Where(s => s.StartsWith(currentWord)).ToList();
                if (snippetsNames.Count > 0)
                {
                    this.OnLibraryWordEntered(
                        new LibraryWordEnteredEventArgs(snippetsNames, this.GetRectFromCharacterIndex(this.CaretIndex)));
                }

                e.Handled = true;
            }

            base.OnPreviewKeyDown(e);
        }

        /// <summary>
        /// Called when <see cref="SourceTextBox"/> is rendered.
        /// </summary>
        /// <param name="drawingContext"><see cref="DrawingContext"/> on which content is rendered.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (drawingContext == null)
            {
                return;
            }

            drawingContext.PushClip(new RectangleGeometry(new Rect(0, 0, this.ActualWidth, this.ActualHeight)));
            drawingContext.DrawRectangle(
                new SolidColorBrush(Color.FromRgb(44, 47, 60)), 
                new Pen(), 
                new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            if (this.document == null || this.document.Text.Length == 0)
            {
                return;
            }

            FormattedText ft = new FormattedText(
                this.document.Text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(this.FontFamily.Source),
                this.FontSize, 
                this.fontBrush);
            
            if (this.syntaxAnalyzer != null)
            {
                foreach (Token token in this.syntaxAnalyzer.Tokens)
                {
                    ft.SetForegroundBrush(new SolidColorBrush(Colors.DarkSeaGreen), token.CaretIndex, token.Length);
                }
            }

            var topMargin = 2.0 + this.BorderThickness.Top;
            ft.MaxTextWidth = this.ActualWidth - 6;
            ScrollViewer scrollview = this.FindVisualChild<ScrollViewer>();
            Visibility verticalVisibility = scrollview.ComputedVerticalScrollBarVisibility;
            if (verticalVisibility == Visibility.Visible)
            {
                ft.MaxTextWidth -= SystemParameters.VerticalScrollBarWidth;
            }

            ft.Trimming = TextTrimming.None;
            double leftBorder = GetRectFromCharacterIndex(0).Left;
            double leftTextBorder = double.PositiveInfinity;
            if (!double.IsInfinity(leftBorder))
            {
                leftTextBorder = leftBorder;
            }
            else
            {
                leftTextBorder = 3;
            }

            drawingContext.DrawText(ft, new Point(leftTextBorder - this.HorizontalOffset, topMargin - this.VerticalOffset));
        }

        /// <summary>
        /// Fires a LibraryWordEntered event.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected virtual void OnLibraryWordEntered(LibraryWordEnteredEventArgs e)
        {
            if (this.LibraryWordEnteredEvent != null)
            {
                this.LibraryWordEnteredEvent(this, e);
            }
        }

        /// <summary>
        /// Check text changed
        /// </summary>
        /// <param name="e">TextChanged event arguments.</param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);

            if (this.syntaxAnalyzer != null)
            {
                this.syntaxAnalyzer.Parse(this.Document);
            }

            this.UpdateUi();
        }

        /// <summary>
        /// Executes the command and updates UI.
        /// </summary>
        /// <param name="command">Command to run.</param>
        private void RunCommand(Commands.ICommand command)
        {
            this.macroLibrary.AddCommand(command);
            this.commandManager.AddCommand(command);
            this.commandManager.Run();
            this.UpdateUi();
        }

        private void TextBox_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            this.InvalidateVisual();
        }

        private void UpdateUi()
        {
            if (this.document != null)
            {
                this.Text = this.document.Text;
                this.CaretIndex = this.lastCarretIndex;
            }

            this.InvalidateVisual();
        }
    }
}
