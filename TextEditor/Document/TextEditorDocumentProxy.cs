using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// Gets or sets array of document's lines.
        /// </summary>
        public List<string> Lines
        {
            get
            {
                return this.document.Lines;
            }

            set
            {
                this.document.Lines = value;
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
