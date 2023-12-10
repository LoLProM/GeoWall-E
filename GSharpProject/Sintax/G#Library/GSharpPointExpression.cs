using System.Collections.Generic;

namespace GSharpProject;

public class GSharpPointExpression : GSharpPrimitive
{
    public Token PointToken {get;}
    public string Identifier {get;}
    public List<string> Coordinates { get; }

    public GSharpPointExpression(Token pointToken, string identifier)
    {
        PointToken = pointToken;
        Identifier = identifier;
        ExpressionType = typeof(Point);
    }

    public GSharpPointExpression(Token pointToken, string identifier, List<string> coordinates)
    {
        PointToken = pointToken;
        Identifier = identifier;
        Coordinates = coordinates;
    }
    public override TokenType TokenType => TokenType.Point;

}