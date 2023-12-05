using System.Collections.Generic;

namespace GSharpProject;

public class GSharpLineExpression : GSharpPrimitive
{
    public GSharpLineExpression(Token line, string identifier)
    {
        Line = line;
        Identifier = identifier;
        ExpressionType = typeof(Line);
    }

    public GSharpLineExpression(Token line, List<string> coordinates)
    {
        Line = line;
        Coordinates = coordinates;
        ExpressionType = typeof(Line);
    }

    public Token Line { get; }
    public List<string> Coordinates { get; }
    public string Identifier { get; }
}
