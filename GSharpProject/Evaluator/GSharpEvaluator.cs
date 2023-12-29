using System;
using System.Drawing;
using System.Linq;
using GSharpProject.Parsing;

namespace GSharpProject.Evaluator;

public class GSharpEvaluator
{
    private int Count;
    private readonly int stackOverflow = 1000;
    private GSharpStatementsCollection _Node;
	public List<(object, Color, string)> drawable;

    public GSharpEvaluator(GSharpStatementsCollection node)
    {
        drawable = new();
        _Node = node;
    }
    public void Evaluate()
    {
        EvalScope scope = StandardLibrary.Variables;
        foreach (var statement in _Node.Statements)
        {
            if (statement is FunctionDeclarationExpression) continue;
            EvaluateExpression(statement, scope);
        }
    }
    private object EvaluateExpression(GSharpExpression node, EvalScope scope)
    {
        Count++;
        if (Count > stackOverflow)
        {
            throw new Exception("!OVERFLOW ERROR : Hulk Stack overflow");
        }
        switch (node)
        {
            case GSharpLiteralExpression literal:
                return EvaluateLiteralExpression(literal, scope);
            case GSharpUnaryExpression unary:
                return EvaluateUnaryExpression(unary, scope);
            case GSharpParenthesesExpression parenthesesExpression:
                return EvaluateParenthesesExpression(parenthesesExpression, scope);
            case GSharpBinaryExpression binaryExpression:
                return EvaluateBinaryExpression(binaryExpression, scope);
            case If_ElseStatement ifElseStatement:
                return EvaluateIfElseStatement(ifElseStatement, scope);
            case Let_In_Expression letInExpression:
                return EvaluateLetInExpression(letInExpression, scope);
            case FunctionCallExpression functionCallExpression:
                return EvaluateFunctionCallExpression(functionCallExpression, scope);
            case FunctionReference functionReference:
                return EvaluateFunctionReference(functionReference, scope);
            case GSharpPointExpression pointExpression:
                return EvaluatePointExpression(pointExpression, scope);
            case GSharpLineExpression lineExpression:
                return EvaluateLineExpression(lineExpression, scope);
            case GSharpArcExpression arcExpression:
                return EvaluateArcExpression(arcExpression, scope);
            case GSharpSegmentExpression segmentExpression:
                return EvaluateSegmentExpression(segmentExpression, scope);
            case GSharpCircleExpression circleExpression:
                return EvaluateCircleExpression(circleExpression, scope);
            case GSharpRayExpression rayExpression:
                return EvaluateRayExpression(rayExpression, scope);
            case DrawExpression drawExpression:
                return EvaluateDrawExpression(drawExpression, scope);
            case GSharpRangeSequence sequenceExpression:
                return EvaluateRangeSequenceExpression(sequenceExpression, scope);
            case GSharpInfiniteSequence sequenceExpression:
                return EvaluateInfiniteSequenceExpression(sequenceExpression, scope);
            case GSharpLiteralSequence sequenceExpression:
                return EvaluateLiteralSequenceExpression(sequenceExpression, scope);
            case AssignmentExpression assignmentExpression:
                return EvaluateAssignmentExpression(assignmentExpression, scope);
            case MeasureExpression measureExpression:
                return EvaluateMeasureExpression(measureExpression, scope);
            case SequenceOf sequenceExpression:
                return EvaluateSequenceOf(sequenceExpression, scope);
        }
        throw new Exception($"! SYNTAX ERROR : Unexpected node {node}");
    }

    private object EvaluateParenthesesExpression(GSharpParenthesesExpression parenthesesExpression, EvalScope scope)
    {
        return EvaluateExpression(parenthesesExpression.InsideExpression, scope);
    }

    private object EvaluateMeasureExpression(MeasureExpression measureExpression, EvalScope scope)
    {
        var pointA = EvaluateExpression(measureExpression.PointA, scope);
        var pointB = EvaluateExpression(measureExpression.PointB, scope);
        return new Measure((Point)pointA, (Point)pointB);
    }

    private object EvaluateDrawExpression(DrawExpression drawExpression, EvalScope scope)
    {
        var drawableFigure = EvaluateExpression(drawExpression.Argument,scope);
        if (drawExpression.Message is not null)
        {
		drawable.Add((drawableFigure,drawExpression.Color,drawExpression.Message));
        }
        else
        {
            drawable.Add((drawableFigure,drawExpression.Color,""));
        }

        return new GSharpVoidEx();
    }

