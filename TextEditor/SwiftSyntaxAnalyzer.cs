using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    /// <summary>
    /// Concrete implementation of Swift syntax analyzer.
    /// </summary>
    public class SwiftSyntaxAnalyzer : ISyntaxAnalyzer
    {
        private Dictionary<string, TokenType> library = new Dictionary<string, TokenType>()
        {
            { "if", TokenType.Keyword },
            { "else", TokenType.Keyword },
            { "for", TokenType.Keyword }
        };

        /// <summary>
        /// Gets list of parsed tokens from text.
        /// </summary>
        public List<Token> Tokens { get; private set; }

        /// <summary>
        /// Parse document text and saves result in Tokens.
        /// </summary>
        /// <param name="document">Document to analyze.</param>
        public void Parse(TextEditorDocument document)
        {
            if (document == null)
            {
                return;
            }

            this.Tokens.Clear();
            int caretIndex = 0;
            foreach (string line in document.Lines)
            {
                List<string> words = line.Split(' ').ToList();
                foreach (string word in words)
                {
                    string key = getKeyForWord(word);
                    if (key != null)
                    {
                        Token token = new Token(this.library[key], caretIndex, word.Length);
                        this.Tokens.Add(token);
                    }
                    caretIndex += word.Length;
                }
                caretIndex++;
            }
        }

        private string getKeyForWord(string word)
        {
            return library.Keys.Where(k => word.Contains(k)).FirstOrDefault();
        }
    }
}
