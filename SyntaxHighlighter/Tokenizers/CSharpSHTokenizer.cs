using SyntaxHighlighter.TokenRecognizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.Tokenizers
{
    public class CSharpSHTokenizer : AbstractSHTokenizer
    {
       

        public CSharpSHTokenizer() : base(new CSharpTokenRecognizer())
        {

        }

        public override bool MoveNext()
        {
            int input = reader.Peek(); // vrátí znak z proudu, ale nepřečte ho tj. neodstraní z proudu
            if (input == -1) // pokud je dosažen konec proudu
                return false; // vrátí příznak dosažení konce iterátoru
            char c = Convert.ToChar(input); // nyní už je bezpečné převést data (v podobě číselné representace znaku) na znak
            if (char.IsWhiteSpace(c))
            { // pokud je to bílý znak
                string text = ReadWhileInClass(lc => char.IsWhiteSpace(lc)); // nači a vrať všechny následující bílé znaky

                //check if we should go on new line
                if (text.Contains("\n")) RowIndex++;
                
                Current = new Token(text, TokenType.WHITE_SPACES, new TokenPosition(RowIndex, 0)); // a nastav příslušný token
            }
            else
            {
                string text = ReadWhileInClass(lc => !char.IsWhiteSpace(lc)); // jinak čti nemezerové znaky
                Current = new Token(text, tokenRecognizer.GetTokenType(text), new TokenPosition(RowIndex, 0)); // a nastav příslušný token
            }
            return true; // signalizuje, že ještě nebyl dosažen konec (a token ve vlastnosti `Current` je platný
        }

        //čte dokud funkce `classifier` předaná jako parametr vrací true
        private string ReadWhileInClass(Func<char, bool> classifier)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int input;
            while ((input = reader.Peek()) != -1)
            {
                char c = Convert.ToChar(input);
                if (classifier(c))
                {
                    stringBuilder.Append(c);
                    reader.Read();
                }
                else
                {
                    break;
                }
            }
            return stringBuilder.ToString();
        }
    }
}
