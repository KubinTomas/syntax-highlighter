using SyntaxHighlighter.Filters;
using SyntaxHighlighter.Formatter;
using SyntaxHighlighter.Interfaces;
using SyntaxHighlighter.Tokenizers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter
{
    class Program
    {
        static void Main(string[] args)
        {
            //https://www.freeformatter.com/java-dotnet-escape.html#ad-output
            string testData = "            formater.Run(); \r\n            \r\n        }\r\n    }\r\n}\r\n";
            ISHTokenizer tokenizer = new CSharpSHTokenizer();
            tokenizer.SetInput(new StringReader(testData));

            ISHFilter tokenFilter = new CSharpTokenFilter(tokenizer.GetTokenRecognizer());
            tokenFilter.SetSource(tokenizer);
            var revisedTokens = tokenFilter.ReviseTokens();

            ISHFormater formater = new HtmlSHFormatter(); // vytvoříme formátor
            formater.SetSource(revisedTokens);
            //formater.SetOutput(Console.Out);
            formater.SetOutput(new StreamWriter("formated.txt"));
            formater.Run(); 
            
        }
    }
}
