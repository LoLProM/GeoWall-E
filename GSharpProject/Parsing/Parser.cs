using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GSharpProject.Parsing;
class Parser
{
	private int position = 0;
	private List<Token> tokens;
	public Parser(string text)
	{
		var lexer = new Lexer(text);
		tokens = lexer.Tokens;
	}
	private Token CurrentToken => LookAhead(0);
	private Token NextToken => LookAhead(1);

	public HashSet<string> TemporalFunctions = new();

	private Token LookAhead(int offset)
	{
		var index = position + offset;
		if (index >= tokens.Count)
		{
			return tokens[index - 1];
		}
		return tokens[index];
	}
	private Token TokenAhead()
	{
		var currentToken = CurrentToken;
		position++;
		return currentToken;
	}
	private Token MatchToken(TokenType type)
	{
		if (CurrentToken.Type == type)
		{
			return TokenAhead();
		}
		throw new Exception($"!SYNTAX ERROR : Not find {type} after {LookAhead(-1).Type} {LookAhead(-1).Text} in position {position}");
	}
	public List<GSharpExpression> Parse()
	{
		var statements = ParseGSharpStatementCollection();
		return statements;
	}
	private List<GSharpExpression> ParseGSharpStatementCollection()
	{
		List<GSharpExpression> statements = new();
		while (CurrentToken.Type is not TokenType.END)
		{
			var expression = ParseExpression();
			statements.Add(expression);
			MatchToken(TokenType.EndOffLineToken);
		}
		return statements;
	}

	//TODO
	private GSharpExpression ParseMatchExpression()
	{
		List<string> identifiers = ParseParameters();

		MatchToken(TokenType.SingleEqualToken);
		var sequence = ParseSequence();
		return new AssignmentExpression(identifiers, sequence);
	}

	//TODO
	private GSharpSequence ParseSequence()
	{
		var openKey = MatchToken(TokenType.OpenBraceToken);
		if (CurrentToken.Type is TokenType.CloseParenthesisToken)
		{
			var closeKey = MatchToken(TokenType.CloseBraceToken);
			return new GSharpLiteralSequence(openKey, Array.Empty<GSharpPrimitive>(), closeKey);
		}

		var firstValue = ParseExpression();
		if (IsAnInfiniteSequence())
		{
			TokenAhead();//Comsume los ...
			var closeKey = MatchToken(TokenType.CloseBraceToken);
			return new GSharpInfiniteSequence(openKey, firstValue as GSharpLiteralExpression, closeKey);
		}
		if (IsARangeSequence())
		{
			var valueAfterThreePoints = ParseExpression();
			var closeKey = MatchToken(TokenType.CloseBraceToken);
			return new GSharpRangeSequence(openKey, firstValue as GSharpLiteralExpression,valueAfterThreePoints as GSharpLiteralExpression, closeKey);
		}

		var expressions = new List<GSharpExpression>()
		{
			firstValue
		};

		while(CurrentToken.Type is not TokenType.CloseParenthesisToken)
		{
			MatchToken(TokenType.ColonToken);
			expressions.Add(ParseExpression());
		}
		var close = MatchToken(TokenType.CloseParenthesisToken);
		return new GSharpLiteralSequence(openKey,expressions.ToArray(),close);
	}

	//TODO

	private bool IsARangeSequence()
	{
		var flag = CurrentToken.Type is TokenType.ThreePointsToken && LookAhead(1).Type is not TokenType.CloseBraceToken;
		if (flag)
		{
			MatchToken(TokenType.ThreePointsToken);
			return flag;
		}
		return false;
	}

	//TODO
	private bool IsAnInfiniteSequence()
	{
		var flag = CurrentToken.Type is TokenType.ThreePointsToken && LookAhead(1).Type is TokenType.CloseBraceToken;
		if (flag)
		{
			MatchToken(TokenType.ThreePointsToken);
			return flag;
		}
		return false;
	}

	//TODO
	public GSharpExpression ParseAssigment(List<GSharpLiteralExpression> identifiers)
	{
		if (CurrentToken.Type == TokenType.Identifier && LookAhead(1).Type == TokenType.SingleEqualToken)
		{
			var identifier = MatchToken(TokenType.Identifier);
			var equalToken = MatchToken(TokenType.SingleEqualToken);
			var assignment = ParseExpression();
			return new AssignmentExpression(identifier.Text, equalToken, assignment);
		}
		return ParseExpression();
	}

