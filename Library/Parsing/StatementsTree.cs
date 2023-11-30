using System.Collections.Generic;

namespace GSharpProject
{
    static class StatementsTree
    {
        public static List<GSharpExpression> Create(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();  
        }
    }
}