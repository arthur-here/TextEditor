using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Commands
{
    /// <summary>
    /// Provides command to insert lines into document at specified position.
    /// </summary>
    public class InsertLinesCommand : ICommand
    {
        private int caretIndex;
        private List<string> text;

        private TextEditorDocument changedDocument;
        private int line;
        private int position;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertLinesCommand"/> class.
        /// </summary>
        /// <param name="text">Text to insert.</param>
        /// <param name="caretIndex">Index of caret in document.</param>
        public InsertLinesCommand(List<string> text, int caretIndex)
        {
            this.text = text;
            this.caretIndex = caretIndex;
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="document">Document insert to.</param>
        public void Execute(TextEditorDocument document)
        {
            if (this.text.Count == 0 || document == null)
            {
                return;
            }

            this.line = document.LineNumberByIndex(this.caretIndex);
            this.position = document.CaretPositionInLineByIndex(this.caretIndex);
            this.changedDocument = document;

            if (this.line == -1)
            {
                this.line = 0;
                document.Lines.Add(string.Empty);
            }

            string paragraph = document.Lines[this.line];
            string partToMove = paragraph.Substring(this.position);
            if (paragraph.Length > this.position)
            {
                paragraph = paragraph.Remove(this.position);
            }

            document.Lines[this.line] = paragraph.Insert(this.position, this.text.First());
            List<string> newLines = new List<string>(this.text);
            newLines.RemoveAt(0);
            for (int i = 0; i < newLines.Count; i++)
            {
                document.Lines.Insert(this.line + i + 1, newLines[i]);
            }

            document.Lines[this.line + this.text.Count - 1] += partToMove;
        }

        /// <summary>
        /// Executes command on new caretIndex.
        /// </summary>
        /// <param name="document">Document to change.</param>
        /// <param name="newCaretIndex">New caretIndex.</param>
        public void Execute(TextEditorDocument document, int newCaretIndex)
        {
            this.caretIndex = newCaretIndex;
            this.Execute(document);
        }

        /// <summary>
        /// Undo command.
        /// </summary>
        public void Undo()
        {
            if (this.text.Count == 0 || this.changedDocument == null)
            {
                return;
            }

            string lastAddedLine = this.changedDocument.Lines[this.line + this.text.Count - 1];
            string partToMoveBack = lastAddedLine.Substring(this.text.Last().Length);
            string paragraph = this.changedDocument.Lines.ElementAt(this.line);
            this.changedDocument.Lines[this.line] = paragraph.Remove(this.position, this.text.First().Length);
            this.changedDocument.Lines[this.line] += partToMoveBack;
            this.changedDocument.Lines.RemoveRange(this.line + 1, this.text.Count - 1);
        }
    }
}
