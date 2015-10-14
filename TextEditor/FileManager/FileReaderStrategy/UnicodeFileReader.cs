using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.FileManager
{
    /// <summary>
    /// Provides algorithm to reading Unicode encoded file.
    /// </summary>
    public class UnicodeFileReader : FileReaderStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnicodeFileReader"/> class.
        /// </summary>
        /// <param name="fileName">Path to read file.</param>
        public UnicodeFileReader(string fileName) 
            : base(fileName)
        {
        }

        /// <summary>
        /// Reads data from file.
        /// </summary>
        /// <returns>Array of read strings.</returns>
        public override string[] Read()
        {
            return File.ReadAllLines(this.FileName, Encoding.Unicode);
        }
    }
}
