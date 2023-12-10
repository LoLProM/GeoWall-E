using System;

namespace GSharpProject;

public class GSharpUnaryExpression : GSharpExpression
{
    public GSharpUnaryExpression(Token operatorToken, GSharpExpression internalExpression)
    {
        OperatorToken = operatorToken;
        InternalExpression = internalExpression;
        ExpressionType = GetUnaryExpressionType();
    }

    public Token OperatorToken { get; }
    public override TokenType TokenType => TokenType.UnaryExpression;
    public GSharpExpression InternalExpression { get; }
    private Type GetUnaryExpressionType()
    {
        if (OperatorToken.Type == TokenType.PlusToken)
        {
            if (InternalExpression.ExpressionType == typeof(double))
                return typeof(double);
        }

        else if (OperatorToken.Type == TokenType.MinusToken)
        {
            if (InternalExpression.ExpressionType == typeof(double))
                return typeof(double);
        }

        else if (OperatorToken.Type == TokenType.NotToken)
        {
            if (InternalExpression.ExpressionType == typeof(bool))
                return typeof(bool);
        }
        throw new Exception($"!SEMANTIC ERROR : Cannot applied {OperatorToken.Type} to {InternalExpression.ExpressionType}");
    }
}




