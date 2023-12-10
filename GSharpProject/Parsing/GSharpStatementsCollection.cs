namespace GSharpProject.Parsing;

public class GSharpStatementsCollection : GSharpExpression
{
    public GSharpStatementsCollection(List<GSharpExpression> statements)
    {
        Statements = statements;
    }
    public List<GSharpExpression> Statements { get; }
    public override TokenType TokenType => TokenType.StatementsColl;
}