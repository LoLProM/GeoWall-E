namespace GSharpProject;

public class Let_In_Expression : GSharpExpression
{
    public Let_In_Expression(List<GSharpExpression> letExpressions, GSharpExpression inExpression)
    {
        LetExpressions = letExpressions;
        InExpression = inExpression;
        ExpressionType = InExpression.ExpressionType;
    }
    public List<GSharpExpression> LetExpressions { get; }
    public GSharpExpression InExpression { get; }

    public override TokenType TokenType => TokenType.LetIn;
}