	// private FunctionDeclarationExpression ParseFunctionDeclaration()
	// {
	// 	var functionName = MatchToken(TokenType.Identifier);
	// 	TemporalFunctions.Add(functionName.Text);
	// 	var functionParameters = ParseParameters();
	// 	MatchToken(TokenType.SingleEqualToken);
	// 	var functionBody = ParseExpression();
	// 	var functionDeclaration = new FunctionDeclarationExpression(functionName.Text, functionParameters, functionBody);

	// 	if (!StandardLibrary.Functions.ContainsKey(functionName.Text))
	// 	{
	// 		StandardLibrary.Functions.Add(functionName.Text, functionDeclaration);
	// 	}
	// 	else
	// 	{
	// 		throw new Exception($"! FUNCTION ERROR : Function {functionName.Text} is already defined");
	// 	}
	// 	return functionDeclaration;
	// }
	private List<string> ParseParameters()
	{
		MatchToken(TokenType.OpenParenthesisToken);
		var coordinates = new List<string>();
		if (CurrentToken.Type is TokenType.CloseParenthesisToken)
		{
			TokenAhead();
			return coordinates;
		}
		coordinates.Add(CurrentToken.Text);
		TokenAhead();
		while (CurrentToken.Type == TokenType.ColonToken)
		{
			TokenAhead();
			if (CurrentToken.Type is not TokenType.Identifier)
			{
				throw new Exception($"! SEMANTIC ERROR : Parameters must be a valid identifier");
			}
			if (coordinates.Contains(CurrentToken.Text))
			{
				throw new Exception($"! SEMANTIC ERROR : A parameter with the name '{CurrentToken.Text}' already exists insert another parameter name");
			}
			coordinates.Add(CurrentToken.Text);
			TokenAhead();
		}
		TokenAhead();
		return coordinates;
	}
	// private GSharpExpression ParseFunctionCall(string identifier)
	// {
	// 	TokenAhead();
	// 	var coordinates = new List<GSharpExpression>();

	// 	MatchToken(TokenType.OpenParenthesisToken);
	// 	while (true)
	// 	{
	// 		if (CurrentToken.Type == TokenType.CloseParenthesisToken)
	// 		{
	// 			break;
	// 		}
	// 		var expression = ParseExpression();
	// 		coordinates.Add(expression);
	// 		if (CurrentToken.Type == TokenType.ColonToken)
	// 		{
	// 			TokenAhead();
	// 		}
	// 	}
	// 	MatchToken(TokenType.CloseParenthesisToken);

	// 	return new FunctionCallExpression(identifier, coordinates);
	// }
	private GSharpExpression ParseIdentifiers()
	{
		List<GSharpLiteralExpression> identifiers = new()
		{
			ParseIdentifier(CurrentToken)
		};
		while (CurrentToken.Type is TokenType.ColonToken)
		{
			var comma = MatchToken(TokenType.ColonToken);
			identifiers.Add(ParseIdentifier(CurrentToken));
		}

		if (identifiers.Count == 1 && CurrentToken.Type is TokenType.OpenParenthesisToken)
		{
			return ParseFunction(identifiers[0]);
		}

		if (CurrentToken.Type is TokenType.SingleEqualToken)
		{
			return ParseAssigment(identifiers);
		}

		if (identifiers.Count == 1)
		{
			return identifiers[0];
		}
		throw new Exception("Identifier must be valid");
	}

	private GSharpExpression ParseFunction(GSharpLiteralExpression identifier)
	{
		MatchToken(TokenType.OpenParenthesisToken);
		var expressions = new List<GSharpExpression>();

		if (CurrentToken.Type is not TokenType.CloseParenthesisToken)
		{
			expressions.Add(ParseExpression());
			while (CurrentToken.Type != TokenType.CloseParenthesisToken)
			{
				MatchToken(TokenType.ColonToken);
				expressions.Add(ParseExpression());
			}
		}

		MatchToken(TokenType.CloseParenthesisToken);

		if (CurrentToken.Type is not TokenType.SingleEqualToken)
		{
			return new FunctionCallExpression(identifier.LiteralToken.Text, expressions);
		}

		var functionBody = ParseExpression();
		var identifiers = expressions.Select(exp => ((GSharpLiteralExpression)exp).LiteralToken.Text).ToList();
		return new FunctionDeclarationExpression(identifier.LiteralToken.Text, identifiers, functionBody);
	}

