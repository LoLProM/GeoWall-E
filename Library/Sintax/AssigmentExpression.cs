using System;
namespace GSharpProject;

public class AssigmentExpression : GSharpExpression
{   
    public AssigmentExpression(string identifier, Token equalToken, GSharpExpression expression)
    {
        Identifier = identifier;
        EqualToken = equalToken;
        Expression = expression;
        ExpressionType = Expression.ExpressionType;
    }
    public string Identifier { get; }
    public Token EqualToken { get; }
    public GSharpExpression Expression { get; }
    public TokenType tokenType => TokenType.AssigmentExpression;
}