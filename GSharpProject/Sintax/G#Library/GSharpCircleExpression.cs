using System.Collections.Generic;

namespace GSharpProject;

public class GSharpCircleExpression : GSharpPrimitive
{
    public GSharpCircleExpression(Token circle, string identifier)
    {
        Circle = circle;
        Identifier = identifier;
        ExpressionType = typeof(Circle);
    }

    public GSharpCircleExpression(Token circle, List<string> coordinates)
    {
        Circle = circle;
        Coordinates = coordinates;
        ExpressionType = typeof(Circle);
    }

    public Token Circle { get; }
    public List<string> Coordinates { get; }
    public string Identifier { get; }
    public override TokenType TokenType => TokenType.Circle;

}
