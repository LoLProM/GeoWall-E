using System;
using GSharpProject.Parsing;

namespace GSharpProject;

public class GSharpUnaryExpression : GSharpExpression
{
    public GSharpUnaryExpression(Token operatorToken, GSharpExpression internalExpression)
    {
        OperatorToken = operatorToken;
        InternalExpression = internalExpression;
    }

    public Token OperatorToken { get; }
    public override TokenType TokenType => TokenType.UnaryExpression;
    public GSharpExpression InternalExpression { get; }

    public override void CheckType(TypedScope typedScope)
    {
        InternalExpression.CheckType(typedScope);
        ExpressionType = InternalExpression.ExpressionType;
    }
}




