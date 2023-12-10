
using GSharpProject.Parsing;

namespace Hulk.Biblioteca.Semantic
{
    internal sealed class Semantic_LetIn : Semantic_Expression
    {
        public Semantic_LetIn(List<GSharpStatementsCollection> statements, Semantic_Expression inExpression)
        {
            Statements = statements;
            InExpression = inExpression;
        }
        public SemanticType Kind => SemanticType.LetInContext;
        public List<GSharpStatementsCollection> Statements { get; }
        public Semantic_Expression InExpression { get; }
        public override GSharpTypes Types => InExpression.Types;
    }
}