using GSharpProject;
using GSharpProject.Parsing;

public sealed class Measure : GSharpExpression
{
    public double EuclideanDistance { get; }
    public override TokenType TokenType => TokenType.Measure;

    public Measure(Point p1,Point p2)
    {
        EuclideanDistance = Utiles.EuclideanDistance(p1, p2);
    }

    public Measure(double value)
    {
        EuclideanDistance = value;
    }

    public override void CheckType(TypedScope typedScope)
    {
        ExpressionType = new SingleType(typeof(Measure));
    }
}