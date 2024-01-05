using System;
using System.Collections.Generic;
using System.Drawing;
using GSharpProject.Parsing;

namespace GSharpProject;
//Biblioteca standard del programa 
//Clase estatica donde tenemos el control de las funciones que ya tiene el lenguaje y las variables como Pi y E
public static class StandardLibrary
{
    //Una vez corra el programa ya estas variables estaran definidas esto nos permite una facilidad para agregar variables o funciones
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
            //diccionario con las funciones del programa 
            ["sin"] = new FunctionDeclarationExpression("sin", new() { "x" }, new FunctionReference((scope) => Math.Sin(Convert.ToDouble(scope.GetValue("x"))), SingleType.Of<double>())),

            ["cos"] = new FunctionDeclarationExpression("cos", new() { "x" }, new FunctionReference((scope) => Math.Cos(Convert.ToDouble(scope.GetValue("x"))), SingleType.Of<double>())),

            ["sqrt"] = new FunctionDeclarationExpression("sqrt", new() { "x" }, new FunctionReference((scope) => Math.Sqrt(Convert.ToDouble(scope.GetValue("x"))), SingleType.Of<double>())),

            ["log"] = new FunctionDeclarationExpression("log", new() { "b", "p" }, new FunctionReference((scope) => Math.Log(Convert.ToDouble(scope.GetValue("p")), Convert.ToDouble(scope.GetValue("p"))), SingleType.Of<double>())),

            ["intersect"] = new FunctionDeclarationExpression("intersect", new() { "c1", "c2" }, new FunctionReference((scope) => Utiles.Interception(scope.GetValue("c1"), scope.GetValue("c2")), new CompoundType(typeof(LiteralSequence), SingleType.Of<Point>()))),

            ["samples"] = new FunctionDeclarationExpression("samples", new() { }, new FunctionReference((scope) => Utiles.GetSamples(), new CompoundType(typeof(InfiniteSequence), SingleType.Of<Point>()))),

            ["points"] = new FunctionDeclarationExpression("points", new() { "f" }, new FunctionReference((scope) => Utiles.GetPointsOf(scope.GetValue("f")), new CompoundType(typeof(InfiniteSequence), SingleType.Of<Point>()))),

            ["random"] = new FunctionDeclarationExpression("random", new() { }, new FunctionReference((scope) => Utiles.GetRandom(), new CompoundType(typeof(LiteralSequence), SingleType.Of<double>()))),

            ["count"] = new FunctionDeclarationExpression("count", new() { "x" }, new FunctionReference((scope) => Utiles.GetCountSequence((Sequence)scope.GetValue("x")), SingleType.Of<int>())),

            ["restore"] = new FunctionDeclarationExpression("restore", new() { }, new FunctionReference((scope) => (WallEColors.ColorDraw.Count != 1) ? WallEColors.ColorDraw.Pop() : 0, SingleType.Of<Color>())),
        };
    }
}


