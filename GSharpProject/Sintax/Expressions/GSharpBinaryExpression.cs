using System;
using GSharpProject.Parsing;

namespace GSharpProject;

public class GSharpBinaryExpression : GSharpExpression
{
    public GSharpBinaryExpression(GSharpExpression left, Token operatorToken, GSharpExpression right)
    {
        Left = left;
        OperatorToken = operatorToken;
        Right = right;
        ExpressionType = GetExpressionType();
    }
    public GSharpExpression Left { get; }
    public Token OperatorToken { get; }
    public GSharpExpression Right { get; }
    public override TokenType TokenType => TokenType.BinaryExpression;
    public override void CheckType(TypedScope typedScope)
    {
        Left.CheckType(typedScope);
        Right.CheckType(typedScope);

        if (Left.ExpressionType == SingleType.Of<Undefined>() || Right.ExpressionType == SingleType.Of<Undefined>() && !(Left.ExpressionType == SingleType.Of<Sequence>() && Right.ExpressionType == SingleType.Of<Undefined>()))
        {
            ExpressionType = SingleType.Of<Undefined>();
        }
        else
        {
            ExpressionType = TypeCheckerHelper.GetBinaryOperatorType(Left, OperatorToken, Right);
        }
    }
    public ExpressionType GetExpressionType()
    {
        if (OperatorToken.Type is TokenType.PlusToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<int>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else
            {
                return null!;
            }
        }

        else if (OperatorToken.Type is TokenType.MinusToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left!.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<int>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else
            {
                return null!;
            }
        }

        else if (OperatorToken.Type is TokenType.MultiplyToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left!.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<int>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else
            {
                return null!;
            }
        }

        else if (OperatorToken.Type is TokenType.DivisionToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left!.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<int>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else
            {
                return null!;
            }
        }

        else if (OperatorToken.Type is TokenType.SingleAndToken)
        {

            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left!.ExpressionType == SingleType.Of<bool>() && Right.ExpressionType == SingleType.Of<bool>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<int>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else
            {
                return null!;
            }
        }
        else if (OperatorToken.Type is TokenType.SingleOrToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left!.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<bool>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else
            {
                return null!;
            }
        }
        else if (OperatorToken.Type is TokenType.ModuleToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left!.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<int>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else
            {
                return null!;
            }
        }
        else if (OperatorToken.Type is TokenType.BiggerToken || OperatorToken.Type is TokenType.BiggerOrEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left!.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<bool>();
            }
            else
            {
                return null!;
            }
        }

        else if (OperatorToken.Type is TokenType.LowerToken || OperatorToken.Type is TokenType.LowerOrEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left!.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<bool>();
            }
            else
            {
                return null!;
            }
        }

        else if (OperatorToken.Type is TokenType.EqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left!.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<bool>();
            }
            else
            {
                return null!;
            }
        }
        else if (OperatorToken.Type is TokenType.NotEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left!.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<bool>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<bool>();
            }
            else
            {
                return null!;
            }

        }
        else if (OperatorToken.Type is TokenType.ExponentialToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left!.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<int>();
            }
            else if (Left!.ExpressionType == SingleType.Of<double>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else
            {
                return null!;
            }
        }
        else if (OperatorToken.Type is TokenType.SingleEqualToken)
        {
            if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<double>())
            {
                return SingleType.Of<double>();
            }
            else if (Left!.ExpressionType == SingleType.Of<int>() && Right.ExpressionType == SingleType.Of<int>())
            {
                return SingleType.Of<int>();
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == SingleType.Of<bool>())
            {
                return SingleType.Of<bool>();
            }
            else
            {
                return null!;
            }
        }
        throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {Left.ExpressionType} with {Right.ExpressionType} using {OperatorToken.Text}");
    }
}

