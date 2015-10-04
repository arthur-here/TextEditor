using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    public interface IFileReader
    {
        string Filename { get; set; }

        string[] ReadFile();
    }
}