	private GSharpLiteralExpression ParseIdentifier(Token identifier)
	{
		TokenAhead();
		return new GSharpLiteralExpression(identifier);
	}
	private GSharpExpression ParseLetInExpression()
	{
		var letKeyword = MatchToken(TokenType.LetToken);
		var letExpression = ParseLetExpression();
		var inKeyword = MatchToken(TokenType.InToken);
		var inExpression = ParseExpression();
		return new Let_In_Expression(letExpression, inExpression);
	}
	private LetExpression ParseLetExpression()
	{
		var identifier = MatchToken(TokenType.Identifier);
		var equal = MatchToken(TokenType.SingleEqualToken);
		var expression = ParseExpression();

		if (CurrentToken.Type == TokenType.EndOffLineToken && LookAhead(1).Type is not TokenType.InToken)
		{
			var comma = MatchToken(TokenType.EndOffLineToken);
			var letChildExpression = ParseLetExpression();
			return new LetExpression(identifier, expression, letChildExpression);
		}
		else
		{
			return new LetExpression(identifier, expression);
		}
	}
	private GSharpExpression ParseIfElseExpression()
	{
		var ifKeyword = MatchToken(TokenType.IfKeyword);
		var condition = ParseExpression();
		var thenKeyword = MatchToken(TokenType.ThenKeyword);
		var thenStatement = ParseExpression();
		MatchToken(TokenType.ElseKeyword);
		var elseStatement = ParseExpression();
		return new If_ElseStatement(ifKeyword, condition, thenStatement, elseStatement);
	}
	private GSharpExpression ParseExpression(int actualPrecedence = 0)
	{
		GSharpExpression left;
		var unaryOperatorPrecedence = TokensPrecedences.GetUnaryOperatorPrecedence(CurrentToken.Type);
		if (unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= actualPrecedence)
		{
			var operatorToken = TokenAhead();
			var expression = ParseExpression(unaryOperatorPrecedence);
			left = new GSharpUnaryExpression(operatorToken, expression);
		}
		else
		{
			left = ParsePrimary();
		}

		while (true)
		{
			var precedence = TokensPrecedences.GetBinaryOperatorPrecedence(CurrentToken.Type);
			if (precedence == 0 || precedence <= actualPrecedence)
			{
				break;
			}
			var operatorToken = TokenAhead();
			var right = ParseExpression(precedence);
			left = new GSharpBinaryExpression(left, operatorToken, right);
		}
		return left;
	}
	private GSharpExpression ParsePrimary()
	{
		switch (CurrentToken.Type)
		{
			case TokenType.LetToken:
				return ParseLetInExpression();
			case TokenType.IfKeyword:
				return ParseIfElseExpression();
			case TokenType.StringToken:
				return ParseString();
			case TokenType.OpenParenthesisToken:
				return ParseParenthesizedExpression();
			case TokenType.NumberToken:
				return ParseNumber();
			case TokenType.Identifier:
				return ParseIdentifiers();
			case TokenType.Point:
				return ParsePoint();
			case TokenType.Line:
				return ParseLine();
			case TokenType.Arc:
				return ParseArc();
			case TokenType.Segment:
				return ParseSegment();
			case TokenType.Circle:
				return ParseCircle();
			case TokenType.Ray:
				return ParseRay();
			default:
				throw new Exception("! SYNTAX ERROR : Invalid Expression");
		}
	}

