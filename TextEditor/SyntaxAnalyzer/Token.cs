using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{   
    /// <summary>
    /// Enumeration that represents all type of available tokens: Comment, Number, Keyword.
    /// </summary>
    public enum TokenType
    {
        Comment, Number, Keyword
    }

    /// <summary>
    /// Encapsulates information about syntax token in text.
    /// </summary>
    public class Token
    {
        private TokenType type;
        private int caretIndex;
        private int length;

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        /// <param name="type"><see cref="TokenType"/> of token.</param>
        /// <param name="caretIndex">Start position in text.</param>
        /// <param name="length">Length of token.</param>
        public Token(TokenType type, int caretIndex, int length)
        {
            this.type = type;
            this.caretIndex = caretIndex;
            this.length = length;
        }

        /// <summary>
        /// Gets <see cref="TokenType"/> of this token.
        /// </summary>
        public TokenType TokenType
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets or sets position of token in text.
        /// </summary>
        public int CaretIndex
        {
            get { return this.caretIndex; }
            set { this.caretIndex = value; }
        }

        /// <summary>
        /// Gets or sets length of token string.
        /// </summary>
        public int Length
        {
            get { return this.length; }
            set { this.length = value; }
        }
    }
}
