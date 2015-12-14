using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TextEditor
{
    public abstract class TextEditorSyntaxAnalyzer: ISyntaxAnalyzer
    {
        /// <summary>
        /// Gets list of parsed tokens from text.
        /// </summary>
        public List<Token> Tokens { get; protected set; }

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

        protected abstract void ParseKeywords(string text);

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
    }
}
