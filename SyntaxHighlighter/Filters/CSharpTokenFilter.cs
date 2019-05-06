using SyntaxHighlighter.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyntaxHighlighter.TokenRecognizers;

namespace SyntaxHighlighter.Filters
{
    public class CSharpTokenFilter : ISHFilter
    {
        private List<Token> source;
        private AbstractTokenRecognizer tokenRecognizer;

        public Token Current { get; protected set; }

        private string[] separableSymbols = "( ) { } ; < >".Split(' ');

        public CSharpTokenFilter(AbstractTokenRecognizer tokenRecognizer)
        {
            this.tokenRecognizer = tokenRecognizer;
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return this;
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// iterate over source tokens and try to split token to only and only one relative token
        /// if(x -> Token IF, Token (, Token x
        /// </summary>
        /// <returns></returns>
        private List<Token> GetSeparatedToken()
        {
            var separatedSource = new List<Token>();

            foreach (var token in source)
            {
                int indexer = 0;
                var tokenText = token.Text;

                bool increaseIndexer = true;
                while (tokenText.Length != indexer)
                {
                    increaseIndexer = true;
                    if (separableSymbols.Contains(tokenText[indexer].ToString()) && !CSharpTokenRecognizer.CSharpOperators.Contains(tokenText) && !tokenRecognizer.IsFloatLiteral(tokenText))
                    {
                        var symbol = tokenText[indexer];
                        var splitedTokenText = tokenText.Split(symbol);
                        int firstIndex = tokenText.IndexOf(symbol);

                        string firstPart = tokenText.Substring(0, firstIndex);
                        string secondPart = tokenText.Substring(firstIndex + 1);

                        splitedTokenText[0] = firstPart;
                        splitedTokenText[1] = secondPart;

                        //Check if identifier can be separated by dots  and check of method nad property

                        if (!string.IsNullOrEmpty(splitedTokenText[0]))
                            separatedSource.Add(new Token(splitedTokenText[0], tokenRecognizer.GetTokenType(splitedTokenText[0]), token.TokenPosition));
                        separatedSource.Add(new Token(symbol.ToString(), tokenRecognizer.GetTokenType(symbol.ToString()), token.TokenPosition));

                        tokenText = splitedTokenText[1];

                        indexer = 0;
                        increaseIndexer = false;
                    }
                    if (tokenText.Length - 1 == indexer && !string.IsNullOrEmpty(tokenText))
                    {
                        separatedSource.Add(new Token(tokenText, tokenRecognizer.GetTokenType(tokenText), token.TokenPosition));
                        break;
                    }

                    if (increaseIndexer)
                        indexer++;
                }
                //if(token.Type == TokenType.IDENTIFIER)
                //{
                //    var previousNonSpaceTokenIndex = source.IndexOf(token) - 1;

                //    var canSeparateTokens = true;


                //    var previousToken = source.ElementAt(previousNonSpaceTokenIndex);

                //}
            }
            separatedSource = FixIdentifiers(separatedSource);

            return separatedSource;
        }
        private List<Token> FixIdentifiers(List<Token> separatedSource)
        {
            var newSource = new List<Token>();

            var usingIsProccessed = false;
            var namespaceIsProccessed = false;

            foreach (var token in separatedSource)
            {
                if (token.Text == "using") usingIsProccessed = true;
                if (token.Text == ";") usingIsProccessed = false;

                if (token.Text == "namespace") namespaceIsProccessed = true;
                if (token.Text == "{") namespaceIsProccessed = false;

                if (token.Type == TokenType.IDENTIFIER && token.Text.Contains(".") && !usingIsProccessed && !namespaceIsProccessed)
                {
                    var tokenIndex = separatedSource.IndexOf(token) + 1;
                    var nextToken = separatedSource.ElementAt(tokenIndex);

                    var split = token.Text.Split('.');

                    var newToken = new Token(split[0], TokenType.IDENTIFIER, token.TokenPosition);
                    newSource.Add(new Token(split[0], TokenType.IDENTIFIER, token.TokenPosition));

                    newSource.Add(new Token(".", TokenType.SEPARATOR, token.TokenPosition));

                    for (int i = 1; i < split.Length - 1; i++)
                    {
                        newToken = new Token(split[i], TokenType.IDENTIFIER_PROPERTY, token.TokenPosition);
                        newSource.Add(newToken);
                        newSource.Add(new Token(".", TokenType.SEPARATOR, token.TokenPosition));

                    }

                    if (nextToken.Text.Contains("("))
                        newToken = new Token(split[split.Length - 1], TokenType.IDENTIFIER_METHOD, token.TokenPosition);
                    else
                        newToken = new Token(split[split.Length - 1], TokenType.IDENTIFIER_PROPERTY, token.TokenPosition);

                    newSource.Add(newToken);
                }
                else
                {
                    newSource.Add(token);
                }
            }
            return newSource;

        }
        public List<Token> ReviseTokens()
        {
            source = GetSeparatedToken();

            foreach (var currentToken in source)
            {
              //  if (!currentToken.ShouldBeModified) continue;

                if (currentToken.Type == TokenType.COMMENT)
                {
                    SetCommentUntilEndOfLine(currentToken);
                    continue;
                }

                if (currentToken.Type == TokenType.OPERATOR) continue;

                CheckOrReplaceMethodIdentifier(currentToken);
                CheckOrReplacePropertyIdentifier(currentToken);
                CheckOrReplaceIdentifier(currentToken);
                CheckOrReplaceClassOrNsIdentifier(currentToken);
                CheckOrReplaceMethodIdentifierArgument(currentToken);
                IsTokenInGenericOperators(currentToken);
            }

            return source;
        }
        private void IsTokenInGenericOperators(Token currentToken)
        {
            var previousNonSpaceTokenIndex = source.IndexOf(currentToken) - 1;

            if (previousNonSpaceTokenIndex < 0) return;

            var previousToken = source.ElementAt(previousNonSpaceTokenIndex);



            if (previousToken.Text == "<")
            {
                var type = tokenRecognizer.GetTokenType(currentToken.Text);

                if (type == TokenType.IDENTIFIER) type = TokenType.IDENTIFIER_CLASS;

                currentToken.ChangeTokenType(type);
            }
        }
        private void SetCommentUntilEndOfLine(Token currentToken)
        {
            var index = source.IndexOf(currentToken);

            for (int i = index; i < source.Count; i++)
            {
                var token = source.ElementAt(i);
                if (currentToken.TokenPosition.RowIndex != token.TokenPosition.RowIndex) return;

                token.ChangeTokenType(TokenType.COMMENT);
            }
        }
        private void CheckOrReplaceMethodIdentifierArgument(Token currentToken)
        {
            var previous1NonSpaceTokenIndex = source.IndexOf(currentToken) - 2;
            var previous2NonSpaceTokenIndex = source.IndexOf(currentToken) - 1;

            if (previous1NonSpaceTokenIndex < 0) return;

            var previous1Token = source.ElementAt(previous1NonSpaceTokenIndex);
            var previous2Token = source.ElementAt(previous2NonSpaceTokenIndex);

            if (previous1Token.Type == TokenType.IDENTIFIER_METHOD && previous2Token.Text == "(" && currentToken.Type == TokenType.IDENTIFIER)
                currentToken.ChangeTokenType(TokenType.IDENTIFIER_CLASS);
        }
        /// <summary>
        /// Check if previous Token is new then current token is IDENTIFIER_CLASS, cuz we are making a new class instance
        /// Same for namespace or using
        /// </summary>
        /// <param name="currentToken"></param>
        private void CheckOrReplaceClassOrNsIdentifier(Token currentToken)
        {
            var previousNonSpaceTokenIndex = source.IndexOf(currentToken) - 2;

            if (previousNonSpaceTokenIndex < 0) return;

            var previousToken = source.ElementAt(previousNonSpaceTokenIndex);

            if (previousToken.Text == "new") currentToken.ChangeTokenType(TokenType.IDENTIFIER_CLASS);
            if (previousToken.Text == "class") currentToken.ChangeTokenType(TokenType.IDENTIFIER_CLASS);
            if (previousToken.Text == "namespace" || previousToken.Text == "using") currentToken.ChangeTokenType(TokenType.IDENTIFIER_NS);
        }
        private void CheckOrReplaceMethodIdentifier(Token currentToken)
        {
            //check basic non return void method
            CheckIfIsNonReturnMethodName(currentToken);

            //check if is return method
            CheckIfIReturnMethodName(currentToken);
        }
        private void CheckIfIReturnMethodName(Token currentToken)
        {
            var previousNonSpaceTokenIndex = source.IndexOf(currentToken) - 2;

            if (previousNonSpaceTokenIndex < 0) return;

            var previousToken = source.ElementAt(previousNonSpaceTokenIndex);

            var nextNonSpaceTokenIndex = source.IndexOf(currentToken) + 2;

            if (nextNonSpaceTokenIndex >= source.Count) return;

            var nextToken = source.ElementAt(nextNonSpaceTokenIndex);

            if (previousToken.Type == TokenType.IDENTIFIER && nextToken.Text.Contains("(")) currentToken.ChangeTokenType(TokenType.IDENTIFIER_METHOD);
        }
        private void CheckIfIsNonReturnMethodName(Token currentToken)
        {
            var previousNonSpaceTokenIndex = source.IndexOf(currentToken) - 2;

            if (previousNonSpaceTokenIndex < 0) return;

            var previousToken = source.ElementAt(previousNonSpaceTokenIndex);

            if (previousToken.Text == "void" || previousToken.Text == "Task") currentToken.ChangeTokenType(TokenType.IDENTIFIER_METHOD);
        }
        private void CheckOrReplacePropertyIdentifier(Token currentToken)
        {
            var previousNonSpaceTokenIndex = source.IndexOf(currentToken) - 2;

            if (previousNonSpaceTokenIndex < 0) return;

            var previousToken = source.ElementAt(previousNonSpaceTokenIndex);

            var nextNonSpaceTokenIndex = source.IndexOf(currentToken) + 2;

            if (nextNonSpaceTokenIndex >= source.Count) return;

            var nextToken = source.ElementAt(nextNonSpaceTokenIndex);

            if (previousToken.Text != ":" && previousToken.Text != "class" && previousToken.Type != TokenType.IDENTIFIER_CLASS && currentToken.Type == TokenType.IDENTIFIER && nextToken.Text == "{") currentToken.ChangeTokenType(TokenType.IDENTIFIER_PROPERTY);
        }
        private void CheckOrReplaceIdentifier(Token currentToken)
        {
            var nextNonSpaceTokenIndex = source.IndexOf(currentToken) + 2;

            if (nextNonSpaceTokenIndex >= source.Count) return;

            var nextToken = source.ElementAt(nextNonSpaceTokenIndex);

            var next1NonSpaceTokenIndex = source.IndexOf(currentToken) + 3;
            var next2NonSpaceTokenIndex = source.IndexOf(currentToken) + 4;

            if (next2NonSpaceTokenIndex >= source.Count) return;

            var next1Token = source.ElementAt(next1NonSpaceTokenIndex);
            var next2Token = source.ElementAt(next2NonSpaceTokenIndex);

            if (currentToken.Type == TokenType.IDENTIFIER && nextToken.Type == TokenType.IDENTIFIER && (next1Token.Text == ";" || next2Token.Text == "=")) currentToken.ChangeTokenType(TokenType.IDENTIFIER_CLASS);
        }
        //IDENTIFIER_CLASS,  // jediný povinný z úrovně 2 (jméno třídy v definici)
        //IDENTIFIER_NS, // jméno jmenného prostoru v konstrukcích using a namespace
        //IDENTIFIER_METHOD, //jméno nově definované metody
        //IDENTIFIER_PROPERTY // jméno nově definované vlastnosti

        public void SetSource(IEnumerable<Token> source)
        {
            this.source = source.ToList();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public List<TokenType> GetTokenTypesFromCurrentTokens()
        {
            ReviseTokens();

            var tokenTypes = new List<TokenType>();

            foreach (var token in source)
            {
                tokenTypes.Add(token.Type);
            }

            return tokenTypes;
        }
    }
}
