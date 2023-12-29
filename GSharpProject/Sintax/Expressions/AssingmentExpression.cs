using System;
using System.Collections.Generic;
using GSharpProject.Parsing;
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

    public override void CheckType(TypedScope typedScope)
    {
        Expression.CheckType(typedScope);

        if (Expression.ExpressionType is CompoundType compoundType)
        {
            SetScopeType(typedScope, compoundType);
        }
        else
        {
            if (Identifiers.Count > 1 && Expression.ExpressionType != SingleType.Of<Undefined>())
            {
                throw new Exception("");
            }
            ExpressionType = Expression.ExpressionType;
            typedScope.AddVariable(Identifiers[0], ExpressionType!);
        }
    }

    private void SetScopeType(TypedScope typedScope, CompoundType compoundType)
    {
        for (int i = 0; i < Identifiers.Count - 1; i++)
        {
            if (Identifiers[i] == "_") continue;
            typedScope.AddVariable(Identifiers[i], compoundType.ContentType);
        }
        if (Identifiers[^1] != "_")
            typedScope.AddVariable(Identifiers[^1], new CompoundType(compoundType.Type,compoundType.ContentType));
    }
}
