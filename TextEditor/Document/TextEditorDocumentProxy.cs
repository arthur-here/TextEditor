using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    /// <summary>
    /// Proxy for ITextEditorDocument. Uses chunks of text instead of full text
    /// </summary>
    public class TextEditorDocumentProxy : ITextEditorDocument
    {
        private TextEditorDocument document;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditorDocumentProxy"/> class.
        /// </summary>
        /// <param name="document">Real document.</param>
        public TextEditorDocumentProxy(TextEditorDocument document)
        {
            this.document = document;
        }

        /// <summary>
        /// Gets or sets path to document.
        /// </summary>
        public string FileName
        {
            get { return this.document.FileName; }
            set { this.document.FileName = value; }
        }

        /// <summary>
        /// Gets title of current opened document.
        /// </summary>
        public string Title
        {
            get { return this.document.Title; }
        }

        /// <summary>
        /// Gets array of document's lines.
        /// </summary>
        public List<string> Lines
        {
            get
            {
                return this.document.Lines.GetRange(0, this.document.Lines.Count);
            }
        }

        /// <summary>
        /// Gets all lines, joined by \n.
        /// </summary>
        public string Text
        {
            get { return string.Join("\n", this.Lines); }
        }

        /// <summary>
        /// Adds new line to the end of this document.
        /// </summary>
        /// <param name="line">Line to add.</param>
        public void AddLine(string line)
        {
            this.document.AddLine(line);
        }

        /// <summary>
        /// Insert new line at specified index.
        /// </summary>
        /// <param name="index">Index to insert.</param>
        /// <param name="line">Line to insert.</param>
        public void InsertLineAtIndex(int index, string line)
        {
            this.document.InsertLineAtIndex(index, line);
        }

        /// <summary>
        /// Insert new lines at specified index.
        /// </summary>
        /// <param name="index">Index to insert.</param>
        /// <param name="newLines">Lines to insert.</param>
        public void InsertLinesAtIndex(int index, List<string> newLines)
        {
            this.document.InsertLinesAtIndex(index, newLines);
        }

        /// <summary>
        /// Replace line at index with provided line.
        /// </summary>
        /// <param name="index">Index of line to replace.</param>
        /// <param name="newLine">New line.</param>
        public void ChangeLineAtIndex(int index, string newLine)
        {
            this.document.ChangeLineAtIndex(index, newLine);
        }

        /// <summary>
        /// Remove lines at specified index.
        /// </summary>
        /// <param name="index">Start index.</param>
        /// <param name="count">Number of lines to remove.</param>
        public void RemoveLines(int index, int count)
        {
            this.document.RemoveLines(index, count);
        }

        /// <summary>
        /// Remove one line at specified index.
        /// </summary>
        /// <param name="index">Index of line to remove.</param>
        public void RemoveLineAtIndex(int index)
        {
            this.document.RemoveLineAtIndex(index);
        }

        /// <summary>
        /// Finds number of line in document by caret index.
        /// </summary>
        /// <param name="caretIndex">Caret index in document.</param>
        /// <returns>Number of line in document.</returns>
        public int LineNumberByIndex(int caretIndex)
        {
            return this.document.LineNumberByIndex(caretIndex);
        }

        /// <summary>
        /// Calculates caret position in line by caret index.
        /// </summary>
        /// <param name="caretIndex">Caret index in document.</param>
        /// <returns>Caret position in line.</returns>
        public int CaretPositionInLineByIndex(int caretIndex)
        {
            return this.document.CaretPositionInLineByIndex(caretIndex);
        }

        /// <summary>
        /// Calculates caret index by provided line and position.
        /// </summary>
        /// <param name="line">Line index in document.</param>
        /// <param name="position">Position in line.</param>
        /// <returns>Caret index.</returns>
        public int CaretIndexByPosition(int line, int position)
        {
            return this.document.CaretIndexByPosition(line, position);
        }

        /// <summary>
        /// Finds word which precedes caret index.
        /// </summary>
        /// <param name="caretIndex">Index to search.</param>
        /// <returns>Word which precedes caret index.</returns>
        public string GetWordByCaretIndex(int caretIndex)
        {
            return this.document.GetWordByCaretIndex(caretIndex);
        }
    }
}
