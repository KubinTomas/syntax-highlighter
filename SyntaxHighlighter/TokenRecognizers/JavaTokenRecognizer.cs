using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter.TokenRecognizers
{
    public class JavaTokenRecognizer : AbstractTokenRecognizer
    {
        public static readonly string[] JavaKeywords = "abstract continue for new switch assert default goto package synchronized boolean do if private this break double implements protected throw byte else import public throws case enum instanceof return transient catch extends int short try char final interface static void class finally long strictfp volatile const float native super while".Split(' ');
        public static readonly string[] JavaSeparators = "( ) { } ; : ,".Split(' ');
        public static readonly string[] JavaOperators = "() [] . ?. ?[] :: + - * & && = == += -= *= &= -> < >  ?? >= <= &lt; &gt; &le; &ge;".Split(' ');
        public static readonly string[] JavaCommentLiterals = "// /*  */".Split(' ');

        public JavaTokenRecognizer()
            : base
            (JavaKeywords,
            JavaSeparators,
            JavaOperators,
            JavaCommentLiterals)
        { }
    }
}
