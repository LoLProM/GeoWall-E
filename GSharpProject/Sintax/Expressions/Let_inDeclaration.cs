using GSharpProject.Parsing;

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

    public override void CheckType(TypedScope typedScope)
    {
        var letScope = new TypedScope();
        typedScope.AddChildScope(letScope);
        foreach(var expression in LetExpressions)
        {
            expression.CheckType(letScope);
        }
        InExpression.CheckType(letScope);
        ExpressionType = InExpression.ExpressionType;
    }
}
