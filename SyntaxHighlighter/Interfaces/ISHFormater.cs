using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.Interfaces
{
    public interface ISHFormater
    {
        /// <summary>
        /// Set Enumerable source of all tokens
        /// </summary>
        /// <param name="source"></param>
        void SetSource(IEnumerable<Token> source);
        void SetOutput(TextWriter output);
        /// <summary>
        /// Start formating of all toknes
        /// </summary>
        void Run();

        IEnumerable<String> GetFormatedTokens();
    }
}
