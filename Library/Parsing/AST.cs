namespace GSharpProject
{
    class ASTree
    {
        public ASTree(GSharpExpression root, Token endOfLineToken)
        {
            Root = root;
            EndOfLineToken = endOfLineToken;
        }
        public GSharpExpression Root { get; }
        public Token EndOfLineToken { get; }
        public static ASTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();  
        }
    }
}