using System;
using System.Collections.Generic;
namespace GSharpProject;

public class AssignmentExpression : GSharpExpression
{   
    public AssignmentExpression(string identifier, Token equalToken, GSharpExpression expression)
    {
        Identifier = identifier;
        EqualToken = equalToken;
        Expression = expression;
        ExpressionType = Expression.ExpressionType;
    }
    public AssignmentExpression(List<string> identifiers, GSharpSequence expression)
    {
        Identifiers = identifiers;
        Expression = expression;
        ExpressionType = Expression.ExpressionType;
    }
    public string Identifier { get; }
    public List<string> Identifiers { get; }
    public Token EqualToken { get; }
    public GSharpExpression Expression { get; }
    public TokenType tokenType => TokenType.AssigmentExpression;
}