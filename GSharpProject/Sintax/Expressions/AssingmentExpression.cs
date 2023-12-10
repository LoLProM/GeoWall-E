using System;
using System.Collections.Generic;
namespace GSharpProject;

public class AssignmentExpression : GSharpExpression
{   
    public AssignmentExpression(List<string> identifiers, GSharpExpression expression)
    {
        Identifiers = identifiers;
        Expression = expression;
        ExpressionType = Expression.ExpressionType;
    }
    public List<string> Identifiers { get; }
    public GSharpExpression Expression { get; }
    public override TokenType TokenType => TokenType.AssignmentExpression;
}