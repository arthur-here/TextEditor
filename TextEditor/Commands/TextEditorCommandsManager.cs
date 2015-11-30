using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Commands
{
    /// <summary>
    /// Provides mechanism to manage commands in TextEditor.
    /// </summary>
    public class TextEditorCommandManager
    {
        private List<ICommand> commandsQueue = new List<ICommand>();
        private int lastExecutedCommandIndex = -1;
        private ITextEditorDocument document;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditorCommandManager"/> class.
        /// </summary>
        /// <param name="document">Document to manage.</param>
        public TextEditorCommandManager(ITextEditorDocument document)
        {
            this.document = document;
        }

        /// <summary>
        /// Adds new command to command queue.
        /// </summary>
        /// <param name="command">Command to add to queue.</param>
        public void AddCommand(ICommand command)
        {
            int unusedCommandCount = this.commandsQueue.Count - (this.lastExecutedCommandIndex + 1);
            this.commandsQueue.RemoveRange(this.lastExecutedCommandIndex + 1, unusedCommandCount);
            this.commandsQueue.Add(command);
        }

        /// <summary>
        /// Removes last command in queue.
        /// </summary>
        public void RemoveLastCommand()
        {
            if (this.lastExecutedCommandIndex != -1)
            {
                this.lastExecutedCommandIndex--;
                this.commandsQueue.RemoveAt(this.commandsQueue.Count - 1);
            }
        }

        /// <summary>
        /// Executes new commands in queue.
        /// </summary>
        public void Run()
        {
            for (int index = this.lastExecutedCommandIndex + 1; index < this.commandsQueue.Count; index++)
            {
                this.commandsQueue.ElementAt(index).Execute(this.document);
            }

            this.lastExecutedCommandIndex = this.commandsQueue.Count - 1;
        }

        /// <summary>
        /// Undo last executed command.
        /// </summary>
        public void Undo()
        {
            if (this.lastExecutedCommandIndex == -1)
            {
                return;
            }

            this.commandsQueue[this.lastExecutedCommandIndex].Undo();
            this.lastExecutedCommandIndex--;
        }
    }
}
