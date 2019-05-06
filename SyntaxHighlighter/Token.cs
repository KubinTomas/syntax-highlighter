using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter
{
    public enum TokenType
    {
        //level 0 (jednoduché rozlišení mezi bílými znaky a slovy jazyka)
        SYMBOL,   // tento typ tokenu by se neměl ve výstupu skutečného tokenizátoru vyskytovat
        WHITE_SPACES,
        // level 1 (základní typy tokenů v C# a podobných jazycích)
        IDENTIFIER,  //libovolný identifikátor
        KEY_WORD,
        SEPARATOR,
        OPERATOR,
        INT_LITERAL,
        FLOAT_LITERAL,
        CHAR_LITERAL,
        STRING_LITERAL,
        COMMENT,
        // level 2 (detailnější typy identifikátorů, rozeznatelných jen podle okolních tokenů)
        IDENTIFIER_CLASS,  // jediný povinný z úrovně 2 (jméno třídy v definici)
        IDENTIFIER_NS, // jméno jmenného prostoru v konstrukcích using a namespace
        IDENTIFIER_METHOD, //jméno nově definované metody
        IDENTIFIER_PROPERTY // jméno nově definované vlastnosti
    }
    public class Token
    {
        public string Text { get; private set; } // text tokenu
        public TokenType Type { get; private set; } // typ tokenu
        public TokenPosition TokenPosition { get; private set; }

        public Token(string text, TokenType type, TokenPosition tokenPosition)
        {
            Text = text;
            Type = type;
            TokenPosition = tokenPosition;
        }
        public void ChangeTokenType(TokenType type)
        {
            Type = type;
        }

        public override string ToString() // určeno pro ladící účely nikoliv fromátování
        {
            return string.Format("|{0}| ({1})", Text, Type);
        }
    }
}
