using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using TextEditor.FileManager;

namespace TextEditor.FileManager
{
    /// <summary>
    /// Provides simple access to opening/saving files.
    /// </summary>
    public class TextEditorFileManager
    {
        /// <summary>
        /// Shows OpenFileDialog.
        /// </summary>
        /// <returns>TextEditorDocument with read data.</returns>
        public TextEditorDocument OpenFile()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            if (ofd.ShowDialog() == false)
            {
                return null;
            }

            string filename = ofd.FileName;
            FileReaderStrategy fileReader = new DefaultFileReader(filename);

            return this.readWithReaderStrategy(filename, fileReader);
        }

        /// <summary>
        /// Reads file at 'filename' using encoding 'encodingName'.
        /// </summary>
        /// <param name="fileName">Path to file.</param>
        /// <param name="encodingName">Name of encoding to use.</param>
        /// <returns>New TextEditorDocument with data from 'fileName'.</returns>
        public TextEditorDocument OpenFileUsingEncoding(string fileName, Encoding encoding)
        {
            FileReaderStrategy fileReader;
            if (encoding == Encoding.ASCII)
            {
                fileReader = new ASCIIFileReader(fileName);
            }
            else if (encoding == Encoding.UTF8)
            {
                fileReader = new UTF8FileReader(fileName);
            }
            else
            {
                fileReader = new DefaultFileReader(fileName);
            }

            return this.readWithReaderStrategy(fileName, fileReader);
        }

        /// <summary>
        /// Saves document to location specified in FileName.
        /// </summary>
        /// <param name="document">Document to save.</param>
        public void SaveDocument(TextEditorDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            using (FileStream sw = new FileStream(document.FileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                TextRange range = new TextRange(document.ContentStart, document.ContentEnd);
                range.Save(sw, DataFormats.Text);
            }
        }

        /// <summary>
        /// Shows SaveFileDialogSaves and saves document to specified location.
        /// </summary>
        /// <param name="document">Document to save.</param>
        public void SaveAsDocument(TextEditorDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();

            if (sfd.ShowDialog() == false)
            {
                return;
            }

            document.FileName = sfd.FileName;
            this.SaveDocument(document);
        }

        /// <summary>
        /// Shows SaveFileDialog to specify new document location.
        /// </summary>
        /// <returns>New document.</returns>
        public TextEditorDocument New()
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();

            if (sfd.ShowDialog() == false)
            {
                return null;
            }
            
            return new TextEditorDocument(sfd.FileName);
        }

        private TextEditorDocument readWithReaderStrategy(string filename, FileReaderStrategy strategy)
        {
            string[] text = strategy.Read();
            TextEditorDocument result = new TextEditorDocument(filename);
            foreach (string line in text)
            {
                result.Blocks.Add(new Paragraph(new Run(line)));
            }

            return result;
        }
    }
}
