using System;
using System.Collections.Generic;
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
            List<string> result = new List<string>();
            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(this.FileName, UTF8Encoding.UTF8))
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
