using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TextEditor
{
    /// <summary>
    /// Represents a TextEditor Document.
    /// </summary>
    public class TextEditorDocument: ITextEditorDocument
    {
        private string fileName;
        private List<string> lines = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditorDocument"/> class.
        /// </summary>
        /// <param name="fileName">Path to document.</param>
        public TextEditorDocument(string fileName)
            : base()
        {
            this.FileName = fileName;
        }

        /// <summary>
        /// Gets or sets path to document.
        /// </summary>
        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        /// <summary>
        /// Gets titile of current openet document.
        /// </summary>
        public string Title
        {
            get
            {
                int lastSlashIndex = this.FileName.LastIndexOf('\\');
                if (lastSlashIndex == -1)
                {
                    return this.FileName;
                }
                else
                {
                    return this.FileName.Substring(lastSlashIndex + 1);
                }
            }
        }

        /// <summary>
        /// Gets array of document's lines.
        /// </summary>
        public List<string> Lines
        {
            get
            {
                return lines;
            }
        }

        /// <summary>
        /// Gets all lines, joined by \n.
        /// </summary>
        public string Text
        {
            get
            {
                return string.Join("\n", this.Lines);
            }
        }

        /// <summary>
        /// Gets or sets offset to show lines.
        /// </summary>
        public int LinesOffset
        {
            get { return 0; }
            set { ; }
        }

        /// <summary>
        /// Gets or sets amount of lines to render.
        /// </summary>
        public int AmountOfLinesToShow
        {
            get { return this.lines.Count; }
            set { ; }
        }

        /// <summary>
        /// Adds new line to the end of this document.
        /// </summary>
        /// <param name="line">Line to add.</param>
        public void AddLine(string line)
        {
            this.lines.Add(line);
        }

        /// <summary>
        /// Insert new line at specified index.
        /// </summary>
        /// <param name="index">Index to insert.</param>
        /// <param name="line">Line to insert.</param>
        public void InsertLineAtIndex(int index, string line)
        {
            Debug.Assert(index >= 0 && index <= this.lines.Count);
            this.lines.Insert(index, line);
        }

        /// <summary>
        /// Insert new lines at specified index.
        /// </summary>
        /// <param name="index">Index to insert.</param>
        /// <param name="newLines">Lines to insert.</param>
        public void InsertLinesAtIndex(int index, List<string> newLines)
        {
            Debug.Assert(index >= 0 && index <= this.lines.Count);
            this.lines.InsertRange(index, newLines);
        }

        /// <summary>
        /// Replace line at index with provided line.
        /// </summary>
        /// <param name="index">Index of line to replace.</param>
        /// <param name="newLine">New line.</param>
        public void ChangeLineAtIndex(int index, string newLine)
        {
            Debug.Assert(index >= 0 && index < this.lines.Count);
            this.lines[index] = newLine;
        }

        /// <summary>
        /// Remove lines at specified index.
        /// </summary>
        /// <param name="index">Start index.</param>
        /// <param name="count">Number of lines to remove.</param>
        public void RemoveLines(int index, int count)
        {
            Debug.Assert(index >= 0 && index + count - 1 < this.lines.Count);
            this.lines.RemoveRange(index, count);
        }

        /// <summary>
        /// Remove one line at specified index.
        /// </summary>
        /// <param name="index">Index of line to remove.</param>
        public void RemoveLineAtIndex(int index)
        {
            Debug.Assert(index >= 0 && index < this.lines.Count);
            this.lines.RemoveAt(index);
        }

        /// <summary>
        /// Finds number of line in document by carret index.
        /// </summary>
        /// <param name="caretIndex">Caret index in document.</param>
        /// <returns>Number of line in document.</returns>
        public int LineNumberByIndex(int caretIndex)
        {
            if (caretIndex < 0 || caretIndex > this.Text.Length)
            {
                throw new ArgumentException("Caret index should be in document range.");
            }

            int line = 0;
            while (line < this.Lines.Count && caretIndex - this.Lines[line].Length - 1 >= 0)
            {
                caretIndex -= this.Lines[line].Length+1;
                line++;
            }

            if (line >= this.Lines.Count)
            {
                line = this.Lines.Count - 1;
            }

            return line;
        }

        /// <summary>
        /// Calculates carret position in line by carret index.
        /// </summary>
        /// <param name="caretIndex">Caret index in document.</param>
        /// <returns>Carret position in line.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "InLine")]
        public int CaretPositionInLineByIndex(int caretIndex)
        {
            if (caretIndex < 0 || caretIndex > this.Text.Length)
            {
                throw new ArgumentException("Caret index should in document range.");
            }

            int line = 0;
            while (line < this.Lines.Count && caretIndex - this.Lines[line].Length - 1 >= 0)
            {
                caretIndex -= this.Lines[line].Length + 1;
                line++;
            }

            return caretIndex;
        }

        /// <summary>
        /// Calculates carret index by provided line and position.
        /// </summary>
        /// <param name="line">Line index in document.</param>
        /// <param name="position">Position in line.</param>
        /// <returns>Caret index.</returns>
        public int CaretIndexByPosition(int line, int position)
        {
            int index = 0;
            for (int i = 0; i < line; i++)
            {
                index += this.Lines[i].Length + 1;
            }

            index += position;
            return index;
        }

        /// <summary>
        /// Finds word which preceds caret index.
        /// </summary>
        /// <param name="caretIndex">Index to search.</param>
        /// <returns>Word which preceds caret index.</returns>
        public string GetWordByCaretIndex(int caretIndex)
        {
            string line = this.Lines[this.LineNumberByIndex(caretIndex)];
            int caretPosition = this.CaretPositionInLineByIndex(caretIndex);
            if (caretPosition < line.Length - 1)
            {
                line = line.Remove(caretPosition);
            }

            int indexOfLastSpace = line.LastIndexOf(' ');
            if (indexOfLastSpace != -1 && line.Length > indexOfLastSpace + 1)
            {
                line = line.Substring(indexOfLastSpace + 1);
            }

            return line;
        }
    }
}
