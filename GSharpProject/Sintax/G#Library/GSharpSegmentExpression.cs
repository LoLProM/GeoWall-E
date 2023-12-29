using System.Collections.Generic;
using GSharpProject.Parsing;

namespace GSharpProject;

public class GSharpSegmentExpression : GSharpPrimitive
{
    public GSharpSegmentExpression(Token segment, string identifier)
    {
        Segment = segment;
        Identifier = identifier;
        ExpressionType = new SingleType(typeof(Segment));
    }

    public GSharpSegmentExpression(Token segment, List<GSharpExpression> coordinates)
    {
        Segment = segment;
        Coordinates = coordinates;
        ExpressionType = new SingleType(typeof(Segment));
    }

    public Token Segment { get; }
    public List<GSharpExpression> Coordinates { get; }
    public string Identifier { get; }
    public override TokenType TokenType => TokenType.Segment;

    public override void CheckType(TypedScope typedScope)
    {
        if (Coordinates is not null)
        {
            var start = Coordinates[0];
            var end = Coordinates[1];

            start.CheckType(typedScope);
            end.CheckType(typedScope);

            if (start.ExpressionType != SingleType.Of<Point>() || end.ExpressionType != SingleType.Of<Point>())
            {
                throw new Exception("Line expression must be points Parameters");
            }
            ExpressionType = new SingleType(typeof(Segment));
        }
        ExpressionType = new SingleType(typeof(Segment));
    }
}
