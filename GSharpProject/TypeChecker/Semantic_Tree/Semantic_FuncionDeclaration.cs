
namespace Hulk.Biblioteca.Semantic
{
    internal class Semantic_FuncionDeclaration : Semantic_Expression
    {
        public Semantic_FuncionDeclaration(string functionName, List<string> parameters, Semantic_Expression functionBody)
        {
            FunctionName = functionName;
            Parameters = parameters;
            FunctionBody = functionBody;
        }
        public override GSharpTypes Types => GSharpTypes.FunctionDeclaration;
        public SemanticType Kind => SemanticType.FuncionDeclaration;
        public string FunctionName { get; }
        public  List<string> Parameters{ get; }
        public Semantic_Expression FunctionBody { get; }
    }
}