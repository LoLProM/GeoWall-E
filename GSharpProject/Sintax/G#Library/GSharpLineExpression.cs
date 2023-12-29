using System.Collections.Generic;
using GSharpProject.Parsing;

namespace GSharpProject;

public class GSharpLineExpression : GSharpPrimitive
{
    public GSharpLineExpression(Token line, string identifier)
    {
        Line = line;
        Identifier = identifier;
        ExpressionType = new SingleType(typeof(Line));
    }

    public GSharpLineExpression(Token line, List<GSharpExpression> coordinates)
    {
        Line = line;
        Coordinates = coordinates;
        ExpressionType = new SingleType(typeof(Line));
    }

    public Token Line { get; }
    public List<GSharpExpression> Coordinates { get; }
    public string Identifier { get; }
    public override TokenType TokenType => TokenType.Line;

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
            ExpressionType = new SingleType(typeof(Line));
        }
        else
        {
            typedScope.AddVariable(Identifier, new SingleType(typeof(Line)));
            ExpressionType = new SingleType(typeof(Line));
        }
    }
}
