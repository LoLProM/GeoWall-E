using System.Collections.Generic;
using GSharpProject.Parsing;

namespace GSharpProject;

public class FunctionDeclarationExpression : GSharpExpression
{
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
        var recursiveChecker = new RecursionChecker();
        if (!recursiveChecker.CheckRecursiveFunctions(this))
        {
            FunctionBody.CheckType(typedScope);
            ExpressionType = FunctionBody.ExpressionType;
        }
        else ExpressionType = new SingleType(typeof(Undefined));
    }
}
