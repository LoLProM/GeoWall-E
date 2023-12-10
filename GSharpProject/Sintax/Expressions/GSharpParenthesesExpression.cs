namespace GSharpProject;

public class GSharpParenthesesExpression : GSharpExpression
{
    public GSharpParenthesesExpression(Token openParenthesisToken, GSharpExpression insideExpression, Token closeParenthesisToken)
    {
        OpenParenthesisToken = openParenthesisToken;
        InsideExpression = insideExpression;
        CloseParenthesisToken = closeParenthesisToken;
        ExpressionType = insideExpression.ExpressionType;
    }
    public override TokenType TokenType => TokenType.ParenthesizedExpression;
    public Token OpenParenthesisToken { get; }
    public GSharpExpression InsideExpression { get; }
    public Token CloseParenthesisToken { get; }
}



