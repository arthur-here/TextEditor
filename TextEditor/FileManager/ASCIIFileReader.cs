using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.FileManager
{
    /// <summary>
    /// Provides agorythm to reading ASCII encoded file.
    /// </summary>
    public class ASCIIFileReader : FileReaderStrategy
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ASCIIFileReader"/> class.
        /// </summary>
        /// <param name="fileName">Path to read file.</param>
        public ASCIIFileReader(string fileName)
            : base(fileName)
        {
        }

        public override string[] Read()
        {
            return File.ReadAllLines(this.FileName, Encoding.ASCII);
        }
    }
}
