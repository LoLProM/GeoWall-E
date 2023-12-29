using GSharpProject;

public static class TypeCheckerHelper
{
    private readonly static Dictionary<(ExpressionType, TokenType, ExpressionType), ExpressionType> binaryTypesDic = new()
    {
        [(SingleType.Of<int>(), TokenType.PlusToken, SingleType.Of<int>())] = SingleType.Of<int>(),//

        [(SingleType.Of<double>(), TokenType.PlusToken, SingleType.Of<int>())] = SingleType.Of<double>(),//

        [(SingleType.Of<int>(), TokenType.PlusToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<double>(), TokenType.PlusToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<int>(), TokenType.MinusToken, SingleType.Of<int>())] = SingleType.Of<int>(),//

        [(SingleType.Of<double>(), TokenType.MinusToken, SingleType.Of<int>())] = SingleType.Of<double>(),//

        [(SingleType.Of<int>(), TokenType.MinusToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<double>(), TokenType.MinusToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<int>(), TokenType.MultiplyToken, SingleType.Of<int>())] = SingleType.Of<int>(),//

        [(SingleType.Of<int>(), TokenType.MultiplyToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<double>(), TokenType.MultiplyToken, SingleType.Of<int>())] = SingleType.Of<double>(),//

        [(SingleType.Of<double>(), TokenType.MultiplyToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<int>(), TokenType.DivisionToken, SingleType.Of<int>())] = SingleType.Of<int>(),//

        [(SingleType.Of<int>(), TokenType.DivisionToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<double>(), TokenType.DivisionToken, SingleType.Of<int>())] = SingleType.Of<double>(),//

        [(SingleType.Of<double>(), TokenType.DivisionToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<int>(), TokenType.ModuleToken, SingleType.Of<int>())] = SingleType.Of<int>(),//

        [(SingleType.Of<int>(), TokenType.ModuleToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<double>(), TokenType.ModuleToken, SingleType.Of<int>())] = SingleType.Of<int>(),//

        [(SingleType.Of<double>(), TokenType.ModuleToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<int>(), TokenType.ExponentialToken, SingleType.Of<int>())] = SingleType.Of<int>(),//

        [(SingleType.Of<int>(), TokenType.ExponentialToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<double>(), TokenType.ExponentialToken, SingleType.Of<int>())] = SingleType.Of<double>(),//

        [(SingleType.Of<double>(), TokenType.ExponentialToken, SingleType.Of<double>())] = SingleType.Of<double>(),//

        [(SingleType.Of<Measure>(), TokenType.PlusToken, SingleType.Of<Measure>())] = SingleType.Of<Measure>(),//

        [(SingleType.Of<Measure>(), TokenType.MinusToken, SingleType.Of<Measure>())] = SingleType.Of<Measure>(),//

        [(SingleType.Of<Measure>(), TokenType.MultiplyToken, SingleType.Of<Measure>())] = SingleType.Of<Measure>(),//

        [(SingleType.Of<Measure>(), TokenType.DivisionToken, SingleType.Of<Measure>())] = SingleType.Of<int>(),//

        [(SingleType.Of<Measure>(), TokenType.MultiplyToken, SingleType.Of<double>())] = SingleType.Of<Measure>(),//

        [(SingleType.Of<Measure>(), TokenType.MultiplyToken, SingleType.Of<int>())] = SingleType.Of<Measure>(),//

        [(SingleType.Of<Measure>(), TokenType.BiggerOrEqualToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.BiggerToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.LowerOrEqualToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.LowerToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.EqualToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.NotEqualToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//


        [(SingleType.Of<bool>(), TokenType.SingleAndToken, SingleType.Of<bool>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<bool>(), TokenType.SingleOrToken, SingleType.Of<bool>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<double>(), TokenType.BiggerOrEqualToken, SingleType.Of<double>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<int>(), TokenType.BiggerOrEqualToken, SingleType.Of<int>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<double>(), TokenType.BiggerOrEqualToken, SingleType.Of<int>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<int>(), TokenType.BiggerOrEqualToken, SingleType.Of<double>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<double>(), TokenType.BiggerToken, SingleType.Of<double>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<int>(), TokenType.BiggerToken, SingleType.Of<int>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<int>(), TokenType.BiggerToken, SingleType.Of<double>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<double>(), TokenType.BiggerToken, SingleType.Of<int>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<int>(), TokenType.LowerToken, SingleType.Of<int>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<double>(), TokenType.LowerToken, SingleType.Of<double>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<int>(), TokenType.LowerOrEqualToken, SingleType.Of<int>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<double>(), TokenType.LowerOrEqualToken, SingleType.Of<double>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<double>(), TokenType.LowerOrEqualToken, SingleType.Of<int>())] = SingleType.Of<bool>(),// 

        [(SingleType.Of<int>(), TokenType.LowerOrEqualToken, SingleType.Of<double>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<double>(), TokenType.EqualToken, SingleType.Of<double>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<int>(), TokenType.EqualToken, SingleType.Of<int>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<double>(), TokenType.EqualToken, SingleType.Of<int>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<int>(), TokenType.EqualToken, SingleType.Of<double>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<bool>(), TokenType.EqualToken, SingleType.Of<bool>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.SingleAndToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.SingleOrToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.BiggerOrEqualToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.BiggerToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.LowerToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.LowerOrEqualToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Measure>(), TokenType.EqualToken, SingleType.Of<Measure>())] = SingleType.Of<bool>(),//

        [(SingleType.Of<Sequence>(), TokenType.PlusToken, SingleType.Of<Sequence>())] = SingleType.Of<Sequence>(),//

        [(SingleType.Of<Sequence>(), TokenType.PlusToken, SingleType.Of<Undefined>())] = SingleType.Of<Sequence>(),//

        
    };

    internal static ExpressionType GetBinaryOperatorType(GSharpExpression left, Token operatorToken, GSharpExpression right)
    {
        if (!binaryTypesDic.ContainsKey((left.ExpressionType, operatorToken.Type, right.ExpressionType)!))
        {
            throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {left.ExpressionType.Type} with {right.ExpressionType.Type} using {operatorToken.Text}");
        }
        return binaryTypesDic[(left.ExpressionType, operatorToken.Type, right.ExpressionType)!];
    }
}