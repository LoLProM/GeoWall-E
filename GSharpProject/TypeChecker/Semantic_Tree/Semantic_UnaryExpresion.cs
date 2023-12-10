using GSharpProject;

namespace Hulk.Biblioteca.Semantic
{
    internal  class Semantic_UnaryExpression : Semantic_Expression
    {
        public Semantic_UnaryExpression(Semantic_UnaryOperator unaryOperator, Semantic_Expression operand)
        {
            UnaryOperator = unaryOperator;
            Operand = operand;
            
        }
        public Semantic_UnaryOperator UnaryOperator { get; }
        public Semantic_Expression Operand { get; }
        public SemanticType Kind => SemanticType.UnaryExpression;
        public override GSharpTypes Types => Operand.Types;
    }
    internal sealed class Semantic_UnaryOperator
    {
        public Semantic_UnaryOperator(TokenType tokenType,
        Semantic_UnaryOperatorType operatorType, GSharpTypes operand) : this(tokenType, operatorType, operand, operand)
        {
        }
        public Semantic_UnaryOperator(TokenType tokenType, Semantic_UnaryOperatorType operatorType, GSharpTypes operand, GSharpTypes returnType)
        {
            TokenTypes = tokenType;
            OperatorType = operatorType;
            Operand = operand;
            ReturnType = returnType;
        }

        public TokenType TokenTypes { get; }
        public Semantic_UnaryOperatorType OperatorType { get; }
        public GSharpTypes Operand { get; }
        public GSharpTypes ReturnType { get; }
        private static readonly Semantic_UnaryOperator[] operators =
        {
            new(TokenType.NotToken,Semantic_UnaryOperatorType.LogicalNot,GSharpTypes.Boolean),
            new(TokenType.PlusToken,Semantic_UnaryOperatorType.Identity,GSharpTypes.Number),
            new(TokenType.MinusToken,Semantic_UnaryOperatorType.Negation,GSharpTypes.Number),
        };
        public static Semantic_UnaryOperator Semantic_Parse_UO(TokenType tokenType, GSharpTypes operatorType)
        {
            foreach (var a in operators)
            {
                if (a.TokenTypes == tokenType && a.Operand == operatorType)
                    return a;
            }
            return null!;
        }
    }
}