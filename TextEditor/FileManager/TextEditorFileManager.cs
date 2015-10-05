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
        /// <returns>FlowDocument with read data.</returns>
        public TextEditorDocument OpenFile()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();

            if (ofd.ShowDialog() == false)
            {
                return null;
            }

            FileReaderStrategy fileReader;
            string filename = ofd.FileName;
            using (StreamReader streamReader = new StreamReader(filename))
            {
                streamReader.Peek();
                if (streamReader.CurrentEncoding == UTF8Encoding.Default)
                {
                    fileReader = new UTF8FileReader(filename);
                }
                else if (streamReader.CurrentEncoding == ASCIIEncoding.Default)
                {
                    fileReader = new ASCIIFileReader(filename);
                }
                else
                {
                    fileReader = new DefaultFileReader(filename);
                }
            }

            string[] text = fileReader.Read();
            TextEditorDocument result = new TextEditorDocument(filename);
            foreach (string line in text)
            {
                result.Blocks.Add(new Paragraph(new Run(line)));
            }

            return result;
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
    }
}