    private object EvaluateSequenceOf(SequenceOf sequenceExpression, EvalScope scope)
    {
        var sequenceOf = new LiteralSequence();
        IEnumerable<IFigure> figures = GetFigures(sequenceExpression);
        foreach (var element in figures)
        {
            sequenceOf.AddElement(element);
        }
        return sequenceOf;
    }

    private static IEnumerable<IFigure> GetFigures(SequenceOf sequenceExpression)
    {
        var range = Enumerable.Range(0, new Random().Next());
        IEnumerable<IFigure> figures = sequenceExpression.TokenType switch
        {
            TokenType.Point => range.Select(_ => new Point()),
            TokenType.Circle => range.Select(_ => new Circle()),
            TokenType.Arc => range.Select(_ => new Arc()),
            TokenType.Ray => range.Select(_ => new Ray()),
            TokenType.Segment => range.Select(_ => new Segment()),
            TokenType.Line => range.Select(_ => new Line()),
            _ => throw new Exception($"Invalid sequence type {sequenceExpression.TokenType}")
        };
        return figures;
    }

    private object EvaluateLiteralSequenceExpression(GSharpLiteralSequence sequenceExpression, EvalScope scope)
    {
        var literalSequence = new LiteralSequence();
        foreach (var element in sequenceExpression.Elements)
        {
            var evaluatedElement = EvaluateExpression(element, scope);
            literalSequence.AddElement(evaluatedElement);
        }
        return literalSequence;
    }

    private object EvaluateInfiniteSequenceExpression(GSharpInfiniteSequence sequenceExpression, EvalScope scope)
    {
        var start = EvaluateExpression(sequenceExpression.First, scope);
        var infiniteSequence = new InfiniteSequence((int)start);
        return infiniteSequence;
    }

    private object EvaluateRangeSequenceExpression(GSharpRangeSequence sequenceExpression, EvalScope scope)
    {
        var start = EvaluateExpression(sequenceExpression.First, scope);
        var end = EvaluateExpression(sequenceExpression.Last, scope);
        var rangeSequence = new RangeSequence((int)start, (int)end);
        return rangeSequence;
    }

    private object EvaluateRayExpression(GSharpRayExpression rayExpression, EvalScope scope)
    {
        if (rayExpression.Coordinates is not null)
        {
            var pointA = EvaluateExpression(rayExpression.Coordinates[0], scope);
            var pointB = EvaluateExpression(rayExpression.Coordinates[1], scope);
            var ray = new Ray((Point)pointA, (Point)pointB);
            return ray;
        }
        var evalRay = new Ray();
        scope.AddVariable(rayExpression.Identifier, evalRay);
        return evalRay;
    }

    private object EvaluateSegmentExpression(GSharpSegmentExpression segmentExpression, EvalScope scope)
    {
        if (segmentExpression.Coordinates is not null)
        {
            var pointA = EvaluateExpression(segmentExpression.Coordinates[0], scope);
            var pointB = EvaluateExpression(segmentExpression.Coordinates[1], scope);
            var segment = new Segment((Point)pointA, (Point)pointB);
            return segment;
        }
        var evalLine = new Segment();
        scope.AddVariable(segmentExpression.Identifier, evalLine);
        return evalLine;
    }

    private object EvaluateCircleExpression(GSharpCircleExpression circleExpression, EvalScope scope)
    {
        if (circleExpression.Coordinates is not null)
        {
            var center = EvaluateExpression(circleExpression.Coordinates[0], scope);
            var radius = EvaluateExpression(circleExpression.Coordinates[1], scope);
            var circle = new Circle((Point)center, (Measure)radius);
            return circle;
        }
        var evalCircle = new Circle();
        scope.AddVariable(circleExpression.Identifier, evalCircle);
        return evalCircle;
    }

    private object EvaluateArcExpression(GSharpArcExpression arcExpression, EvalScope scope)
    {
        if (arcExpression.Coordinates is not null)
        {
            var center = EvaluateExpression(arcExpression.Coordinates[0], scope);
            var start = EvaluateExpression(arcExpression.Coordinates[1], scope);
            var end = EvaluateExpression(arcExpression.Coordinates[2], scope);
            var radius = EvaluateExpression(arcExpression.Coordinates[3], scope);
            var arc = new Arc((Point)center, (Point)start, (Point)end, (Measure)radius);
            return arc;
        }
        var evalArc = new Arc();
        scope.AddVariable(arcExpression.Identifier, evalArc);
        return evalArc;
    }

