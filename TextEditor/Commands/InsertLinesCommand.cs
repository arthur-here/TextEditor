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
        private TextEditorDocument document;
        private int line;
        private int position;
        private List<string> text;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertLinesCommand"/> class.
        /// </summary>
        /// <param name="text">Text to insert.</param>
        /// <param name="document">Document insert to.</param>
        /// <param name="line">Line number.</param>
        /// <param name="position">Index in line.</param>
        public InsertLinesCommand(List<string> text, TextEditorDocument document, int line, int position)
        {
            this.text = text;
            this.document = document;
            this.line = line;
            this.position = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertLinesCommand"/> class.
        /// </summary>
        /// <param name="text">Text to insert.</param>
        /// <param name="document">Document insert to.</param>
        /// <param name="caretIndex">Index of caret in document.</param>
        public InsertLinesCommand(List<string> text, TextEditorDocument document, int caretIndex)
        {
            if (document == null)
            {
                throw new ArgumentException("document shouldn't be null");
            }

            this.text = text;
            this.document = document;
            this.line = this.document.LineNumberByIndex(caretIndex);
            this.position = this.document.CaretPositionInLineByIndex(caretIndex);
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        public void Execute()
        {
            if (this.text.Count == 0)
            {
                return;
            }

            string paragraph = this.document.Lines[this.line];
            string partToMove = paragraph.Substring(this.position);
            if (paragraph.Length > this.position)
            {
                paragraph = paragraph.Remove(this.position);
            }
            
            this.document.Lines[this.line] = paragraph.Insert(this.position, this.text.First());
            List<string> newLines = new List<string>(this.text);
            newLines.RemoveAt(0);
            for (int i = 0; i < newLines.Count; i++)
            {
                this.document.Lines.Insert(this.line + i + 1, newLines[i]);
            }

            this.document.Lines[this.line + this.text.Count - 1] += partToMove;
        }

        /// <summary>
        /// Undo command.
        /// </summary>
        public void Undo()
        {
            if (this.text.Count == 0)
            {
                return;
            }

            string lastAddedLine = this.document.Lines[this.line + this.text.Count - 1];
            string partToMoveBack = lastAddedLine.Substring(this.text.Last().Length);
            string paragraph = this.document.Lines.ElementAt(this.line);
            this.document.Lines[this.line] = paragraph.Remove(this.position, this.text.First().Length);
            this.document.Lines[this.line] += partToMoveBack;
            this.document.Lines.RemoveRange(this.line + 1, this.text.Count - 1);
        }
    }
}
