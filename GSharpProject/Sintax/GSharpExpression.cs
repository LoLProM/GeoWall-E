using System;

namespace GSharpProject;

public abstract class GSharpExpression : GSharpStatement
{
    public virtual Type ExpressionType { get; init;}
}