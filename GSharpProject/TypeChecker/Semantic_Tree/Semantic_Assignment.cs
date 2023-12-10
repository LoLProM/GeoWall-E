namespace Hulk.Biblioteca.Semantic
{
    internal sealed class Semantic_AsignacionVariable : Semantic_Expression
    {
        public Semantic_AsignacionVariable(string identifier, Semantic_Expression expression)
        {
            Identifier = identifier;
            Expression = expression;
        }
        public string Identifier { get; }
        public Semantic_Expression Expression { get; }
        public SemanticType Kind => SemanticType.AsignacionVariable;
        public override GSharpTypes Types => Expression.Types;
    }
}