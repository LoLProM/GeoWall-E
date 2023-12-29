using System.Collections.Generic;
using GSharpProject.Parsing;

namespace GSharpProject;

public class GSharpPointExpression : GSharpPrimitive
{
    public Token PointToken { get; }
    public string Identifier { get; }
    public Point Value { get; }
    public List<GSharpExpression> Coordinates { get; }

    public GSharpPointExpression(Token pointToken, string identifier, Point value)
    {
        PointToken = pointToken;
        Identifier = identifier;
        Value = value;
        ExpressionType = new SingleType(typeof(Point));
    }

    public GSharpPointExpression(Token pointToken, List<GSharpExpression> coordinates)
    {
        PointToken = pointToken;
        Coordinates = coordinates;
    }
    public override TokenType TokenType => TokenType.Point;

    public override void CheckType(TypedScope typedScope)
    {
        if (Coordinates is not null)
        {
            var firstValue = Coordinates[0];
            var secondValue = Coordinates[1];
            firstValue.CheckType(typedScope);
            secondValue.CheckType(typedScope);
            if (firstValue.ExpressionType != SingleType.Of<int>() || secondValue.ExpressionType != SingleType.Of<int>())
            {
                throw new Exception("");
            }
            ExpressionType = new SingleType(typeof(Point));
        }
        ExpressionType = new SingleType(typeof(Point));
    }
}