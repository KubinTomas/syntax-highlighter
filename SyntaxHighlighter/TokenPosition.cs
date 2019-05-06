using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyntaxHighlighter
{
    public class TokenPosition
    {
        public int RowIndex { get; private set; } 

        public int ColIndex { get; private set; }

        public TokenPosition(int rowIndex, int colIndex)
        {
            RowIndex = rowIndex;
            ColIndex = colIndex;
        }
    }
}
