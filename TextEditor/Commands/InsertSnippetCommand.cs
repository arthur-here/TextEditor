using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Commands
{
    /// <summary>
    /// Provides command to insert snippet into document at specified position.
    /// Consists from two other commands: RemoveRange and InsertLines.
    /// </summary>
    public class InsertSnippetCommand : ICommand
    {
        private TextEditorDocument document;
        private int line;
        private int position;
        private Snippet snippet;

        private RemoveRangeCommand removeCommand;
        private InsertLinesCommand insertCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertSnippetCommand"/> class.
        /// </summary>
        /// <param name="snippet">Snippet to insert.</param>
        /// <param name="document">Document insert to.</param>
        /// <param name="line">Line number.</param>
        /// <param name="position">Index in line.</param>
        public InsertSnippetCommand(Snippet snippet, TextEditorDocument document, int line, int position)
        {
            if (document == null)
            {
                throw new ArgumentException("Document shouldn't be null");
            }
            
            this.snippet = snippet;
            this.document = document;
            this.line = line;
            this.position = position;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertSnippetCommand"/> class.
        /// </summary>
        /// <param name="snippet">Snippet to insert.</param>
        /// <param name="document">Document insert to.</param>
        /// <param name="caretIndex">Index of caret in document.</param>
        public InsertSnippetCommand(Snippet snippet, TextEditorDocument document, int caretIndex)
        {
            if (document == null)
            {
                throw new ArgumentException("Document shouldn't be null");
            }
            
            this.snippet = snippet;
            this.document = document;
            this.line = this.document.LineNumberByIndex(caretIndex);
            this.position = this.document.CaretPositionInLineByIndex(caretIndex);
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        public void Execute()
        {
            string paragrapgh = this.document.Lines[this.line];
            int paragraphIndex = this.position;
            int length = 0;
            while (paragraphIndex < paragrapgh.Length && paragrapgh[paragraphIndex] == this.snippet.Name[length])
            {
                paragraphIndex++;
                length++;
            }

            this.removeCommand = new RemoveRangeCommand(this.document, this.line, this.position, length);
            this.removeCommand.Execute();
            this.insertCommand = new InsertLinesCommand(this.snippet.Content, this.document, this.line, this.position);
            this.insertCommand.Execute();
        }

        /// <summary>
        /// Undo command.
        /// </summary>
        public void Undo()
        {
            this.insertCommand.Undo();
            this.removeCommand.Undo();
        }
    }
}
