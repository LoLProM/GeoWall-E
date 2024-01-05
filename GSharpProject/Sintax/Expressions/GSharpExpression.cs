using System;
using GSharpProject.Parsing;

namespace GSharpProject;
//Clase padre del Walle todas las expressiones heredan de aqui todas las expresiones saben chekear su tipo y tienen un tipo de expression

public abstract class GSharpExpression
{
    public abstract void CheckType(TypedScope typedScope);
    public virtual ExpressionType? ExpressionType { get; protected set;}
    public abstract TokenType TokenType { get; }
}

public abstract record ExpressionType(Type Type)
{
}

public record SingleType(Type Type) : ExpressionType(Type) //tenemos las expresiones q tienen un tipo solamente
{
    public static SingleType Of<T>() => new(typeof(T));
}

public record CompoundType(Type Type, ExpressionType ContentType) : ExpressionType(Type);// expresiones con tipo compuesto esto facilita a la hora de tratar con secuencias