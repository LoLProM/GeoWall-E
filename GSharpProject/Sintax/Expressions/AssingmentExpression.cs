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
        //Checkeamos la expression de la derecha si su tipo es un compound type entonces es una secuencia a la cual seteamos a cada variable su tipo en el scope de tipos
        Expression.CheckType(typedScope);

        if (Expression.ExpressionType is CompoundType compoundType)
        {
            SetScopeType(typedScope, compoundType);
        }
        else
        //si no es una secuencia entonces asignamos directamente su valor
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
