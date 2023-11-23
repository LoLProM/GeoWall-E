using System.Collections.Generic;

namespace GSharpProject;

public class FunctionCallExpression : GSharpExpression
{
    public FunctionCallExpression(string functionName, List<GSharpExpression> parameters)
    {
        FunctionName = functionName;
        Parameters = parameters;
    }
    public string FunctionName { get; }
    public List<GSharpExpression> Parameters { get; }
}
