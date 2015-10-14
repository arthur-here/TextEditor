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
        private int lastCarretIndex = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceTextBox"/> class.
        /// </summary>
        public SourceTextBox()
        {
            this.AcceptsReturn = true;

            this.InitializeComponent();
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
            if (!char.IsControl(pressedChar) && !pressedChar.Equals(' '))
            {
                this.lastCarretIndex = this.CaretIndex + 1;
                InsertStringCommand insertCommand = new InsertStringCommand(e.Key.GetChar().ToString(), this.document, this.CaretIndex);
                this.commandManager.AddCommand(insertCommand);
                this.commandManager.Run();
                this.UpdateUi();
                e.Handled = true;
            }
            else if (e.Key == System.Windows.Input.Key.Return)
            {
                int index = this.CaretIndex;
                int lastLine = this.document.Text.LastIndexOf(Environment.NewLine, index, StringComparison.CurrentCulture);
                int spaces = 0;

                if (lastLine != -1)
                {
                    string line = this.document.Text.Substring(lastLine, this.document.Text.Length - lastLine);

                    int startLine = line.IndexOf(Environment.NewLine, StringComparison.CurrentCulture);

                    if (startLine != -1)
                    {
                        line = line.Substring(startLine).TrimStart('\r', '\n');
                    }

                    foreach (char c in line)
                    {
                        if (c == ' ')
                        {
                            spaces++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                this.Text = this.Text.Insert(index, Environment.NewLine + new string(' ', spaces));
                this.CaretIndex = index + Environment.NewLine.Length + spaces;

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
