using System.Collections.Generic;

namespace GSharpProject;

public class GSharpArcExpression : GSharpPrimitive
{
    public GSharpArcExpression(Token arc, string identifier)
    {
        Arc = arc;
        Identifier = identifier;
        ExpressionType = typeof(Arc);
    }

    public GSharpArcExpression(Token arc, List<string> coordinates)
    {
        Arc = arc;
        Coordinates = coordinates;
        ExpressionType = typeof(Arc);
    }
    public Token Arc { get; }
    public List<string> Coordinates { get; }
    public string Identifier { get; }
    public override TokenType TokenType => TokenType.Arc;
}