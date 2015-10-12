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
    public class TextEditorDocument : FlowDocument
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
            this.setupUI();
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
        public Collection<string> Lines
        {
            get
            {
                Collection<string> result = new Collection<string>();
                foreach (Block b in this.Blocks)
                {
                    result.Add(new TextRange(b.ContentStart, b.ContentEnd).Text);
                }

                return result;
            }
        }

        /// <summary>
        /// Gets amount of lines in document.
        /// </summary>
        public int LinesCount
        {
            get { return this.Blocks.Count; }
        }

        private void setupUI()
        {
            this.FontFamily = new System.Windows.Media.FontFamily("Consolas");
            this.FontSize = 12.0;
        }
    }
}
