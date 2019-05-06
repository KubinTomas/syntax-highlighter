using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.Formatter
{
    public class HtmlSHFormatter : AbstractSHFormatter
    {
        private Dictionary<TokenType, String> tokenTypeCssDictionary;
        private int rowIndex;

        public HtmlSHFormatter()
        {
            InitCssDictionary();

            rowIndex = 0;
        }
        private void InitCssDictionary()
        {
            tokenTypeCssDictionary = new Dictionary<TokenType, String>();

            foreach (TokenType type in (TokenType[])Enum.GetValues(typeof(TokenType)))
            {
                tokenTypeCssDictionary.Add(type, type.ToString().ToLower());
            }
        }

        public override string Footer()
        {
            return "";
        }
        public int GetNumSubstringOccurrences(string text, string search)
        {
            int num = 0;
            int pos = 0;

            if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(search))
            {
                while ((pos = text.IndexOf(search, pos)) > -1)
                {
                    num++;
                    pos += search.Length;
                }
            }
            return num;
        }
        public override string Format(Token token)
        {
            int offsetMultiplicator = 5;
            var newLineAdded = false;
            var formatedToken = "<span class='" + GetCssClassRelativeToTokenType(token.Type) + "'>" + token.Text + "</span>";

            if (token.Type == TokenType.WHITE_SPACES)
            {
                if (token.Text.Contains("\n"))
                {
                    int whiteSpaceCount = 0;

                    for (int i = 0; i < token.Text.Length; i++)
                    {
                        if (token.Text[i] == ' ') whiteSpaceCount++;
                    }

                    int brCount = GetNumSubstringOccurrences(token.Text, "\n");
                    int start = -1;
                    int end = -1;

                    //REMOVE whitespaces from 0 to first \n occurence

                    //remove space between new lines - cuz that did a bug that move next token 
                    if (brCount >= 2)
                    {
                        for (int i = 0; i < token.Text.Length; i++)
                        {

                            if (token.Text[i] == '\n')
                            {
                                if (start == -1)
                                {
                                    start = i;
                                }else if (start != -1 && end == -1) end = i;
                                if (start != -1 && end != -1) break;
                            }
                        }

                    }
                    if (start != -1 && end != -1)
                        whiteSpaceCount = whiteSpaceCount - (end - start);

                    formatedToken = "<span style = padding-left:" + (whiteSpaceCount * offsetMultiplicator) + "px" + "></span>" + formatedToken;

                    for (int i = 0; i < brCount; i++)
                    {
                        formatedToken = "<br>" + formatedToken;
                    }
                    newLineAdded = true;
                }
                else
                {
                    var whiteSpaceCount = token.Text.Length;
                    var appendText = "";

                    for (int i = 0; i < whiteSpaceCount; i++)
                    {
                        appendText += "-";
                    }
                    //formatedToken += appendText;
                    formatedToken = "<span style = padding-left:" + (whiteSpaceCount * offsetMultiplicator) + "px" + "></span>" + formatedToken;
                }

            }


            //TODO ADD BR IF NEW ROW
            if (rowIndex != token.TokenPosition.RowIndex)
            {
                rowIndex = token.TokenPosition.RowIndex;
                //if (!newLineAdded)
                //    formatedToken += "<br>";
            }
            formatedTokens.Add(formatedToken);

            return formatedToken;
        }
        private string GetCssClassRelativeToTokenType(TokenType type)
        {
            if (!tokenTypeCssDictionary.ContainsKey(type)) return "undefined";

            return tokenTypeCssDictionary[type];
        }
        public override void Run()
        {
            formatedTokens.Add(Header());
            foreach (Token token in source)
            {
                Format(token);
            }
            formatedTokens.Add(Footer());
        }
        public override string Header()
        {
            rowIndex = 0;
            return "";
        }
    }
}
