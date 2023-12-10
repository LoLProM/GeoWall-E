using System;

namespace GSharpProject;

public class GSharpLiteralExpression : GSharpPrimitive // Numeros, Strings, Booleanos
{
    public GSharpLiteralExpression(Token literalToken) : this (literalToken,literalToken.Value)
    {
    }

    public GSharpLiteralExpression(Token literalToken, object value)
    {
        LiteralToken = literalToken;
        Value = value;
    }

    public override Type ExpressionType => Value.GetType();
    public override TokenType TokenType => TokenType.LiteralExpression;
    public Token LiteralToken { get; }
    public object Value { get; }
}



