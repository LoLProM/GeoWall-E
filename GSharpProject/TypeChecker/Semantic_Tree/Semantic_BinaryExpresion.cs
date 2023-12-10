
using GSharpProject;

namespace Hulk.Biblioteca.Semantic
{
    internal sealed class Semantic_BinaryExpression : Semantic_Expression
    {
        public Semantic_Expression Left { get; }
        public Semantic_BinaryOperator Operator { get; }
        public Semantic_Expression Right { get; }
        public Semantic_BinaryExpression(Semantic_Expression left, Semantic_BinaryOperator operatorSymbol, Semantic_Expression right)
        {
            Left = left;
            Operator = operatorSymbol;
            Right = right;
        }
        public Semantic_BinaryOperatorType OperatorType { get; }
        public SemanticType Kind => SemanticType.BinaryExpression;
        public override GSharpTypes Types => Operator.ReturnType;
    }

    internal sealed class Semantic_BinaryOperator
    {
        public Semantic_BinaryOperator(TokenType tokenType, Semantic_BinaryOperatorType operatorSymbol,
        GSharpTypes type) : this(tokenType, operatorSymbol, type, type, type)
        {
        }
        public Semantic_BinaryOperator(TokenType tokenType, Semantic_BinaryOperatorType operatorSymbol,
        GSharpTypes operatorsTypes, GSharpTypes returnType) : this(tokenType, operatorSymbol, operatorsTypes, operatorsTypes, returnType)
        {
        }
        public Semantic_BinaryOperator(TokenType tokenType, Semantic_BinaryOperatorType operatorSymbol,
        GSharpTypes leftType, GSharpTypes rightType, GSharpTypes returnType)
        {
            TokenType = tokenType;
            OperatorSymbol = operatorSymbol;
            LeftType = leftType;
            RightType = rightType;
            ReturnType = returnType;
        }

        private static readonly Semantic_BinaryOperator[] operators =
        {
            new(TokenType.PlusToken,Semantic_BinaryOperatorType.Addition,GSharpTypes.Number),
            new(TokenType.MinusToken,Semantic_BinaryOperatorType.Difference,GSharpTypes.Number),
            new(TokenType.DivisionToken,Semantic_BinaryOperatorType.Division,GSharpTypes.Number),
            new(TokenType.MultiplyToken,Semantic_BinaryOperatorType.Multiplication,GSharpTypes.Number),
            new(TokenType.ExponentialToken,Semantic_BinaryOperatorType.Pow,GSharpTypes.Number),
            new(TokenType.ModuleToken,Semantic_BinaryOperatorType.Modulo,GSharpTypes.Number),
            new(TokenType.SingleOrToken,Semantic_BinaryOperatorType.LogicalOr,GSharpTypes.Boolean),
            new(TokenType.SingleAndToken,Semantic_BinaryOperatorType.LogicalAnd,GSharpTypes.Boolean),
            new(TokenType.EqualToken,Semantic_BinaryOperatorType.EqualsComparer,GSharpTypes.Number, GSharpTypes.Boolean),
            new(TokenType.NotEqualToken,Semantic_BinaryOperatorType.NotEqualComparer,GSharpTypes.Number, GSharpTypes.Boolean),
            new(TokenType.BiggerToken,Semantic_BinaryOperatorType.GreaterThanComparer,GSharpTypes.Number, GSharpTypes.Boolean),
            new(TokenType.LowerToken,Semantic_BinaryOperatorType.LowerThanComparer,GSharpTypes.Number, GSharpTypes.Boolean),
            new(TokenType.BiggerOrEqualToken,Semantic_BinaryOperatorType.GreaterOrEqualComparer,GSharpTypes.Number, GSharpTypes.Boolean),
            new(TokenType.LowerOrEqualToken,Semantic_BinaryOperatorType.LowerOrEqualComparer,GSharpTypes.Number, GSharpTypes.Boolean),
            new(TokenType.EqualToken,Semantic_BinaryOperatorType.EqualsComparer, GSharpTypes.Boolean),
            new(TokenType.NotEqualToken,Semantic_BinaryOperatorType.NotEqualComparer, GSharpTypes.Boolean)
        };

        public TokenType TokenType { get; }
        public Semantic_BinaryOperatorType OperatorSymbol { get; }
        public GSharpTypes LeftType { get; }
        public GSharpTypes RightType { get; }
        public GSharpTypes ReturnType { get; }

        public static Semantic_BinaryOperator Semantic_Parse_BO(TokenType tokenType, GSharpTypes leftType, GSharpTypes rightType)
        {
            foreach (var a in operators)
            {
                if (a.TokenType == tokenType && a.LeftType == leftType && a.RightType == rightType)
                    return a;
            }
            return null!;
        }
    }
}