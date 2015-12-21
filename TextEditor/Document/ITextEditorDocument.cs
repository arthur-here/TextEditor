using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    /// <summary>
    /// Provides an interface for usage of TextEditorDocuments
    /// </summary>
    public interface ITextEditorDocument
    {
        /// <summary>
        /// Gets or sets path to document.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets title of current opened document.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Gets array of all document's lines.
        /// </summary>
        List<string> AllLines { get; }

        /// <summary>
        /// Gets all lines, joined by \n.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets or sets offset to show lines.
        /// </summary>
        int LinesOffset { get; set; }

        /// <summary>
        /// Gets or sets amount of lines to render.
        /// </summary>
        int AmountOfLinesToShow { get; set; }

        /// <summary>
        /// Adds new line to the end of this document.
        /// </summary>
        /// <param name="line">Line to add.</param>
        void AddLine(string line);

        /// <summary>
        /// Insert new line at specified index.
        /// </summary>
        /// <param name="index">Index to insert.</param>
        /// <param name="line">Line to insert.</param>
        void InsertLineAtIndex(int index, string line);

        /// <summary>
        /// Insert new lines at specified index.
        /// </summary>
        /// <param name="index">Index to insert.</param>
        /// <param name="newLines">Lines to insert.</param>
        void InsertLinesAtIndex(int index, List<string> newLines);

        /// <summary>
        /// Replace line at index with provided line.
        /// </summary>
        /// <param name="index">Index of line to replace.</param>
        /// <param name="newLine">New line.</param>
        void ChangeLineAtIndex(int index, string newLine);

        /// <summary>
        /// Remove lines at specified index.
        /// </summary>
        /// <param name="index">Start index.</param>
        /// <param name="count">Number of lines to remove.</param>
        void RemoveLines(int index, int count);

        /// <summary>
        /// Remove one line at specified index.
        /// </summary>
        /// <param name="index">Index of line to remove.</param>
        void RemoveLineAtIndex(int index);

        /// <summary>
        /// Finds number of line in document by caret index.
        /// </summary>
        /// <param name="caretIndex">Caret index in document.</param>
        /// <returns>Number of line in document.</returns>
        int LineNumberByIndex(int caretIndex);

        /// <summary>
        /// Calculates caret position in line by caret index.
        /// </summary>
        /// <param name="caretIndex">Caret index in document.</param>
        /// <returns>Caret position in line.</returns>
        int CaretPositionInLineByIndex(int caretIndex);

        /// <summary>
        /// Calculates caret index by provided line and position.
        /// </summary>
        /// <param name="line">Line index in document.</param>
        /// <param name="position">Position in line.</param>
        /// <returns>Caret index.</returns>
        int CaretIndexByPosition(int line, int position);

        /// <summary>
        /// Finds word which precedes caret index.
        /// </summary>
        /// <param name="caretIndex">Index to search.</param>
        /// <returns>Word which precedes caret index.</returns>
        string GetWordByCaretIndex(int caretIndex);
    }
}
