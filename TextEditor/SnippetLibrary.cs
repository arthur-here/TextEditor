using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace TextEditor
{
    /// <summary>
    /// Provides access to library of snippets.
    /// </summary>
    public class SnippetLibrary
    {
        private List<Snippet> snippets;

        private string filename = "UserData\\SnippetLibrary.xml";

        /// <summary>
        /// Initializes a new instance of the <see cref="SnippetLibrary"/> class.
        /// </summary>
        public SnippetLibrary()
        {
            try
            {
                using (FileStream fs = new FileStream(this.filename, FileMode.Open))
                {
                    XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas());
                    DataContractSerializer ser = new DataContractSerializer(typeof(List<Snippet>));
                    this.snippets = (List<Snippet>)ser.ReadObject(reader, true) ?? new List<Snippet>();
                    reader.Close();
                }
            }
            catch (FileNotFoundException e)
            {
                Console.WriteLine(e);
                this.snippets = new List<Snippet>();
            }
        }

        /// <summary>
        /// Gets list of stored snippets names.
        /// </summary>
        public List<string> Names
        {
            get
            {
                return this.snippets.Select(s => s.Name).ToList();
            }
        }

        /// <summary>
        /// Gets stored snippet by name.
        /// </summary>
        /// <param name="name">Snippet's name.</param>
        /// <returns>Found snippet.</returns>
        public Snippet GetByName(string name)
        {
            return this.snippets.Where(s => s.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Adds new snippet to library.
        /// </summary>
        /// <param name="snippet">Snippet to add.</param>
        public void Add(Snippet snippet)
        {
            if (this.snippets.Where(s => s.Name == snippet.Name).Count() == 0)
            {
                this.snippets.Add(snippet);
                this.Save();
            }            
        }

        /// <summary>
        /// Remove snippet with specified name.
        /// </summary>
        /// <param name="snippetName">Name of snippet to remove.</param>
        public void Remove(string snippetName)
        {
            var itemToRemove = this.snippets.Single(s => s.Name == snippetName);
            if (itemToRemove != null)
            {
                this.snippets.Remove(itemToRemove);
                this.Save();
            }
        }

        /// <summary>
        /// Removes all snippets from library.
        /// </summary>
        public void RemoveAll()
        {
            this.snippets = new List<Snippet>();
            this.Save();
        }

        private void Save()
        {
            using (Stream writer = new FileStream(this.filename, FileMode.Create))
            {
                DataContractSerializer ser = new DataContractSerializer(typeof(List<Snippet>));
                ser.WriteObject(writer, this.snippets);
            }
        }
    }
}
