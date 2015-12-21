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
        private int caretIndex;
        private int indentationLevel;

        private ITextEditorDocument changedDocument;
        private int line;
        private int position;
        private string changedLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewLineCommand"/> class.
        /// </summary>
        /// <param name="indentationLevel">How many spaces place before line.</param>
        /// <param name="caretIndex">Index of caret in document.</param>
        public NewLineCommand(int indentationLevel, int caretIndex)
        {
            this.indentationLevel = indentationLevel;
            this.caretIndex = caretIndex;
            this.CaretIndexOffset = indentationLevel + 1;
        }

        /// <summary>
        /// Gets offset of the caret index after command's execution.
        /// </summary>
        public int CaretIndexOffset { get; private set; }

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="document">Document to run command.</param>
        public void Execute(ITextEditorDocument document)
        {
            if (document == null)
            {
                return;
            }

            this.line = document.LineNumberByIndex(this.caretIndex);
            this.position = document.CaretPositionInLineByIndex(this.caretIndex);
            this.changedDocument = document;

            string paragraph = document.AllLines[this.line];
            this.changedLine = document.AllLines[this.line];

            string substringToTranslate = string.Empty;
            if (this.position < paragraph.Length)
            {
                int substringLength = paragraph.Length - this.position;
                substringToTranslate = paragraph.Substring(this.position, substringLength);
                document.ChangeLineAtIndex(this.line, paragraph.Remove(this.position, substringLength));
            }

            for (int i = 0; i < this.indentationLevel; i++)
            {
                substringToTranslate = " " + substringToTranslate;
            }

            document.InsertLineAtIndex(this.line + 1, substringToTranslate);
        }

        /// <summary>
        /// Executes command on new caretIndex.
        /// </summary>
        /// <param name="document">Document to change.</param>
        /// <param name="newCaretIndex">New caretIndex.</param>
        public void Execute(ITextEditorDocument document, int newCaretIndex)
        {
            this.caretIndex = newCaretIndex;
            this.Execute(document);
        }

        /// <summary>
        /// Undo command.
        /// </summary>
        public void Undo()
        {
            this.changedDocument.ChangeLineAtIndex(this.line, this.changedLine);
            this.changedDocument.RemoveLineAtIndex(this.line + 1);
        }
    }
}
