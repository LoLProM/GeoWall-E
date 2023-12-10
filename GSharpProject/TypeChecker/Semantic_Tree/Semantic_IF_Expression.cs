
namespace Hulk.Biblioteca.Semantic
{
    internal sealed class Semantic_IF_Expression : Semantic_Expression
    {
        public Semantic_IF_Expression(Semantic_Expression condition, Semantic_Expression ifBody, Semantic_Expression? elseBody)
        {
            Condition = condition;
            IfBody = ifBody;
            ElseBody = elseBody;
        }

        public SemanticType Kind => SemanticType.IfExpression;

        public Semantic_Expression Condition { get; }
        public Semantic_Expression IfBody { get; }
        public Semantic_Expression? ElseBody { get; }
        public override GSharpTypes Types => IfBody.Types;
    }
}