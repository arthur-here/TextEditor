using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Commands;

namespace TextEditor
{
    /// <summary>
    /// Represents a text editor macros.
    /// </summary>
    public class Macro
    {
        private List<ICommand> commands = new List<ICommand>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Macro"/> class.
        /// </summary>
        /// <param name="name">Name of macro.</param>
        public Macro(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the macros name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Adds command to macro.
        /// </summary>
        /// <param name="command">Command to add.</param>
        public void AddCommand(ICommand command)
        {
            this.commands.Add(command);
        }

        /// <summary>
        /// Removes last command from macro queue.
        /// </summary>
        public void RemoveLastCommand()
        {
            if (this.commands.Count > 0)
            {
                this.commands.RemoveAt(this.commands.Count - 1);
            }
        }

        /// <summary>
        /// Runs the macro.
        /// </summary>
        /// <param name="document">Document to execute macro.</param>
        public void Run(TextEditorDocument document)
        {
            foreach (ICommand command in this.commands)
            {
                command.Execute(document);
            }
        }
    }
}
