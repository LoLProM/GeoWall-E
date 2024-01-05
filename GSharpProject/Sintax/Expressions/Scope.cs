using System;
using System.Collections.Generic;

namespace GSharpProject.Parsing;

public abstract class Scope<T>
{//Clase encargada de tener el ambito del programa
//Tenemos el diccionario de variables 
    protected Scope<T>? parent;
    private readonly Dictionary<string, T> variables;
    public Scope()
    {
        variables = new Dictionary<string, T>();
    }

    public void AddVariable(string identifier, T expression)
    {
        variables[identifier] = expression;
    }
    public bool Contains(string identifier)
    {
        if(variables.ContainsKey(identifier))
        {
            return true;
        }
        return parent is not null && parent.Contains(identifier);
    }
    public void AddChildScope(Scope<T> childScope)
    {
        childScope.parent = this;
    }

    public T GetValue(string identifier)
    {
        if(variables.ContainsKey(identifier))
        {
            return variables[identifier];
        }
        return parent is null ? throw new Exception($"! SEMANTIC ERROR : Undefine variable {identifier}") : parent.GetValue(identifier);
    }

    public List<string> GetVariables()
    {
        return variables.Keys.ToList();
    }
}

public class TypedScope : Scope<ExpressionType>//scope de tipos 
{
}

public class EvalScope : Scope<object> //scope de valores
{ 
}