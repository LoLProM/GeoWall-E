using GSharpProject.Parsing;

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

    public override void CheckType(TypedScope typedScope)
    {
        //checkeamos lo q esta dentro de los parentesis
        InsideExpression.CheckType(typedScope);
        ExpressionType = InsideExpression.ExpressionType;
    }
}



