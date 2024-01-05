using System.Collections.Generic;
using GSharpProject.Parsing;

namespace GSharpProject;

public class FunctionDeclarationExpression : GSharpExpression
{//Esta clase es la representacion de la declaracion de una funcion su nombre, la lista de parametros y el cuerpo de la funcion
    public FunctionDeclarationExpression(string functionName, List<string> parameters, GSharpExpression functionBody)
    {
        FunctionName = functionName;
        Arguments = parameters;
        FunctionBody = functionBody;
    }
    public string FunctionName { get; }
    public List<string> Arguments { get; }
    public GSharpExpression FunctionBody { get; private set;}
    public override TokenType TokenType => TokenType.FunctionDeclaration;
    public override void CheckType(TypedScope typedScope)
    {
        //verificamos si es uan funcion recursiva si no lo es entonces checkeamos su body 
        var recursiveChecker = new RecursionChecker();
        if (!recursiveChecker.CheckRecursiveFunctions(this))
        {
            FunctionBody.CheckType(typedScope);
            ExpressionType = FunctionBody.ExpressionType;
        }
        else ExpressionType = new SingleType(typeof(Undefined));
    }
}
