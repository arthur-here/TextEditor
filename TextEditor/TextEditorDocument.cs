﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace TextEditor
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

        public string Title
        {
            get
            {
                int lastSlashIndex = this.FileName.LastIndexOf('\\');
                if (lastSlashIndex == -1)
                {
                    return this.FileName;
                }
                else
                {
                    return this.FileName.Substring(lastSlashIndex + 1);
                }
            }
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

        /// <summary>
        /// Gets all lines, joined by \n.
        /// </summary>
        public string Text
        {
            get
            {
                return string.Join("\n", this.Lines);
            }
        }

        private List<string> lines = new List<string>();

        /// <summary>
        /// Finds number of line in document by carret index.
        /// </summary>
        /// <param name="caretIndex">Caret index in document.</param>
        /// <returns>Number of line in document.</returns>
        public int LineNumberByIndex(int caretIndex)
        {
            if (caretIndex < 0 || caretIndex > this.Text.Length)
            {
                throw new ArgumentException("Caret index should be >= 0");
            }

            int line = 0;
            while (line < this.Lines.Count && caretIndex - this.Lines[line].Length - 1 >= 0)
            {
                caretIndex -= this.Lines[line].Length+1;
                line++;
            }

            if (line >= this.Lines.Count)
            {
                line = this.Lines.Count - 1;
            }

            return line;
        }

        /// <summary>
        /// Calculates carret position in line by carret index.
        /// </summary>
        /// <param name="caretIndex">Caret index in document.</param>
        /// <returns>Carret position in line.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "InLine")]
        public int CaretPositionInLineByIndex(int caretIndex)
        {
            if (caretIndex < 0 || caretIndex > this.Text.Length)
            {
                throw new ArgumentException("Caret index should be >= 0");
            }

            int line = 0;
            while (line < this.Lines.Count && caretIndex - this.Lines[line].Length - 1 >= 0)
            {
                caretIndex -= this.Lines[line].Length + 1;
                line++;
            }

            return caretIndex;
        }
    }
}
