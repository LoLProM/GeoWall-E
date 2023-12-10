namespace Hulk.Biblioteca.Semantic
{
    internal class Semantic_FunctionCall : Semantic_Expression
    {
        public Semantic_FunctionCall(string functionName,List<Semantic_Expression> arguments)
        {
            FunctionName = functionName;
            Arguments = arguments;
        }
        public override GSharpTypes Types => Arguments[0].Types;
        public SemanticType Kind => SemanticType.FunctionCall;
        public string FunctionName { get; }
        public List<Semantic_Expression> Arguments { get; }
    }
}