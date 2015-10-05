using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TextEditor.FileManager
{
    /// <summary>
    /// Represents a TextEditor Document.
    /// </summary>
    public interface ITextEditorDocument
    {
        /// <summary>
        /// Path to document.
        /// </summary>
        string Filename { get; set; }

        /// <summary>
        /// Array of document's lines
        /// </summary>
        string[] Lines { get; set; }

        /// <summary>
        /// Amount of lines in document
        /// </summary>
        int LinesCount { get; }
    }
}
