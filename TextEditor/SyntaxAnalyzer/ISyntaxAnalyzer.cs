using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextEditor
{
    /// <summary>
    /// Provides interface to syntax analyzers.
    /// </summary>
    public interface ISyntaxAnalyzer
    {
        /// <summary>
        /// Gets list of parsed tokens from text.
        /// </summary>
        List<Token> Tokens { get; }

        /// <summary>
        /// Parse document text and saves result in Tokens.
        /// </summary>
        /// <param name="document">Document to analyze.</param>
        void Parse(ITextEditorDocument document);
    }
}
