using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.TokenRecognizers
{

    public abstract class AbstractTokenRecognizer
    {
        protected readonly string[] Keywords;
        protected readonly string[] Separators;
        protected readonly string[] Operators;
        protected readonly string[] CommentLiterals;

        public AbstractTokenRecognizer()
        {}

        public AbstractTokenRecognizer(string[] keywords, string[] separators, string[] operators)
        {
            Keywords = keywords;
            Separators = separators;
            Operators = operators;
        }
        public AbstractTokenRecognizer(string[] keywords, string[] separators, string[] operators, string[] commentLiterals) : this(keywords, separators, operators)
        {
            CommentLiterals = commentLiterals;
        }
        public virtual TokenType GetTokenType(String tokenContent)
        {
          
            if (IsStringInArray(tokenContent, Keywords)) return TokenType.KEY_WORD;
            if (IsStringInArray(tokenContent, Separators)) return TokenType.SEPARATOR;
            if (IsStringInArray(tokenContent, Operators)) return TokenType.OPERATOR;
            if (IsStringInArray(tokenContent, CommentLiterals) || IsComment(tokenContent)) return TokenType.COMMENT;
            if (string.IsNullOrWhiteSpace(tokenContent)) return TokenType.WHITE_SPACES;
            if (IsIntLiteral(tokenContent)) return TokenType.INT_LITERAL;
            if (IsFloatLiteral(tokenContent)) return TokenType.FLOAT_LITERAL;
            if (IsCharLiteral(tokenContent)) return TokenType.CHAR_LITERAL;
            if (IsStringLiteral(tokenContent)) return TokenType.STRING_LITERAL;

            return TokenType.IDENTIFIER;
        }
        protected virtual bool IsIntLiteral(string tokenContent)
        {
            return int.TryParse(tokenContent, out int n);
        }
        public virtual bool IsFloatLiteral(string tokenContent)
        {
            return float.TryParse(tokenContent, out float n);
        }
        protected virtual bool IsCharLiteral(string tokenContent)
        {
            return tokenContent.Length > 2 && tokenContent.ElementAt(0) == '\'' && tokenContent.ElementAt(tokenContent.Length - 1) == '\'';
        }
        protected virtual bool IsStringLiteral(string tokenContent)
        {
            return tokenContent.Length > 2 && tokenContent.ElementAt(0) == '\"' && tokenContent.ElementAt(tokenContent.Length - 1) == '\"';
        }
        private bool IsStringInArray(string stringToCheck, string[] stringArray)
        {
            foreach (var stringInArray in stringArray)
            {
                if (Equals(stringToCheck, stringInArray)) return true;
            }
            return false;
        }
        private bool IsComment(string tokenContent)
        {
            return (tokenContent.Length > 2 && tokenContent.ElementAt(0) == '/' && tokenContent.ElementAt(1) == '/');
        }
    }
}
