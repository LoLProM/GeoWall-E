using System;
using GSharpProject.Parsing;

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
    public override TokenType TokenType => LiteralToken.Type;
    public Token LiteralToken { get; }
    public object Value { get; private set;}

    public override void CheckType(TypedScope typedScope)
    {
        if (LiteralToken.Type is TokenType.Identifier)
        {
            Value = Undefined.Value;
            ExpressionType = typedScope.GetValue(LiteralToken.Text);
        }
        else if (LiteralToken.Type is TokenType.Undefine)
        {
            Value = Undefined.Value;
            ExpressionType = new SingleType(typeof(Undefined));
        }
        else{
            ExpressionType = new SingleType(LiteralToken.Value.GetType());
        }
    }
}
