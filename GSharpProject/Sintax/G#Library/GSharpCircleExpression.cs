using System.Collections.Generic;
using GSharpProject.Parsing;

namespace GSharpProject;

public class GSharpCircleExpression : GSharpPrimitive
{
    public GSharpCircleExpression(Token circle, string identifier)
    {
        Circle = circle;
        Identifier = identifier;
        ExpressionType = new SingleType(typeof(Circle));
    }

    public GSharpCircleExpression(Token circle, List<GSharpExpression> coordinates)
    {
        Circle = circle;
        Coordinates = coordinates;
        ExpressionType = new SingleType(typeof(Circle));
    }

    public Token Circle { get; }
    public List<GSharpExpression> Coordinates { get; }
    public string Identifier { get; }
    public override TokenType TokenType => TokenType.Circle;

    public override void CheckType(TypedScope typedScope)
    {
        if (Coordinates is not null)
        {
            var center = Coordinates[0];
            var radius = Coordinates[1];

            center.CheckType(typedScope);
            radius.CheckType(typedScope);

            if (center.ExpressionType != SingleType.Of<Point>() || radius.ExpressionType != SingleType.Of<Point>())
            {
                throw new Exception("Circle Expressions parameters not have expected types");
            }
            ExpressionType = new SingleType(typeof(Circle));
        }
        ExpressionType = new SingleType(typeof(Circle));
    }
}
