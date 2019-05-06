using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.Formatter
{
    // ladící formátovaní (vypisuje jen poslupnost ladící representace tokenů tj. jméno a typ)
   public class DebugSHFormater : AbstractSHFormatter
    {
        public override string Header()
        {
            return ""; // není potřeba žádné hlavičky
        }

        public override string Footer()
        {
            return ""; // ani patičky
        }

        public override string Format(Token token)
        {
            return token.ToString() + "\n"; // vypíše se informace o tokenu nálsedovaná 
        }
    }
}
