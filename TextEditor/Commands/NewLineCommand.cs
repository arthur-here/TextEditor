using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Commands
{
    /// <summary>
    /// Provides command to insert new line into document at specified position.
    /// </summary>
    public class NewLineCommand : ICommand
    {
        private TextEditorDocument document;
        private int currentLineIndex;
        private int currentLineCaretPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewLineCommand"/> class.
        /// </summary>
        /// <param name="document">Document insert to.</param>
        /// <param name="currentLineIndex">Number of current line.</param>
        /// <param name="currentLineCaretPosition">Caret position in current line.</param>
        public NewLineCommand(TextEditorDocument document, int currentLineIndex, int currentLineCaretPosition)
        {
            this.document = document;
            this.currentLineIndex = currentLineIndex;
            this.currentLineCaretPosition = currentLineCaretPosition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NewLineCommand"/> class.
        /// </summary>
        /// <param name="document">Document insert to.</param>
        /// <param name="caretIndex">Index of caret in document.</param>
        public NewLineCommand(TextEditorDocument document, int caretIndex)
        {
            if (document == null)
            {
                throw new ArgumentException("Document shouldn't be null.");
            }

            this.document = document;
            this.currentLineIndex = document.LineNumberByIndex(caretIndex);
            this.currentLineCaretPosition = document.CaretPositionInLineByIndex(caretIndex);
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        public void Execute()
        {
            string paragraph = this.document.Lines[this.currentLineIndex];
            string substringToTranslate = string.Empty;
            if (this.currentLineCaretPosition < paragraph.Length)
            {
                int substringLength = paragraph.Length - this.currentLineCaretPosition;
                substringToTranslate = paragraph.Substring(this.currentLineCaretPosition, substringLength);
                this.document.Lines[this.currentLineIndex] = paragraph.Remove(this.currentLineCaretPosition, substringLength);
            }

            this.document.Lines.Insert(this.currentLineIndex + 1, substringToTranslate);
        }

        /// <summary>
        /// Undo command.
        /// </summary>
        public void Undo()
        {
            this.document.Lines[this.currentLineIndex] = this.document.Lines[this.currentLineIndex] + 
                this.document.Lines[this.currentLineIndex + 1];
            this.document.Lines.RemoveAt(this.currentLineIndex + 1);
        }
    }
}
