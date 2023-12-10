namespace GSharpProject;

public class If_ElseStatement : GSharpExpression
{
    public If_ElseStatement(Token ifKeyword, GSharpExpression ifCondition, GSharpExpression thenStatement, GSharpExpression elseClause)
    {
        IfKeyword = ifKeyword;
        IfCondition = ifCondition;
        ThenStatement = thenStatement;
        ElseClause = elseClause;
        ExpressionType = thenStatement.ExpressionType;
    }
    public override TokenType TokenType => TokenType.IfElseExpression;
    public Token IfKeyword { get; }
    public GSharpExpression IfCondition { get; }
    public GSharpExpression ThenStatement { get; }
    public GSharpExpression ElseClause { get; }
}

