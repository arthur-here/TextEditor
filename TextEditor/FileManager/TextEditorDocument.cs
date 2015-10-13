using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TextEditor.FileManager
{
    /// <summary>
    /// Represents a TextEditor Document.
    /// </summary>
    public class TextEditorDocument
    {
        private string fileName;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditorDocument"/> class.
        /// </summary>
        /// <param name="fileName">Path to document.</param>
        public TextEditorDocument(string fileName)
            : base()
        {
            this.FileName = fileName;
        }

        /// <summary>
        /// Gets or sets path to document.
        /// </summary>
        public string FileName
        {
            get { return this.fileName; }
            set { this.fileName = value; }
        }

        /// <summary>
        /// Gets array of document's lines.
        /// </summary>
        public List<string> Lines
        {
            get
            {
                return lines;
            }
            set
            {
                lines = value;
            }
        }

        private List<string> lines = new List<string>();
    }
}
