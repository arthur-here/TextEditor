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
        /// <param name="document">Document to change.</param>
        void Execute(TextEditorDocument document);

        /// <summary>
        /// Unexecutes command.
        /// </summary>
        void Undo();
    }
}
