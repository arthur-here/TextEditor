using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TextEditor
{   
    /// <summary>
    /// Enumeration that represents all type of available tokens: Comment, Number, Keyword.
    /// </summary>
    public enum TokenType
    {
        Comment, Number, Keyword, String
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

        /// <summary>
        /// Gets <see cref="SolidColorBrush"/> of this token.
        /// </summary>
        public Brush ColorBrush
        {
            get
            {
                Color color;
                switch (this.TokenType)
                {
                    case TokenType.Keyword:
                        color = Color.FromRgb(251, 222, 45);
                        break;
                    case TokenType.Comment:
                        color = Color.FromRgb(174, 174, 174);
                        break;
                    case TokenType.Number:
                        color = Color.FromRgb(216, 250, 60);
                        break;
                    case TokenType.String:
                        color = Color.FromRgb(255, 165, 0);
                        break;
                    default:
                        color = Color.FromRgb(230, 230, 230);
                        break;
                }

                return new SolidColorBrush(color);
            }
        }
    }
}
