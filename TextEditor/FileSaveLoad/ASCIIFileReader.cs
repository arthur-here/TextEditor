using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    /// <summary>
    /// Provides agorythm to reading ASCII encoded file
    /// </summary>
    public class ASCIIFileReader : FileReaderStrategy
    {
        public ASCIIFileReader(string fileName)
        {
            this.FileName = fileName;
        }

        public override string[] ReadFile()
        {
            List<string> result = new List<string>();
            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(this.FileName, Encoding.ASCII))
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
