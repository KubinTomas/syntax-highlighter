using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.TokenRecognizers
{
    public class CSharpTokenRecognizer : AbstractTokenRecognizer
    {
        public static readonly string[] CSharpKeywords = "abstract add as ascending async await base bool break by byte case catch char checked class const continue decimal default delegate descending do double dynamic else enum equals explicit extern false finally fixed float for foreach from get get; global goto group if implicit in int interface internal into is join let lock long namespace new null object on operator orderby out override params partial private protected public readonly ref remove return sbyte sealed select set set; short sizeof stackalloc static string struct switch this throw true try typeof uint ulong unchecked unsafe ushort using value var virtual void Task volatile where while yield".Split(' ');
        public static readonly string[] CSharpSeparators = "( ) { } ; : ,".Split(' ');
        public static readonly string[] CSharpOperators = "() [] . ?. ?[] :: + - * & && = == += -= *= &= -> < >  ?? >= <= &lt; &gt; &le; &ge;".Split(' ');
        public static readonly string[] CSharpCommentLiterals = "// /*  */".Split(' ');

        public CSharpTokenRecognizer()
            : base
            (CSharpKeywords,
            CSharpSeparators,
            CSharpOperators,
            CSharpCommentLiterals)
        {}
    }
}
