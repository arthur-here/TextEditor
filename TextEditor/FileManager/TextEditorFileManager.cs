using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public FlowDocument OpenFile()
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
            FlowDocument result = new FlowDocument();
            foreach (string line in text)
            {
                result.Blocks.Add(new Paragraph(new Run(line)));
            }

            return result;
        }
    }
}
