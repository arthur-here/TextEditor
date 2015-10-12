using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.Commands
{
    /// <summary>
    /// Provides interface for Command.
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// Executes command.
        /// </summary>
        void Execute();

        /// <summary>
        /// Unexecutes command.
        /// </summary>
        void Undo();
    }
}
