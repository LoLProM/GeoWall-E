namespace GSharpProject;

public class Let_In_Expression : GSharpExpression
{
    public Let_In_Expression(LetExpression letExpression, GSharpExpression inExpression)
    {
        LetExpression = letExpression;
        InExpression = inExpression;
        ExpressionType = InExpression.ExpressionType;
    }
    public LetExpression LetExpression { get; }
    public GSharpExpression InExpression { get; }

}
