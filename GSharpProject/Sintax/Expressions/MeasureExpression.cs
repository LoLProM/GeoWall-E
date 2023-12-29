namespace GSharpProject.Parsing;

public class MeasureExpression : GSharpExpression
{
    public MeasureExpression(GSharpExpression pointA, GSharpExpression pointB)
    {
        PointA = pointA;
        PointB = pointB;
    }
    public override TokenType TokenType => throw new NotImplementedException();
    public GSharpExpression PointA { get; }
    public GSharpExpression PointB { get; }

    public override void CheckType(TypedScope typedScope)
    {
        PointA.CheckType(typedScope);
        PointB.CheckType(typedScope);

        if (PointA.ExpressionType == PointB.ExpressionType && PointA.ExpressionType == SingleType.Of<int>())
        {
            ExpressionType = new SingleType(typeof(Measure));
        }
        else throw new Exception("Measure parameters must be points");
    }
}