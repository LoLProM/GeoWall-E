namespace Hulk.Biblioteca.Semantic
{
    internal sealed class Semantic_LiteralExpression : Semantic_Expression
    {
        public Semantic_LiteralExpression(object value)
        {
            Value = value;
        }
        public object Value { get; private set; }
        public SemanticType Kind => SemanticType.LiteralExpression;
        public override GSharpTypes Types => GetGSharpTypes();

        private GSharpTypes GetGSharpTypes()
        {
            switch(Value)
            {
                case bool:
                return GSharpTypes.Boolean;
                case double:
                return GSharpTypes.Number;
                case string:
                return GSharpTypes.String;
                default:
                return GSharpTypes.Identifier;
            }
        }
    }
}