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
    class InsertStringCommand : ICommand
    {
        private TextEditorDocument document;
        private int line;
        private int position;

        public InsertStringCommand(TextEditorDocument document, int line, int position)
        {
            this.document = document;
            this.line = line;
            this.position = position;
        }

        public void Execute()
        {
            Paragraph paragraph = document.Blocks.ElementAt(line) as Paragraph;
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}
