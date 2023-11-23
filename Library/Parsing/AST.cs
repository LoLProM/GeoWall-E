namespace GSharpProject
{
    class ASTree
    {
        public ASTree(GSharpExpression root)
        {
            Root = root;
        }
        public GSharpExpression Root { get; }
        public static ASTree Parse(string text)
        {
            var parser = new Parser(text);
            return parser.Parse();  
        }
    }
}