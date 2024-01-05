using GSharpProject.Parsing;

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

    public override void CheckType(TypedScope typedScope)
    {
        IfCondition.CheckType(typedScope);
        ThenStatement.CheckType(typedScope);
        ElseClause.CheckType(typedScope);

        //checkeamos la condicion el then y el else y verificamos q el then y el else sean del mismo tipo de retorno
        if (ThenStatement.ExpressionType != ElseClause.ExpressionType)
        {
            throw new Exception("ThenStatement and ElseClause must be the same return type");
        }

        ExpressionType = ThenStatement.ExpressionType;
    }
}

