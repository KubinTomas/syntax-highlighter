using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SyntaxHighlighter.Filters;
using SyntaxHighlighter.Tokenizers;
using SyntaxHighlighter.Interfaces;
using System.IO;
using System.Collections.Generic;

namespace SyntaxHighlighter.UnitTests
{
    [TestClass]
    public class BasicCSharpTokenTests
    {
        [TestMethod]
        [DataRow(
            "var",
            new TokenType[] { TokenType.KEY_WORD }
            )]
        [DataRow(
            "if ( x &lt; 42 )",
            new TokenType[] { TokenType.KEY_WORD, TokenType.WHITE_SPACES, TokenType.SEPARATOR,
                TokenType.WHITE_SPACES, TokenType.IDENTIFIER, TokenType.WHITE_SPACES,
                TokenType.OPERATOR, TokenType.WHITE_SPACES,
                TokenType.INT_LITERAL, TokenType.WHITE_SPACES, TokenType.SEPARATOR }
            )]
        [DataRow(
            "using Microsoft.VisualStudio.TestTools.UnitTesting;",
            new TokenType[] { TokenType.KEY_WORD, TokenType.WHITE_SPACES, TokenType.IDENTIFIER_NS, TokenType.SEPARATOR }
            )]
        [DataRow(
            "private void DoIt(Token token) { int a = 2; }",
            new TokenType[] { TokenType.KEY_WORD, TokenType.WHITE_SPACES, TokenType.KEY_WORD,
            TokenType.WHITE_SPACES, TokenType.IDENTIFIER_METHOD,
            TokenType.SEPARATOR, TokenType.IDENTIFIER_CLASS, TokenType.WHITE_SPACES, TokenType.IDENTIFIER,
            TokenType.SEPARATOR, TokenType.WHITE_SPACES, TokenType.SEPARATOR, TokenType.WHITE_SPACES,
            TokenType.KEY_WORD, TokenType.WHITE_SPACES, TokenType.IDENTIFIER, TokenType.WHITE_SPACES, TokenType.OPERATOR,
            TokenType.WHITE_SPACES, TokenType.INT_LITERAL, TokenType.SEPARATOR, TokenType.WHITE_SPACES, TokenType.SEPARATOR}
            )]
        [DataRow(
            "var ahj = new Hi();",
            new TokenType[] { TokenType.KEY_WORD, TokenType.WHITE_SPACES, TokenType.IDENTIFIER,
            TokenType.WHITE_SPACES, TokenType.OPERATOR, TokenType.WHITE_SPACES, TokenType.KEY_WORD, TokenType.WHITE_SPACES,
            TokenType.IDENTIFIER_CLASS, TokenType.SEPARATOR, TokenType.SEPARATOR, TokenType.SEPARATOR}
            )]
        [DataRow(
            "var //test komentu\nint",
            new TokenType[] { TokenType.KEY_WORD, TokenType.WHITE_SPACES, TokenType.COMMENT,
            TokenType.COMMENT, TokenType.COMMENT, TokenType.WHITE_SPACES, TokenType.KEY_WORD}
            )]
        public void SHCsharpAreTokensWellConverted_TokensAreCorrect_ReturnsTrue(string inputData, TokenType[] expectedTokens)
        {
            //arange
            ISHTokenizer tokenizer = new CSharpSHTokenizer();
            ISHFilter tokenFilter = new CSharpTokenFilter(tokenizer.GetTokenRecognizer());

            //act
            tokenizer.SetInput(new StringReader(inputData));
            tokenFilter.SetSource(tokenizer);
            var tokenTypes = tokenFilter.GetTokenTypesFromCurrentTokens();

            //assert
            CollectionAssert.AreEqual(expectedTokens, tokenTypes);
        }
    }
}
//tokenizer.SetInput(new StringReader("if ( x &lt; 42 )"));
//tokenizer.SetInput(new StringReader("if(x &lt; 42){ int a = 2; }"));