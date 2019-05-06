using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.TokenRecognizers
{
    public class TrivialTokenRecognizer : AbstractTokenRecognizer
    {
        public override TokenType GetTokenType(String tokenContent)
        {
            return TokenType.FLOAT_LITERAL;
        }
    }
}