	private GSharpExpression ParsePoint()
	{
		var pointKeyword = MatchToken(TokenType.Point);
		var pointId = MatchToken(TokenType.Identifier);

		if (LookAhead(0).Type is TokenType.SingleEqualToken)
		{
			var equalToken = MatchToken(TokenType.SingleEqualToken);
			var coordinates = ParseCoordinates();
			return new GSharpPointExpression(pointKeyword, pointId.Text, coordinates);
		}
		return new GSharpPointExpression(pointKeyword, pointId.Text);
	}
	private List<string> ParseCoordinates()
	{
		MatchToken(TokenType.OpenParenthesisToken);
		var coordinates = new List<string>();
		if (CurrentToken.Type is TokenType.CloseParenthesisToken)
		{
			TokenAhead();
			return coordinates;
		}
		coordinates.Add(CurrentToken.Text);
		TokenAhead();
		while (CurrentToken.Type == TokenType.ColonToken)
		{
			TokenAhead();
			if (CurrentToken.Type is not TokenType.NumberToken)
			{
				throw new Exception($"! SEMANTIC ERROR : Coordinates must be a number");
			}
			coordinates.Add(CurrentToken.Text);
			TokenAhead();
		}
		TokenAhead();
		return coordinates;
	}
	private GSharpExpression ParseLine()
	{
		var lineKeyword = MatchToken(TokenType.Line);
		var lineId = MatchToken(TokenType.Identifier);

		if (LookAhead(0).Type is TokenType.OpenParenthesisToken)
		{
			var parameters = ParseParameters();
			return new GSharpLineExpression(lineKeyword, parameters);
		}
		return new GSharpLineExpression(lineKeyword, lineId.Text);
	}
	private GSharpExpression ParseSegment()
	{
		var segmentKeyword = MatchToken(TokenType.Segment);
		var segmentId = MatchToken(TokenType.Identifier);

		if (LookAhead(0).Type is TokenType.OpenParenthesisToken)
		{
			var parameters = ParseParameters();
			return new GSharpSegmentExpression(segmentKeyword, segmentId.Text);
		}
		return new GSharpSegmentExpression(segmentKeyword, segmentId.Text);
	}
    private GSharpExpression ParseRay()
    {
        var rayKeyword = MatchToken(TokenType.Ray);
        if (LookAhead(1).Type is TokenType.Identifier)
        {
            var rayId = MatchToken(TokenType.Identifier);
            return new GSharpRayExpression(rayKeyword, rayId.Text);
        }
        var parameters = ParseParameters();
        return new GSharpRayExpression(rayKeyword, parameters);

    }
    private GSharpExpression ParseCircle()
    {
        var circleKeyWord = MatchToken(TokenType.Circle);
        if (LookAhead(1).Type is TokenType.Identifier)
        {
            var circleId = MatchToken(TokenType.Identifier);
            return new GSharpCircleExpression(circleKeyWord, circleId.Text);
        }
        var parameters = ParseParameters();
        return new GSharpCircleExpression(circleKeyWord, parameters);
    }

    private GSharpExpression ParseArc()
    {
        var arcKeyWord = MatchToken(TokenType.Arc);
        if (LookAhead(1).Type is TokenType.Identifier)
        {
            var arcId = MatchToken(TokenType.Identifier);
            return new GSharpCircleExpression(arcKeyWord, arcId.Text);
        }
        var parameters = ParseParameters();

        return new GSharpArcExpression(arcKeyWord, parameters);
    }
    private GSharpColorExpression ParseColor()
    {
        var colorKeyWord = MatchToken(TokenType.Color);
        var colorId = MatchToken(TokenType.Identifier);
        return new GSharpColorExpression(colorKeyWord, colorId.Text);
    }
	private GSharpExpression ParseParenthesizedExpression()
	{
		var left = TokenAhead();
		var expression = ParseExpression();
		var right = MatchToken(TokenType.CloseParenthesisToken);
		return new GSharpParenthesesExpression(left, expression, right);
	}
	private GSharpExpression ParseString()
	{
		var stringToken = MatchToken(TokenType.StringToken);
		var resultStringToken = stringToken.Text[1..^1];//Obtener el string sin comillas
		return new GSharpLiteralExpression(stringToken, resultStringToken);
	}
	private GSharpExpression ParseNumber()
	{
		var number = MatchToken(TokenType.NumberToken);
		var doubleNumber = Convert.ToDouble(number.Value);
		return new GSharpLiteralExpression(number, doubleNumber);
	}

}
