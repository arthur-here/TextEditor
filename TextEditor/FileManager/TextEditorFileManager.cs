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
        public ITextEditorDocument OpenFile()
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
        public ITextEditorDocument OpenFileUsingEncoding(string fileName, Encoding encoding)
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
            else if (encoding == Encoding.Unicode)
            {
                fileReader = new UnicodeFileReader(fileName);
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
        public void SaveDocument(ITextEditorDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            File.WriteAllLines(document.FileName, document.Lines);
        }

        /// <summary>
        /// Shows SaveFileDialogSaves and saves document to specified location.
        /// </summary>
        /// <param name="document">Document to save.</param>
        public void SaveAsDocument(ITextEditorDocument document)
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
        public ITextEditorDocument New()
        {
            Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();

            if (sfd.ShowDialog() == false)
            {
                return null;
            }
            
            return new TextEditorDocument(sfd.FileName);
        }

        private ITextEditorDocument readWithReaderStrategy(string filename, FileReaderStrategy strategy)
        {
            string[] text = strategy.Read();
            TextEditorDocument result = new TextEditorDocument(filename);
            foreach (string line in text)
            {
                result.Lines.Add(line);
            }

            return result;
        }
    }
}
