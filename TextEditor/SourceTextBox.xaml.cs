using System;
using System.Collections.Generic;
using System.Linq;
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
using TextEditor.FileManager;

namespace TextEditor
{
    /// <summary>
    /// Interaction logic for SourceTextBox.
    /// </summary>
    public partial class SourceTextBox : TextBox
    {
        private Brush fontBrush = new SolidColorBrush(Color.FromRgb(230, 230, 230));

        private TextEditorDocument document;

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
                this.Text = string.Join("\n", this.document.Lines);
                this.InvalidateVisual();
            }
        }
 
        /// <summary>
        /// Handles text changing.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            base.OnTextChanged(e);
            this.InvalidateVisual();
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

            if (e.Key == System.Windows.Input.Key.Return)
            {
                int index = this.CaretIndex;
                int lastLine = this.Text.LastIndexOf(Environment.NewLine, index, StringComparison.CurrentCulture);
                int spaces = 0;

                if (lastLine != -1)
                {
                    string line = this.Text.Substring(lastLine, this.Text.Length - lastLine);

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

            if (this.Text == null)
            {
                return;
            }

            FormattedText ft = new FormattedText(
                this.Text,
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(this.FontFamily.Source),
                this.FontSize, 
                this.fontBrush);
            
            var topMargin = 2.0 + this.BorderThickness.Top;

            ft.MaxTextWidth = this.ActualWidth - 10;
            ScrollViewer scrollview = this.FindVisualChild<ScrollViewer>(this);
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
                Console.WriteLine(leftTextBorder);
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

        private T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = this.FindVisualChild<T>(child);
                    if (childItem != null)
                    {
                        return childItem;
                    }
                }
            }

            return null;
        }
    }
}
