using SyntaxHighlighter.TokenRecognizers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.Interfaces
{
    //rozhraní tokenizeru (vrací jednotlivé tokeny jako v podobě lenivého [enumerovatelného]enumerátoru)
    public interface ISHTokenizer : IEnumerator<Token>, IEnumerable<Token>
    {
        void SetInput(TextReader input);

        AbstractTokenRecognizer GetTokenRecognizer();
    }

}
