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
        private int caretIndex;
        private Snippet snippet;
        
        private int line;
        private int position;

        private RemoveRangeCommand removeCommand;
        private InsertLinesCommand insertCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertSnippetCommand"/> class.
        /// </summary>
        /// <param name="snippet">Snippet to insert.</param>
        /// <param name="caretIndex">Index of caret in document.</param>
        public InsertSnippetCommand(Snippet snippet, int caretIndex)
        {
            this.snippet = snippet;
            this.caretIndex = caretIndex;
        }

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

            string paragrapgh = document.Lines[this.line];
            int paragraphIndex = this.position;
            int length = 0;
            while (paragraphIndex < paragrapgh.Length && paragrapgh[paragraphIndex] == this.snippet.Name[length])
            {
                paragraphIndex++;
                length++;
            }

            this.removeCommand = new RemoveRangeCommand(this.caretIndex, length);
            this.removeCommand.Execute(document);
            this.insertCommand = new InsertLinesCommand(this.snippet.Content, this.caretIndex);
            this.insertCommand.Execute(document);
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
            this.insertCommand.Undo();
            this.removeCommand.Undo();
        }
    }
}
