using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextEditor.FileManager;

namespace TextEditor.FileManager
{
    /// <summary>
    /// Provides agorythm to reading UTF-8 encoded file
    /// </summary>
    public class UTF8FileReader : FileReaderStrategy
    {
        public UTF8FileReader(string fileName)
        {
            this.FileName = fileName;
        }

        public override string[] Read()
        {
            return File.ReadAllLines(this.FileName, Encoding.UTF8);
        }
    }
}
