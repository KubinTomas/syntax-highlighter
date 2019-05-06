using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.Interfaces
{
    //rozhraní nepovinných filtrů tokenů
    public interface ISHFilter : IEnumerator<Token>, IEnumerable<Token>
    {
        void SetSource(IEnumerable<Token> source);

        List<Token> ReviseTokens();

        List<TokenType> GetTokenTypesFromCurrentTokens();
    }
}
