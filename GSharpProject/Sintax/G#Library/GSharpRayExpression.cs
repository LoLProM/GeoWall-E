using System.Collections.Generic;
using GSharpProject.Parsing;

namespace GSharpProject;

public class GSharpRayExpression : GSharpPrimitive
{
    public GSharpRayExpression(Token ray, string identifier)
    {
        Ray = ray;
        Identifier = identifier;
        ExpressionType = new SingleType(typeof(Ray));
    }

    public GSharpRayExpression(Token ray, List<GSharpExpression> coordinates)
    {
        Ray = ray;
        Coordinates = coordinates;
        ExpressionType = new SingleType(typeof(Ray));
    }

    public Token Ray { get; }
    public List<GSharpExpression> Coordinates { get; }
    public string Identifier { get; }
    public override TokenType TokenType => TokenType.Ray;

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
            ExpressionType = new SingleType(typeof(Ray));
        }
        else
        {
            typedScope.AddVariable(Identifier, new SingleType(typeof(Ray)));
            ExpressionType = new SingleType(typeof(Ray));
        }
    }
}