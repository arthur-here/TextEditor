using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.FileManager
{
    class TextEditorDocumentProxy : ITextEditorDocument
    {
        private TextEditorDocument realDocument;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextEditorDocument"/> class.
        /// </summary>
        /// <param name="fileName">Path to document.</param>
        public TextEditorDocumentProxy(TextEditorDocument realDocument)
            : base()
        {
            this.realDocument = realDocument;
        }

        /// <summary>
        /// Gets or sets path to document.
        /// </summary>
        public string FileName
        {
            get { return this.realDocument.FileName; }
            set { this.realDocument.FileName = value; }
        }

        /// <summary>
        /// Gets or sets array of document's lines.
        /// </summary>
        public List<string> Lines
        {
            get
            {
                List<string> result = new List<string>();
                int linesCount = 0;
                foreach (string line in this.realDocument.Lines) {
                    linesCount++;
                    if (linesCount > 40)
                    {
                        break;
                    }
                    result.Add(line);
                }
                return result;
            }

            set
            {
                this.realDocument.Lines = value;
            }
        }
    }
}
