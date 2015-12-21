using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.Commands;

namespace TextEditor
{
    /// <summary>
    /// Represents library of Text Editor macros.
    /// </summary>
    public class MacroLibrary
    {
        private List<Macro> library = new List<Macro>();
        private Macro newMacro;
        private bool isRecording = false;

        /// <summary>
        /// Gets library of macros.
        /// </summary>
        public List<Macro> Library
        {
            get { return this.library; }
        }

        /// <summary>
        /// Gets a value indicating whether recording is on.
        /// </summary>
        public bool IsRecording
        {
            get { return this.isRecording; }
        }

        /// <summary>
        /// Starts recording.
        /// </summary>
        public void StartRecording()
        {
            this.newMacro = new Macro("New Macro");
            this.isRecording = true;
        }

        /// <summary>
        /// Finishes recording.
        /// </summary>
        /// <returns>Recorded macro.</returns>
        public Macro StopRecording()
        {
            Macro recordedMacro = this.newMacro;
            this.isRecording = false;
            this.newMacro = null;
            return recordedMacro;
        }

        /// <summary>
        /// Adds command to new macro during recording.
        /// </summary>
        /// <param name="command">Command to add.</param>
        public void AddCommand(ICommand command)
        {
            if (this.isRecording)
            {
                this.newMacro.AddCommand(command);
            }
        }
    }
}
