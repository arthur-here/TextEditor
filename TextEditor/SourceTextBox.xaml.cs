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
    /// Interaction logic for SourceTextBox.
    /// </summary>
    public partial class SourceTextBox : TextBox
    {
        private Brush fontBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));

        private TextEditorDocument document;
        private TextEditorCommandManager commandManager = new TextEditorCommandManager();
        private SnippetLibrary snippetLibrary = new SnippetLibrary();
        private int lastCarretIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceTextBox"/> class.
        /// </summary>
        public SourceTextBox()
        {
            this.AcceptsReturn = true;
            this.AcceptsTab = true;
            this.InitializeComponent();
            List<string> s1Content = new List<string>()
            {
                "lorem ipsum dot sit amet,",
                "consectetur adipisicing elit"
            };
            Snippet s1 = new Snippet("lorem", s1Content);
            this.snippetLibrary.Add(s1);
        }

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
                this.UpdateUi();
            }
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
                if (this.SelectionLength > 0)
                {
                    this.lastCarretIndex = this.SelectionStart;
                    RemoveStringCommand removeSelectionCommand = new RemoveStringCommand(this.document, 
                        this.SelectionStart, 
                        this.SelectionLength);
                    this.commandManager.AddCommand(removeSelectionCommand);
                }
                else
                {
                    if (this.CaretIndex == 0)
                    {
                        return;
                    }

                    this.lastCarretIndex = this.CaretIndex - 1;
                    RemoveStringCommand removeCommand = new RemoveStringCommand(this.document, this.CaretIndex - 1, 1);
                    this.commandManager.AddCommand(removeCommand);
                }

                this.commandManager.Run();
                this.UpdateUi();
                e.Handled = true;
            }

            // Tab
            else if (e.Key == Key.Tab)
            {
                string line = this.document.Lines[this.document.LineNumberByIndex(this.CaretIndex)];
                int caretPosition = this.document.CaretPositionInLineByIndex(this.CaretIndex);
                if (caretPosition < line.Length - 1)
                {
                    line = line.Remove(caretPosition);
                }
                
                int indexOfLastSpace = line.LastIndexOf(' ');
                if (indexOfLastSpace != -1 && line.Length > indexOfLastSpace + 1)
                {
                    line = line.Substring(indexOfLastSpace + 1);
                }

                Snippet snippet = this.snippetLibrary.GetByName(line);
                if (snippet != null)
                {
                    this.lastCarretIndex = this.CaretIndex + snippet.Name.Length;
                    InsertStringCommand insertCommand = new InsertStringCommand(snippet.Name, this.document, this.CaretIndex);
                    this.commandManager.AddCommand(insertCommand);
                    this.commandManager.Run();
                    this.UpdateUi();
                }

                e.Handled = true;
            }

            // Enter
            else if (e.Key == Key.Return)
            {
                NewLineCommand newLineCommand = new NewLineCommand(this.document, this.CaretIndex);
                this.lastCarretIndex = this.CaretIndex + 1;
                this.commandManager.AddCommand(newLineCommand);
                this.commandManager.Run();
                this.UpdateUi();
                e.Handled = true;
            }

            // Some symbol 
            else if ((!char.IsControl(pressedChar) && !pressedChar.Equals(' ')) || e.Key == Key.Space)
            {
                this.lastCarretIndex = this.CaretIndex + 1;
                InsertStringCommand insertCommand = new InsertStringCommand(e.Key.GetChar().ToString(), this.document, this.CaretIndex);
                this.commandManager.AddCommand(insertCommand);
                this.commandManager.Run();
                this.UpdateUi();
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
