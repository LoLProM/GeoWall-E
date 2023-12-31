using System.Collections.Generic;

namespace GSharpProject;

public class GSharpSegmentExpression : GSharpPrimitive
{
    public GSharpSegmentExpression(Token segment, string identifier)
    {
        Segment = segment;
        Identifier = identifier;
        ExpressionType = typeof(Line);
    }

    public GSharpSegmentExpression(Token segment, List<string> coordinates)
    {
        Segment = segment;
        Coordinates = coordinates;
        ExpressionType = typeof(Line);
    }

    public Token Segment { get; }
    public List<string> Coordinates { get; }
    public string Identifier { get; }
}
