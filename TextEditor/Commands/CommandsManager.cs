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
        private Queue<ICommand> commandsQueue = new Queue<ICommand>();

        private int lastExecutedCommandIndex = 0;

        /// <summary>
        /// Adds new command to command queue.
        /// </summary>
        /// <param name="command">Command to add to queue.</param>
        public void AddCommand(ICommand command)
        {
            this.commandsQueue.Enqueue(command);
        }

        /// <summary>
        /// Executes new commands in queue.
        /// </summary>
        public void Run()
        {
            for (int index = this.lastExecutedCommandIndex; index < this.commandsQueue.Count; index++)
            {
                this.commandsQueue.ElementAt(index).Execute();
            }

            this.lastExecutedCommandIndex = this.commandsQueue.Count;
        }

        /// <summary>
        /// Undo last executed command.
        /// </summary>
        public void Undo()
        {
            if (this.lastExecutedCommandIndex == 0)
            {
                return;
            }

            this.commandsQueue.Last().Undo();
            this.lastExecutedCommandIndex--;
        }
    }
}
