using System;
using System.Collections.Generic;
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
        {
            this.FileName = fileName;
        }

        public override string[] Read()
        {
            List<string> result = new List<string>();
            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(this.FileName, ASCIIEncoding.Default))
            {
                while (!streamReader.EndOfStream)
                {
                    result.Add(streamReader.ReadLine());
                }
            }

            return result.ToArray();
        }
    }
}
