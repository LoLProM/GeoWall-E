using System.Collections.Generic;
using GSharpProject.Parsing;

namespace GSharpProject;

//Esta clase es la representacion de la llamada de una funcion su nombre y la lista de parametros
public class FunctionCallExpression : GSharpExpression
{
    public FunctionCallExpression(string functionName, List<GSharpExpression> parameters)
    {
        FunctionName = functionName;
        Parameters = parameters;
    }
    public string FunctionName { get; }
    public List<GSharpExpression> Parameters { get; }
    public override TokenType TokenType => TokenType.FunctionCall;

    public override void CheckType(TypedScope typedScope)
    {
        if (!StandardLibrary.Functions.ContainsKey(FunctionName))
        {
            throw new Exception($"Function {FunctionName} is not declared");
        }
        //Checkeamos si la funcion existe si es asi entonces a cada parametro verificamos si tiene la misma cantidad q su declaracion y entonces a cada parametro le chekeamos el tipo

        TypedScope functionDeclarationScope = new();

        var functionDeclaration = StandardLibrary.Functions[FunctionName];

        if (functionDeclaration.Arguments.Count != Parameters.Count)
        {
            throw new Exception($"!FUNCTION ERROR : Function {FunctionName} does not have {Parameters.Count} parameters but {StandardLibrary.Functions[FunctionName]?.Arguments.Count} parameters");
        }

        for (int i = 0; i < Parameters.Count; i++)
        {
            var argsScope = new TypedScope();
            typedScope.AddChildScope(argsScope);
            Parameters[i].CheckType(argsScope);
            functionDeclarationScope.AddVariable(functionDeclaration.Arguments[i], Parameters[i].ExpressionType!);
        }

        functionDeclaration.CheckType(functionDeclarationScope);
        ExpressionType = functionDeclaration.ExpressionType;
    }
}
