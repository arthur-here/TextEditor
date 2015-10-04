using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor.FileManager
{
    /// <summary>
    /// Represents an abstract strategy class for reading from file
    /// in different encodings.
    /// </summary>
    public abstract class FileReaderStrategy
    {
        private string fileName;

        /// <summary>
        /// Gets or sets the path to file.
        /// </summary>
        /// <value>The FileName property gets/sets the value of the string field, filename.</value>
        public string FileName
        {
            get
            {
                return this.fileName;
            }

            set
            {
                this.fileName = value;
            }
        }

        /// <summary>
        /// Reads data from file.
        /// </summary>
        /// <returns>Array of read strings.</returns>
        public abstract string[] Read();
    }
}
