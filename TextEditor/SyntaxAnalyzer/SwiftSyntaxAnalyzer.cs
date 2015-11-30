using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace TextEditor
{
    /// <summary>
    /// Concrete implementation of Swift syntax analyzer.
    /// </summary>
    public class SwiftSyntaxAnalyzer : ISyntaxAnalyzer
    {
        private Dictionary<string, TokenType> library;
        private string keywordsPattern;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwiftSyntaxAnalyzer"/> class.
        /// </summary>
        public SwiftSyntaxAnalyzer()
        {
            this.InitLibrary();
            this.Tokens = new List<Token>();
        }

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

            this.ParseKeywords(document.Text);
            this.ParseComments(document.Text);
        }

        private void ParseComments(string text)
        {
            string pattern = @"(?s:/\*((?!\*/).)*\*/)";
            MatchCollection matches = Regex.Matches(text, pattern);
            foreach (Match m in matches)
            {
                Tokens.Add(new Token(TokenType.Comment, m.Index, m.Length));
            }

            pattern = "//+.*";
            matches = Regex.Matches(text, pattern);
            foreach (Match m in matches)
            {
                this.Tokens.Add(new Token(TokenType.Comment, m.Index, m.Length));
            }
        }

        private void ParseKeywords(string text)
        {
            MatchCollection matches = Regex.Matches(text, keywordsPattern);
            foreach (Match m in matches)
            {
                Tokens.Add(new Token(TokenType.Keyword, m.Index, m.Length));
            }
        }

        private void InitLibrary()
        {
            library =  new Dictionary<string, TokenType>()
            {
                { "if", TokenType.Keyword },
                { "else", TokenType.Keyword },
                { "for", TokenType.Keyword },
                { "class", TokenType.Keyword},
                { "deinit", TokenType.Keyword},
                { "enum", TokenType.Keyword},
                { "extension", TokenType.Keyword},
                { "func", TokenType.Keyword},
                { "import", TokenType.Keyword},
                { "init", TokenType.Keyword},
                { "inout", TokenType.Keyword},
                { "internal", TokenType.Keyword},
                { "let", TokenType.Keyword},
                { "operator", TokenType.Keyword},
                { "private", TokenType.Keyword},
                { "protocol", TokenType.Keyword},
                { "public", TokenType.Keyword},
                { "static", TokenType.Keyword},
                { "struct", TokenType.Keyword},
                { "subscript", TokenType.Keyword},
                { "typealias", TokenType.Keyword},
                { "var", TokenType.Keyword},
                { "break", TokenType.Keyword},
                { "case", TokenType.Keyword},
                { "continue", TokenType.Keyword},
                { "default", TokenType.Keyword},
                { "do", TokenType.Keyword},
                { "fallthrough", TokenType.Keyword},
                { "guard", TokenType.Keyword},
                { "in", TokenType.Keyword},
                { "repeat", TokenType.Keyword},
                { "return", TokenType.Keyword},
                { "switch", TokenType.Keyword},
                { "where", TokenType.Keyword},
                { "while", TokenType.Keyword},
                { "as", TokenType.Keyword},
                { "catch", TokenType.Keyword},
                { "dynamicType", TokenType.Keyword},
                { "false", TokenType.Keyword},
                { "is", TokenType.Keyword},
                { "nil", TokenType.Keyword},
                { "rethrows", TokenType.Keyword},
                { "super", TokenType.Keyword},
                { "self", TokenType.Keyword},
                { "throw", TokenType.Keyword},
                { "throws", TokenType.Keyword},
                { "true", TokenType.Keyword},
                { "try", TokenType.Keyword},
                { "__COLUMN__", TokenType.Keyword},
                { "__FILE__", TokenType.Keyword},
                { "__FUNCTION__", TokenType.Keyword},
                { "__LINE__", TokenType.Keyword},
                { "Self", TokenType.Keyword},
                { "_", TokenType.Keyword},
                { "associativity", TokenType.Keyword},
                { "convenience", TokenType.Keyword},
                { "dynamic", TokenType.Keyword},
                { "didSet", TokenType.Keyword},
                { "final", TokenType.Keyword},
                { "get", TokenType.Keyword},
                { "infix", TokenType.Keyword},
                { "indirect", TokenType.Keyword},
                { "lazy", TokenType.Keyword},
                { "left", TokenType.Keyword},
                { "mutating", TokenType.Keyword},
                { "nonmutating", TokenType.Keyword},
                { "override", TokenType.Keyword},
                { "postfix", TokenType.Keyword},
                { "precedence", TokenType.Keyword},
                { "prefix", TokenType.Keyword},
                { "Protocol", TokenType.Keyword},
                { "required", TokenType.Keyword},
                { "unowned", TokenType.Keyword},
                { "weak", TokenType.Keyword},
                { "willSet", TokenType.Keyword}
            };

            keywordsPattern = "\\b(";
            foreach (string key in library.Keys)
            {
                keywordsPattern += key + "|";
            }

            keywordsPattern = keywordsPattern.Remove(keywordsPattern.Length - 1);
            keywordsPattern += ")\\b";
        }
    }
}
