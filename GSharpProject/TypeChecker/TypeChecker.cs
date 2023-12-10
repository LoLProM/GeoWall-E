using GSharpProject;
using GSharpProject.Parsing;
using Hulk.Biblioteca.Semantic;

internal class TypeChecker
{
    private Scope scope;
    public TypeChecker(Scope scopes)
    {
        scope = scopes;
    }
    public static TypeCheckingTree GetSemanticTypeTree(GSharpStatementsCollection statementsTree)
    {
        var listOfCheckingStatements = new List<Semantic_Expression>();
        List<string> variables = new(); 
        foreach (var statement in statementsTree.Statements)
        {
            var semanticStatement = new TypeChecker(new Scope());
            var expression = semanticStatement.CheckExpressions(statement);
            variables = semanticStatement.scope.GetVariables();
            listOfCheckingStatements.Add(expression);
        }
        return new TypeCheckingTree(variables,listOfCheckingStatements);
    }
    private Semantic_Expression CheckIfElseExpression(If_ElseStatement ifElseExpression)
    {
        var condition = CheckBooleanCondition(ifElseExpression.IfCondition, GSharpTypes.Boolean);
        var thenStatement = CheckExpressions(ifElseExpression.ThenStatement);
        if (ifElseExpression.ElseClause == null)
        {
            throw new Exception($"Expected else clause in if-else expression");
        }
        var elseClause = CheckExpressions(ifElseExpression.ElseClause);

        return new Semantic_IF_Expression(condition, thenStatement, elseClause);
    }
    private Semantic_Expression CheckBooleanCondition(GSharpExpression expression, GSharpTypes type)
    {
        var resultType = CheckExpressions(expression);
        if (resultType.Types != type)
        {
            throw new Exception($"The conditional expresion in the if statement is not boolean.");
        }
        return resultType;
    }
    private Semantic_Expression CheckLetIn(Let_In_Expression letInExpression)
    {
        return null;
    }
    private Semantic_Expression CheckParenthesizedExpression(GSharpParenthesesExpression expression)
    {
        return CheckExpressions(expression.InsideExpression);
    }
    private Semantic_Expression CheckExpressions(GSharpExpression expression)
    {
        switch (expression.TokenType)
        {
            case TokenType.ParenthesizedExpression:
                return CheckParenthesizedExpression((GSharpParenthesesExpression)expression);
            case TokenType.BinaryExpression:
                return CheckBinaryExpression((GSharpBinaryExpression)expression);
            case TokenType.UnaryExpression:
                return CheckUnaryExpression((GSharpUnaryExpression)expression);
            case TokenType.LiteralExpression:
                return CheckLiteralExpression((GSharpLiteralExpression)expression);
            case TokenType.Identifier:
                return CheckIdentifier((GSharpLiteralExpression)expression);
            case TokenType.AssignmentExpression:
                return CheckAssignmentExpression((AssignmentExpression)expression);
            case TokenType.FunctionCall:
                return CheckFunctionCall((FunctionCallExpression)expression);
            case TokenType.LetIn:
                return CheckLetIn((Let_In_Expression)expression);
            case TokenType.IfElseExpression:
                return CheckIfElseExpression((If_ElseStatement)expression);
            default:
                return null!;
        }
    }
    private Semantic_Expression CheckFunctionCall(FunctionCallExpression functionCallExpression)
    {
        var functionName = functionCallExpression.FunctionName;
        var functionCallParametersCount = functionCallExpression.Parameters.Count;
        var functionDeclarationArgumentsCount = StandardLibrary.Functions[functionName].Arguments.Count;
        List<Semantic_Expression> parameters = new();
        if (!StandardLibrary.Functions.ContainsKey(functionName))
        {
            throw new Exception($"Function '{functionName}' is not defined.");
        }
        if (functionDeclarationArgumentsCount != functionCallParametersCount)
        {
            throw new Exception($"!FUNCTION ERROR : Function {functionName} does not have {functionCallExpression.Parameters.Count} parameters but {StandardLibrary.Functions[functionCallExpression.FunctionName]?.Arguments.Count} parameters");
        }
        foreach (var parameter in functionCallExpression.Parameters)
        {
            var semantic = CheckExpressions(parameter);
            parameters.Add(semantic);
        }
        return new Semantic_FunctionCall(functionName, parameters);
    }

    private Semantic_Expression CheckLiteralExpression(GSharpLiteralExpression expression)
    {
        var value = expression.Value ?? 0;
        return new Semantic_LiteralExpression(value);
    }

    private Semantic_Expression CheckUnaryExpression(GSharpUnaryExpression unaryExpression)
    {
        var Operand = CheckExpressions(unaryExpression.InternalExpression);
        var Operator = Semantic_UnaryOperator.Semantic_Parse_UO(unaryExpression.OperatorToken.Type, Operand.Types);
        if (Operator == null)
        {
            throw new Exception($"Operator '{unaryExpression.OperatorToken.Text}' cannot be used with '{Operand.Types}'.");
        }
        return new Semantic_UnaryExpression(Operator, Operand);
    }

    private Semantic_Expression CheckBinaryExpression(GSharpBinaryExpression expression)
    {
        var left = CheckExpressions(expression.Left);
        var right = CheckExpressions(expression.Right);
        var expressionOperator = Semantic_BinaryOperator.Semantic_Parse_BO(expression.OperatorToken.Type, left.Types, right.Types);

        if (expressionOperator == null)
        {
            throw new Exception($"Operator '{expression.OperatorToken.Text}' cannot be used between '{left.Types}' and '{right.Types}'.");
        }
        return new Semantic_BinaryExpression(left, expressionOperator, right);
    }

    private Semantic_Expression CheckAssignmentExpression(AssignmentExpression assignmentExpression)
    {
        string identifierName = assignmentExpression.Identifiers[0];
        Semantic_Expression semantic_Expression = CheckExpressions(assignmentExpression.Expression);

        if (!scope.Contains(identifierName))
        {
            throw new Exception($"'{identifierName}' does not exist in the current context.");
        }
        return new Semantic_AsignacionVariable(identifierName, semantic_Expression);
    }

    private Semantic_Expression CheckIdentifier(GSharpLiteralExpression expression)
    {
        string idName = expression.LiteralToken.Text;

        if (!scope.Contains(idName))
        {
            throw new Exception($"'{idName}' does not exist in the current context.");
        }
        return new Semantic_LiteralExpression(expression.Value);
    }

    // public static Dictionary<string, Semantic_Expression> Conecta_CuerpoFuncion(Dictionary<string, FunctionDeclarationExpression> functions)
    // {
    //     var functionDeclarationDictionary = functions;
    //     var FunctionsBodys = new Dictionary<string, Semantic_Expression>();
    //     foreach (var function in functions)
    //     {
    //         var semantic_Object = new TypeChecker(null);
    //         foreach (var param in function.Value.Arguments)
    //         {
    //             semantic_Object.Ambito.Try_DeclararVariable(new VariableSymbol(param.Text, param.GetType(), GSharpTypes.Number));
    //         }
    //         var expresion = semantic_Object.ConectaExpresion(funcion.Value.Cuerpo);
    //         FunctionsBodys.Add(funcion.Value.Identificador.Text, expresion);
    //     }
    //     return FunctionsBodys;
    // }
}