using System;
using GSharpProject.Parsing;

namespace GSharpProject;

public abstract class GSharpExpression
{
    public abstract void CheckType(TypedScope typedScope);
    public virtual ExpressionType? ExpressionType { get; protected set;}
    public abstract TokenType TokenType { get; }
}

public abstract record ExpressionType(Type Type)
{
}

public record SingleType(Type Type) : ExpressionType(Type) 
{
    public static SingleType Of<T>() => new(typeof(T));
}

public record CompoundType(Type Type, ExpressionType ContentType) : ExpressionType(Type);