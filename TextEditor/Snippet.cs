using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    /// <summary>
    /// Represents snippet in document.
    /// </summary>
    [DataContract]
    public class Snippet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Snippet"/> class.
        /// </summary>
        /// <param name="name">Name of snippet.</param>
        /// <param name="content">Snippet's content.</param>
        public Snippet(string name, List<string> content)
        {
            this.Name = name;
            this.Content = content;
        }
        
        /// <summary>
        /// Gets or sets name of snippet.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets content of snippet.
        /// </summary>
        [DataMember]
        public List<string> Content { get; set; }
    }
}
