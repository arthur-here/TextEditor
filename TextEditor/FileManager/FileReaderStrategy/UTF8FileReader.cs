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
    /// Provides algorithm to reading UTF-8 encoded file.
    /// </summary>
    public class UTF8FileReader : FileReaderStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UTF8FileReader"/> class.
        /// </summary>
        /// <param name="fileName">Path to read file.</param>
        public UTF8FileReader(string fileName)
            : base(fileName)
        {
        }

        /// <summary>
        /// Reads data from file.
        /// </summary>
        /// <returns>Array of read strings.</returns>
        public override string[] Read()
        {
            return File.ReadAllLines(this.FileName, Encoding.UTF8);
        }
    }
}
