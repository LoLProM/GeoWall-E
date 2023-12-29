using System;
using System.Collections.Generic;
using System.Drawing;
using GSharpProject.Parsing;

namespace GSharpProject;

public static class StandardLibrary
{
    public static Dictionary<string, FunctionDeclarationExpression> Functions = new();
    public static EvalScope Variables = new();
    public static TypedScope VariableTypes = new();

    public static void Initialize()
    {
        Functions = GetStandardLibraryFunctions();
        Variables = GetStandardLibraryVariables();
        VariableTypes = GetStandardLibraryVariablesTypes();
    }

    private static EvalScope GetStandardLibraryVariables()
    {
        var variables = new EvalScope();
        variables.AddVariable("PI", Math.PI);
        variables.AddVariable("E", Math.E);
        return variables;
    }

    private static TypedScope GetStandardLibraryVariablesTypes()
    {
        var variables = new TypedScope();
        variables.AddVariable("PI", new SingleType(typeof(double)));
        variables.AddVariable("E", new SingleType(typeof(double)));
        return variables;
    }

    private static Dictionary<string, FunctionDeclarationExpression> GetStandardLibraryFunctions()
    {
        return new()
        {
            ["sin"] = new FunctionDeclarationExpression("sin", new() { "x" }, new FunctionReference((scope) => Math.Sin(Convert.ToDouble(scope.GetValue("x"))))),

            ["cos"] = new FunctionDeclarationExpression("cos", new() { "x" }, new FunctionReference((scope) => Math.Cos(Convert.ToDouble(scope.GetValue("x"))))),

            ["sqrt"] = new FunctionDeclarationExpression("sqrt", new() { "x" }, new FunctionReference((scope) => Math.Sqrt(Convert.ToDouble(scope.GetValue("x"))))),

            ["log"] = new FunctionDeclarationExpression("log", new() { "b", "p" }, new FunctionReference((scope) => Math.Log(Convert.ToDouble(scope.GetValue("p")),Convert.ToDouble(scope.GetValue("p"))))),

            ["log2"] = new FunctionDeclarationExpression("log2", new() { "p" }, new FunctionReference((scope) => Math.Log2(Convert.ToDouble(scope.GetValue("p"))))),

            ["log10"] = new FunctionDeclarationExpression("log10", new() { "p" }, new FunctionReference((scope) => Math.Log10(Convert.ToDouble(scope.GetValue("p"))))),

            ["print"] = new FunctionDeclarationExpression("print", new() { "x" }, new FunctionReference((scope) => scope.GetValue("x").ToString()!)),

            ["intercept"] = new FunctionDeclarationExpression("intercept", new() { "c1", "c2" }, new FunctionReference((scope) => Utiles.Interception(scope.GetValue("f1"), scope.GetValue("f1")))),

            ["samples"] = new FunctionDeclarationExpression("samples", new(){},new FunctionReference((scope)=> Utiles.GetSamples())),

            ["points"] = new FunctionDeclarationExpression("points", new(){"f"}, new FunctionReference((scope)=> Utiles.GetPointsOf(scope.GetValue("f")))),

            ["random"] = new FunctionDeclarationExpression("random", new(){}, new FunctionReference((scope)=> Utiles.GetRandom())),
            
            ["count"] = new FunctionDeclarationExpression("count", new(){"x"}, new FunctionReference((scope)=> Utiles.GetCountSequence((Sequence)scope.GetValue("x")))),

            ["restore"] = new FunctionDeclarationExpression("restore", new(){}, new FunctionReference((scope)=> WallEColors.ColorDraw.Pop) ),
        };
    }
}


