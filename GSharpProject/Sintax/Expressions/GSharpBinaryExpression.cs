using System;

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

    public Type GetExpressionType()
    {
        if (OperatorToken.Type is TokenType.PlusToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == typeof(double) && Right.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.MinusToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.MultiplyToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.DivisionToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.SingleAndToken)
        {

            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == typeof(bool) && Right.ExpressionType == typeof(bool))
            {
                return typeof(bool);
            }
            else
            {
                return null;
            }
        }
        else if (OperatorToken.Type is TokenType.SingleOrToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(bool))
            {
                return typeof(bool);
            }
            else
            {
                return null;
            }
        }
        else if (OperatorToken.Type is TokenType.ModuleToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }
        else if (OperatorToken.Type is TokenType.BiggerToken || OperatorToken.Type is TokenType.BiggerOrEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
                return typeof(bool);
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.LowerToken || OperatorToken.Type is TokenType.LowerOrEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
                return typeof(bool);
            else
            {
                return null;
            }
        }

        else if (OperatorToken.Type is TokenType.EqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
                return typeof(bool);
            else
            {
                return null;
            }

        }
        else if (OperatorToken.Type is TokenType.NotEqualToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(bool);
            }
            else
            {
                return null;
            }

        }
        else if (OperatorToken.Type is TokenType.ExponentialToken)
        {
            if (Left == null && Right == null)
            {
                return null!;
            }

            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else
            {
                return null;
            }
        }
        else if (OperatorToken.Type is TokenType.SingleEqualToken)
        {
            if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(double))
            {
                return typeof(double);
            }
            else if (Left.ExpressionType == Right.ExpressionType && Left.ExpressionType == typeof(bool))
            {
                return typeof(bool);
            }
            else
            {
                return null;
            }
        }
        throw new Exception($"!SEMANTIC ERROR : Invalid expression: Can't operate {Left.ExpressionType} with {Right.ExpressionType} using {OperatorToken.Text}");
    }
}

