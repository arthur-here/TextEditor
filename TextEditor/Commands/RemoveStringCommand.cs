using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Commands
{
    /// <summary>
    /// Provides command to remove string from document at specified position.
    /// </summary>
    public class RemoveStringCommand : ICommand
    {
        private TextEditorDocument document;
        private int line;
        private int position;
        private int length;
        private string removedString;
        private bool isLineRemoved = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveStringCommand"/> class.
        /// </summary>
        /// <param name="document">Document insert to.</param>
        /// <param name="line">Line number.</param>
        /// <param name="position">Index in line.</param>
        /// <param name="length">Count of chars to delete.</param>
        public RemoveStringCommand(TextEditorDocument document, int line, int position, int length)
        {
            this.document = document;
            this.line = line;
            this.position = position;
            this.length = length;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveStringCommand"/> class.
        /// </summary>
        /// <param name="document">Document insert to.</param>
        /// <param name="caretIndex">Index of caret in document.</param>
        /// <param name="length">Count of chars to delete.</param>
        public RemoveStringCommand(TextEditorDocument document, int caretIndex, int length)
        {
            if (document == null)
            {
                throw new ArgumentException("Document shouldn't be null");
            }

            this.document = document;
            this.line = this.document.LineNumberByIndex(caretIndex);
            this.position = this.document.CaretPositionInLineByIndex(caretIndex);
            this.length = length;
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        public void Execute()
        {
            string paragraph = this.document.Lines[this.line];
            if (this.position + this.length > paragraph.Length)
            {
                if (this.line + 1 < this.document.Lines.Count)
                {
                    this.isLineRemoved = true;
                    this.document.Lines.RemoveAt(this.line + 1);
                }

                return;
            }

            this.removedString = paragraph.Substring(this.position, this.length);
            this.document.Lines[this.line] = paragraph.Remove(this.position, this.length);
        }

        /// <summary>
        /// Undo command.
        /// </summary>
        public void Undo()
        {
            if (this.removedString == null)
            {
                if (this.isLineRemoved)
                {
                    this.document.Lines.Insert(this.line + 1, string.Empty);
                }

                return;
            }

            string paragraph = this.document.Lines.ElementAt(this.line);
            this.document.Lines[this.line] = paragraph.Insert(this.position, this.removedString);
        }
    }
}
