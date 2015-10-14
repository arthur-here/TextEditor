using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.FileManager
{
    /// <summary>
    /// Represents a TextEditor Document.
    /// </summary>
    public interface ITextEditorDocument
    {
        /// <summary>
        /// Gets or sets path to document.
        /// </summary>
        string FileName { get; set; }

        /// <summary>
        /// Gets or sets array of document's lines.
        /// </summary>
        List<string> Lines { get; set; }
    }
}
