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
    public class SwiftSyntaxAnalyzer : TextEditorSyntaxAnalyzer
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

        protected override void ParseKeywords(string text)
        {
            MatchCollection matches = Regex.Matches(text, this.keywordsPattern);
            foreach (Match m in matches)
            {
                this.Tokens.Add(new Token(TokenType.Keyword, m.Index, m.Length));
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
