namespace GSharpProject
{
    public class LetExpression : GSharpExpression
    {
        public LetExpression(Token identifier, GSharpExpression expression) : this(identifier, expression, null!)
        {
        }

        public LetExpression(Token identifier, GSharpExpression expression, LetExpression letChildExpression)
        {
            Identifier = identifier;
            Expression = expression;
            LetChildExpression = letChildExpression;
            ExpressionType = Expression.ExpressionType;
        }
        public Token Identifier { get; }
        public GSharpExpression Expression { get; }
        public LetExpression LetChildExpression { get; }
    }
}