using System.Collections.Generic;

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
    
}
