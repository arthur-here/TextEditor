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
        /// Gets or sets array of document's lines.
        /// </summary>
        List<string> Lines { get; set; }

        /// <summary>
        /// Gets all lines, joined by \n.
        /// </summary>
        string Text { get; }

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