    private object EvaluateLineExpression(GSharpLineExpression lineExpression, EvalScope scope)
    {
        if (lineExpression.Coordinates is not null)
        {
            var pointA = EvaluateExpression(lineExpression.Coordinates[0], scope);
            var pointB = EvaluateExpression(lineExpression.Coordinates[1], scope);
            var line = new Line((Point)pointA, (Point)pointB);
            return line;
        }
        var evalLine = new Line();
        scope.AddVariable(lineExpression.Identifier, evalLine);
        return evalLine;
    }
    private object EvaluatePointExpression(GSharpPointExpression pointExpression, EvalScope scope)
    {
        if (pointExpression.Coordinates is not null)
        {
            var firstValue = EvaluateExpression(pointExpression.Coordinates[0], scope);
            var secondValue = EvaluateExpression(pointExpression.Coordinates[1], scope);
            var point = new Point(Convert.ToDouble(firstValue), Convert.ToDouble(secondValue));
            return point;
        }
        scope.AddVariable(pointExpression.Identifier, pointExpression.Value);
        return pointExpression.Value;
    }
    private static object EvaluateFunctionReference(FunctionReference functionReference, EvalScope scope)
    {
        return functionReference.Eval(scope);
    }
    private object EvaluateFunctionCallExpression(FunctionCallExpression functionCallExpression, EvalScope scope)
    {
        var functionDeclaration = StandardLibrary.Functions[functionCallExpression.FunctionName];

        var parameters = functionCallExpression.Parameters;
        var arguments = functionDeclaration.Arguments;

        var functionCallScope = new EvalScope();
        foreach (var (arg, param) in arguments.Zip(parameters))
        {
            scope.AddChildScope(functionCallScope);
            var evaluatedParameter = EvaluateExpression(param, functionCallScope);
            functionCallScope.AddVariable(arg, evaluatedParameter);
        }

        var b = EvaluateExpression(functionDeclaration.FunctionBody, functionCallScope);
        return b;
    }
    private object EvaluateLetInExpression(Let_In_Expression letInExpression, EvalScope scope)
    {
        var inScope = EvaluateLetExpression(letInExpression.LetExpressions, scope);
        var inExpression = EvaluateExpression(letInExpression.InExpression, inScope);
        return inExpression;
    }
    private EvalScope EvaluateLetExpression(List<GSharpExpression> letExpressions, EvalScope scope)
    {
        var newScope = new EvalScope();

        foreach (var expression in letExpressions)
        {
            scope.AddChildScope(newScope);
            EvaluateExpression(expression, newScope);
        }
        return newScope;
    }

    private object EvaluateIfElseStatement(If_ElseStatement ifElseStatement, EvalScope scope)
    {
        var condition = EvaluateExpression(ifElseStatement.IfCondition, scope);

        if (condition is int v && v == 0 || condition is int v1 && v1 == 0 || condition is Undefined || condition is Sequence t && t.IsEmpty())
        {
            return EvaluateExpression(ifElseStatement.ElseClause, scope);
        }
        else
        {
            return EvaluateExpression(ifElseStatement.ThenStatement, scope);
        }
    }
    private object EvaluateUnaryExpression(GSharpUnaryExpression unary, EvalScope scope)
    {
        var value = EvaluateExpression(unary.InternalExpression, scope);
        if (unary.OperatorToken.Type == TokenType.MinusToken)
        {
            if ((int)value == 0) return value;
            return -(int)(value);
        }
        else if (unary.OperatorToken.Type == TokenType.PlusToken)
        {
            return value;
        }
        else if (unary.OperatorToken.Type == TokenType.NotToken)
        {
            return !(bool)value;
        }
        throw new Exception($"!SEMANTIC ERROR : Invalid unary operator {unary.OperatorToken}");
    }
    private object EvaluateBinaryExpression(GSharpBinaryExpression binaryExpression, EvalScope scope)
    {
        var left = EvaluateExpression(binaryExpression.Left, scope);
        var right = EvaluateExpression(binaryExpression.Right, scope);

