using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Utilities;

namespace TextEditor.Commands
{
    /// <summary>
    /// Provides command to insert lines into document at specified position.
    /// </summary>
    public class InsertLinesCommand : ICommand
    {
        private int caretIndex;
        private List<string> text;

        private ITextEditorDocument changedDocument;
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
            this.CaretIndexOffset = text.Text().Length;
        }

        /// <summary>
        /// Gets offset of the caret index after command's execution.
        /// </summary>
        public int CaretIndexOffset { get; private set; }

        /// <summary>
        /// Executes command.
        /// </summary>
        /// <param name="document">Document insert to.</param>
        public void Execute(ITextEditorDocument document)
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
                document.AddLine(string.Empty);
            }

            string paragraph = document.AllLines[this.line];
            string partToMove = paragraph.Substring(this.position);
            if (paragraph.Length > this.position)
            {
                paragraph = paragraph.Remove(this.position);
            }
            
            document.ChangeLineAtIndex(this.line, paragraph.Insert(this.position, this.text.First()));
            List<string> newLines = new List<string>(this.text);
            newLines.RemoveAt(0);
            for (int i = 0; i < newLines.Count; i++)
            {
                document.InsertLineAtIndex(this.line + i + 1, newLines[i]);
            }

            string lastLine = document.AllLines[this.line + this.text.Count - 1];
            document.ChangeLineAtIndex(this.line + this.text.Count - 1, lastLine + partToMove);
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
            if (this.text.Count == 0 || this.changedDocument == null)
            {
                return;
            }

            string lastAddedLine = this.changedDocument.AllLines[this.line + this.text.Count - 1];
            string partToMoveBack = lastAddedLine.Substring(this.text.Last().Length);
            string paragraph = this.changedDocument.AllLines.ElementAt(this.line);
            string lastLine = paragraph.Remove(this.position, this.text.First().Length) + partToMoveBack;
            this.changedDocument.ChangeLineAtIndex(this.line, lastLine);
            this.changedDocument.RemoveLines(this.line + 1, this.text.Count - 1);
        }
    }
}
