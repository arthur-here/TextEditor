using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using TextEditor.FileManager;

namespace TextEditor.Commands
{
    /// <summary>
    /// Provides command to insert text into document at specified position.
    /// </summary>
    public class InsertStringCommand : ICommand
    {
        private TextEditorDocument document;
        private int line;
        private int position;
        private string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertStringCommand"/> class.
        /// </summary>
        /// <param name="text">Text to insert.</param>
        /// <param name="document">Document insert to.</param>
        /// <param name="line">Line number.</param>
        /// <param name="position">Index in line.</param>
        public InsertStringCommand(string text, TextEditorDocument document, int line, int position)
        {
            this.text = text;
            this.document = document;
            this.line = line;
            this.position = position;
        }

        /// <summary>
        /// Executes command.
        /// </summary>
        public void Execute()
        {
            Paragraph paragraph = this.document.Blocks.ElementAt(this.line) as Paragraph;
            TextRange textRange = new TextRange(paragraph.ContentStart, paragraph.ContentEnd);
            textRange.Text = textRange.Text.Insert(this.position, this.text); 
        }

        /// <summary>
        /// Undo command.
        /// </summary>
        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