        switch (binaryExpression.OperatorToken.Type)//Hacer los casos
        {
            case TokenType.PlusToken:
                if (left is Sequence s && right is Sequence p)
                    return SequenceSums(s, p);
                else if (left is Sequence && right is Undefined)
                    return left;
                else if (left is Undefined && right is Sequence || left is Undefined && right is Undefined)
                    return Undefined.Value;
                    if (left is int && right is int)
                        return (int)left + (int)right;
                    else if (left is Measure && right is Measure)
                        return GetMeasureSum((Measure)left, (Measure)right);
                throw new Exception("");

            case TokenType.MinusToken:
                if (left is Measure && right is Measure)
                    return GetMeasureDifference(left, right);
                if (left is int && right is int)
                    return (int)left - (int)right;
                throw new Exception("");

            case TokenType.MultiplyToken:
                if (left is Measure && right is int || left is int && right is Measure)
                    return GetMeasureMultiplication(left, right);
                if (left is int && right is int)
                    return (int)left * (int)right;
                throw new Exception("");

            case TokenType.DivisionToken:
                if (left is Measure && right is Measure)
                    return GetMeasureDivision(left, right);
                if (left is int)
                {
                    if ((int)left == 0)
                        throw new Exception("! SEMANTIC ERROR : Cannot divide by zero");
                    return (int)left / (int)right;
                }
                throw new Exception("");

            case TokenType.ModuleToken:
                return (int)left % (int)right;
            case TokenType.SingleAndToken:
                return (bool)left && (bool)right;
            case TokenType.SingleOrToken:
                return (bool)left || (bool)right;
            case TokenType.BiggerToken:
                return (int)left > (int)right;
            case TokenType.BiggerOrEqualToken:
                return (int)left >= (int)right;
            case TokenType.LowerToken:
                return (int)left < (int)right;
            case TokenType.LowerOrEqualToken:
                return (int)left <= (int)right;
            case TokenType.EqualToken:
                return Equals(left, right);
            case TokenType.NotEqualToken:
                return !Equals(left, right);
            case TokenType.ExponentialToken:
                return Pow((int)left, (int)right);
            default:
                throw new Exception($"! SEMANTIC ERROR : Unexpected binary operator {binaryExpression.OperatorToken}");
        }
    }

    private object SequenceSums(Sequence left, Sequence right)
    {
        var resultSequence = new LiteralSequence();
        foreach(var element in left)
        {
            resultSequence.AddElement(element);
        }
        foreach(var element in right)
        {
            resultSequence.AddElement(element);
        }
        return resultSequence;
    }

    private object GetMeasureDivision(object left, object right)
    {
        var leftValue = ((Measure)left).EuclideanDistance;
        var rightValue = ((Measure)right).EuclideanDistance;
        return (int)(leftValue / rightValue);
    }
    private object GetMeasureMultiplication(object left, object right)
    {
        if (left is Measure measure)
        {
            var leftValues = measure.EuclideanDistance;
            var rightValues = Math.Abs((int)right);
            return new Measure(leftValues * rightValues);
        }
        var leftValue = Math.Abs((int)right);
        var rightValue = ((Measure)right).EuclideanDistance;

        return new Measure(leftValue * rightValue);
    }
    private object GetMeasureDifference(object left, object right)
    {
        var leftValue = ((Measure)left).EuclideanDistance;
        var rightValue = ((Measure)right).EuclideanDistance;
        return new Measure(Math.Abs(leftValue - rightValue));
    }

    private static object GetMeasureSum(object left, object right)
    {
        var leftValue = ((Measure)left).EuclideanDistance;
        var rightValue = ((Measure)right).EuclideanDistance;
        return new Measure(leftValue + rightValue);
    }

    private object EvaluateLiteralExpression(GSharpLiteralExpression literal, EvalScope scope)
    {
        if (literal.LiteralToken.Type is TokenType.Identifier)
        {
            return scope.GetValue(literal.LiteralToken.Text);
        }
        return literal.Value;
    }
    private int Pow(int left, int right)
    {
        if (left == 0 && right == 0) throw new Exception($"!SEMANTIC ERROR : {left} pow to {right} is not defined");
        double a = Convert.ToDouble(left);
        double b = Convert.ToDouble(right);

        return (int)Math.Pow(a, b);
    }
    private object EvaluateAssignmentExpression(AssignmentExpression assignmentExpression, EvalScope scope)
    {
        var evaluatedExpression = EvaluateExpression(assignmentExpression.Expression, scope);

        if (evaluatedExpression is Sequence sequence)
        {
            int identifiersCount = assignmentExpression.Identifiers.Count;
            var index = 0;

            foreach (var element in sequence)
            {
                if (identifiersCount - 1 > index)
                {
                    var current = assignmentExpression.Identifiers[index];
                    if (current == "_")
                    {
                        index++;
                        continue;
                    }
                    scope.AddVariable(current, element);
                    index++;
                }
                else break;
            }

            while (index < identifiersCount - 1)
            {
                if (assignmentExpression.Identifiers[index] == "_") continue;
                scope.AddVariable(assignmentExpression.Identifiers[index], Undefined.Value);
                index++;
            }

            if (assignmentExpression.Identifiers[identifiersCount - 1] != "_")
                scope.AddVariable(assignmentExpression.Identifiers[index], sequence.RemainingSequence(index));
        }
        else
        {
            if (assignmentExpression.Identifiers[^1] != "_")
                scope.AddVariable(assignmentExpression.Identifiers[0], evaluatedExpression);
        }
        return evaluatedExpression;
    }
}
