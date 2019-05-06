using SyntaxHighlighter.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.Formatter
{
    // abstraktní definice formátovače (zajištuje především interakci s proudem)
    public abstract class AbstractSHFormatter : ISHFormater
    {
        TextWriter output;
        protected IEnumerable<Token> source;

        protected List<string> formatedTokens;

        public void SetSource(IEnumerable<Token> source)
        {
            this.source = source;

            formatedTokens = new List<String>();
        }

        public void SetOutput(TextWriter output)
        {
            this.output = output;
        }
        
        public virtual void Run()
        {
            output.Write(Header()); //vypiš statickou hlavičku (např. neměnný počátek HTML)
            foreach (Token token in source)
            {
                output.Write(Format(token)); // vypiš naformátovaný token
            }
            output.Write(Footer()); //a fixní patičku např. </body></html>

            output.Close();
        }

        // všechny metody volané z  `run` je nutno definovat v odvozených třídách
        public abstract string Header();
        public abstract string Format(Token token);
        public abstract string Footer();

        public IEnumerable<string> GetFormatedTokens()
        {
            return formatedTokens;
        }
    }
}
