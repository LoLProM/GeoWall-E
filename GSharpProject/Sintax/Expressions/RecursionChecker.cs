namespace GSharpProject.Parsing;

public class RecursionChecker
{
    public HashSet<string> CalledFunction = new();
    public bool CheckRecursiveFunctions(FunctionDeclarationExpression functionDeclarationExpression)
    {
        CalledFunction.Add(functionDeclarationExpression.FunctionName);
        return CheckRecursiveFunctions(functionDeclarationExpression.FunctionBody);
    }

    private bool CheckRecursiveFunctions(GSharpExpression expression)
    {
        if (expression is GSharpStatementsCollection gsharpStatementsCollection)
        {
            return gsharpStatementsCollection.Statements.Any(statement => CheckRecursiveFunctions(statement));
        }
        if (expression is AssignmentExpression assignmentExpression)
        {
            return CheckRecursiveFunctions(assignmentExpression.Expression);
        }

        if (expression is GSharpBinaryExpression gSharpBinaryExpression)
        {
            return CheckRecursiveFunctions(gSharpBinaryExpression.Left) || CheckRecursiveFunctions(gSharpBinaryExpression.Right);
        }

        if (expression is GSharpUnaryExpression gSharpUnaryExpression)
        {
            return CheckRecursiveFunctions(gSharpUnaryExpression.InternalExpression);
        }

        if (expression is GSharpParenthesesExpression gSharpParenthesesExpression)
        {
            return CheckRecursiveFunctions(gSharpParenthesesExpression.InsideExpression);
        }

        if (expression is If_ElseStatement if_ElseStatement)
        {
            return CheckRecursiveFunctions(if_ElseStatement.ThenStatement) || CheckRecursiveFunctions(if_ElseStatement.ElseClause) || CheckRecursiveFunctions(if_ElseStatement.IfCondition);
        }

        if (expression is Let_In_Expression let_In_Expression)
        {
            return let_In_Expression.LetExpressions.Any(statement => CheckRecursiveFunctions(statement)) || CheckRecursiveFunctions(let_In_Expression.InExpression);
        }

        if (expression is GSharpLiteralSequence gSharpLiteralSequence)
        {
            return gSharpLiteralSequence.Elements.Any(statement => CheckRecursiveFunctions(statement));
        }

        if (expression is GSharpPointExpression gSharpPointExpression)
        {
            return gSharpPointExpression.Coordinates.Any(statement => CheckRecursiveFunctions(statement));
        }
        if (expression is GSharpCircleExpression gSharpCircleExpression)
        {
            return gSharpCircleExpression.Coordinates.Any(statement => CheckRecursiveFunctions(statement));
        }

        if (expression is GSharpLineExpression gSharpLineExpression)
        {
            return gSharpLineExpression.Coordinates.Any(statement => CheckRecursiveFunctions(statement));
        }
        if (expression is GSharpArcExpression gSharpArcExpression)
        {
            return gSharpArcExpression.Coordinates.Any(statement => CheckRecursiveFunctions(statement));
        }
        if (expression is GSharpSegmentExpression gSharpSegmentExpression)
        {
            return gSharpSegmentExpression.Coordinates.Any(statement => CheckRecursiveFunctions(statement));
        }
        if (expression is GSharpRayExpression gSharpRayExpression)
        {
            return gSharpRayExpression.Coordinates.Any(statement => CheckRecursiveFunctions(statement));
        }

        if (expression is MeasureExpression measureExpression)
        {
            return CheckRecursiveFunctions(measureExpression.PointA) || CheckRecursiveFunctions(measureExpression.PointB);
        }

        if (expression is FunctionCallExpression functionCallExpression)
        {
            if (StandardLibrary.Functions.ContainsKey(functionCallExpression.FunctionName))
            {
                return true;
            }
            CalledFunction.Add(functionCallExpression.FunctionName);
            var functionBody = StandardLibrary.Functions[functionCallExpression.FunctionName];
            return CheckRecursiveFunctions(functionBody);
        }
        return false;
    }
}