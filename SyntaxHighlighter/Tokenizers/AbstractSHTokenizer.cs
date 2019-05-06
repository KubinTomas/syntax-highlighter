using SyntaxHighlighter.Interfaces;
using SyntaxHighlighter.TokenRecognizers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.Tokenizers
{
    // základní implementace tokenizátoru zaměřená na podporu rozhraní enumerátoru
    public abstract class AbstractSHTokenizer : ISHTokenizer
    {
        public readonly AbstractTokenRecognizer tokenRecognizer;
        protected int RowIndex { get; set; }

        protected TextReader reader;
        public Token Current { get; protected set; }

        public AbstractSHTokenizer(AbstractTokenRecognizer tokenRecognizer)
        {
            this.tokenRecognizer = tokenRecognizer;
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        // jediná metoda kterou je nutno doimplementovat, přečte token z proudu a jeho representací 
        // (= instanci třídy vloží do vlastnosti Current)
        public abstract bool MoveNext();

        public void Dispose()
        {
          //  reader.Dispose();
        }

        public void Reset()
        {
            throw new NotImplementedException("Invalid method");
        }

        public void SetInput(TextReader reader)
        {
            RowIndex = 0;

            this.reader = reader;
        }

        public IEnumerator<Token> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this;
        }

        public AbstractTokenRecognizer GetTokenRecognizer()
        {
            return tokenRecognizer;
        }
    }
}
