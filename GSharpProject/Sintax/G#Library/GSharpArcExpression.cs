using System.Collections.Generic;
using GSharpProject.Parsing;

namespace GSharpProject;
//Expression de tipo arco 
public class GSharpArcExpression : GSharpPrimitive
{
    public GSharpArcExpression(Token arc, string identifier)
    {
        Arc = arc;
        Identifier = identifier;
        ExpressionType = new SingleType(typeof(Arc));
    }

    public GSharpArcExpression(Token arc, List<GSharpExpression> coordinates)
    {
        Arc = arc;
        Coordinates = coordinates;
        ExpressionType = new SingleType(typeof(Arc));
    }
    public Token Arc { get; }
    public List<GSharpExpression> Coordinates { get; }
    public string Identifier { get; }
    public override TokenType TokenType => TokenType.Arc;

    public override void CheckType(TypedScope typedScope)
    {
        if (Coordinates is not null)
        {
            //Checkeamos cada una de sus coordenadas y verificamos q tengan el tipo correcto
            var center = Coordinates[0];
            var start = Coordinates[1];
            var end = Coordinates[2];
            var radius = Coordinates[3];
            center.CheckType(typedScope);
            start.CheckType(typedScope);
            end.CheckType(typedScope);
            radius.CheckType(typedScope);
            if (center.ExpressionType != SingleType.Of<Point>() || start.ExpressionType != SingleType.Of<Point>() || end.ExpressionType != SingleType.Of<Point>() || radius.ExpressionType != SingleType.Of<Measure>())
            {
                throw new Exception("Arc Expressions parameters have not the expectede type");
            }
            ExpressionType = new SingleType(typeof(Arc));
        }
        else
        {
            typedScope.AddVariable(Identifier, new SingleType(typeof(Arc)));
            ExpressionType = new SingleType(typeof(Arc));
        }
    }
}