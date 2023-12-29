namespace GSharpProject.Parsing;

public class GSharpStatementsCollection : GSharpExpression
{
    public GSharpStatementsCollection(List<GSharpExpression> statements)
    {
        Statements = statements;
    }
    public List<GSharpExpression> Statements { get; }
    public override TokenType TokenType => TokenType.StatementsColl;

    public override void CheckType(TypedScope typedScope)
    {
        foreach(var expression in Statements)
        {
            if (expression is not FunctionDeclarationExpression)
            expression.CheckType(typedScope);
        }
    }
}
