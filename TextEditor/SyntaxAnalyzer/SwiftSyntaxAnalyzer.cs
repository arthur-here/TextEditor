using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextEditor
{
    /// <summary>
    /// Concrete implementation of Swift syntax analyzer.
    /// </summary>
    public class SwiftSyntaxAnalyzer : ISyntaxAnalyzer
    {
        private List<string> library;
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
        public void Parse(ITextEditorDocument document)
        {
            if (document == null)
            {
                return;
            }

            this.Tokens.Clear();

            this.ParseKeywords(document.Text);
            this.ParseNumbers(document.Text);
            this.ParseStrings(document.Text);
            this.ParseComments(document.Text);
        }

        private void ParseComments(string text)
        {
            string pattern = @"(?s:/\*((?!\*/).)*\*/)";
            MatchCollection matches = Regex.Matches(text, pattern);
            foreach (Match m in matches)
            {
                this.Tokens.Add(new Token(TokenType.Comment, m.Index, m.Length));
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
            MatchCollection matches = Regex.Matches(text, this.keywordsPattern);
            foreach (Match m in matches)
            {
                this.Tokens.Add(new Token(TokenType.Keyword, m.Index, m.Length));
            }
        }

        private void ParseStrings(string text)
        {
            string pattern = "\"[^\"\\\r\n]*(?:\\.[^\"\\\r\n]*)*\"";
            MatchCollection matches = Regex.Matches(text, pattern);
            foreach (Match m in matches)
            {
                this.Tokens.Add(new Token(TokenType.String, m.Index, m.Length));
            }
        }

        private void ParseNumbers(string text)
        {
            string pattern = "\\b(\\d+.?\\d*)\\b";
            MatchCollection matches = Regex.Matches(text, pattern);
            foreach (Match m in matches)
            {
                this.Tokens.Add(new Token(TokenType.Number, m.Index, m.Length));
            }
        }

        private void InitLibrary()
        {
            this.library = new List<string>()
            {
                "if", "else", "for", "class", "deinit", "enum", "extension", "func", "import", "init", "inout", "internal", "let",
                "operator", "private", "protocol", "public", "static", "struct", "subscript", "typealias", "var", "break", "case",
                "continue", "default", "do", "fallthrough", "guard", "in", "repeat", "return", "switch", "where", "while", "as",
                "catch", "dynamicType", "false", "is", "nil", "rethrows", "super", "self", "throw", "throws", "true", "try", "__COLUMN__",
                "__FILE__", "__FUNCTION__", "__LINE__", "Self", "_", "associativity", "convenience", "dynamic", "didSet", "final",
                "get", "infix", "indirect", "lazy", "left", "mutating", "nonmutating", "override", "postfix", "precedence", "prefix",
                "Protocol", "required", "unowned", "weak", "willSet"
            };

            this.keywordsPattern = "\\b(";
            foreach (string key in this.library)
            {
                this.keywordsPattern += key + "|";
            }

            this.keywordsPattern = this.keywordsPattern.Remove(this.keywordsPattern.Length - 1);
            this.keywordsPattern += ")\\b";
        }
    }
}
