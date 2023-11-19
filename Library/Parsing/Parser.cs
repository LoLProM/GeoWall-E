using System;
using System.Collections.Generic;
using System.IO;

namespace GSharpProject;

class Parser
{
	private List<Token> tokens;
	// Comentario
	public Parser(string text)
	{
		var lexer = new Lexer(text);
		tokens = lexer.Tokens;
	}
	private Token CurrentToken => LookAhead(0);
	private Token NextToken => LookAhead(1);
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

	public ASTree Parse()
	{
		Scope scope = new Scope();
		var expression = ParseAny();
		var endOfFileToken = MatchToken(TokenType.EndOffLineToken);
		return new ASTree(expression, endOfFileToken);
	}
	private GSharpExpression ParseAny()
	{
		if (CurrentToken.Type is TokenType.Identifier)
		{
			return ParseFunctionDeclaration();
		}
		return ParseExpression();
	}

	private Point ParsePoint()
	{
		var pointKeyWord = MatchToken(TokenType.Point);
		var pointId = MatchToken(TokenType.Identifier);
		return new Point(pointId.Text);
	}
	private FunctionDeclarationExpression ParseFunctionDeclaration()
	{
		var functionName = MatchToken(TokenType.Identifier);
		var functionParameters = ParseParameters();
		var equalToken = MatchToken(TokenType.SingleEqualToken);
		var functionBody = ParseExpression();
		var functionDeclaration = new FunctionDeclarationExpression(functionName.Text, functionParameters, functionBody);

		if (!StandardLibrary.Functions.ContainsKey(functionName.Text))
		{
			StandardLibrary.Functions.Add(functionName.Text, functionDeclaration);
		}
		else
		{
			throw new Exception($"! FUNCTION ERROR : Function {functionName.Text} is already defined");
		}

		return functionDeclaration;
	}

	private List<string> ParseParameters()
	{
		MatchToken(TokenType.OpenParenthesisToken);
		var parameters = new List<string>();
		if (CurrentToken.Type is TokenType.CloseParenthesisToken)
		{
			TokenAhead();
			return parameters;
		}
		parameters.Add(CurrentToken.Text);
		TokenAhead();
		while (CurrentToken.Type == TokenType.ColonToken)
		{
			TokenAhead();
			if (CurrentToken.Type is not TokenType.Identifier)
			{
				throw new Exception($"! SEMANTIC ERROR : Parameters must be a valid identifier");
			}
			if (parameters.Contains(CurrentToken.Text))
			{
				throw new Exception($"! SEMANTIC ERROR : A parameter with the name '{CurrentToken.Text}' already exists insert another parameter name");
			}
			parameters.Add(CurrentToken.Text);
			TokenAhead();
		}
		TokenAhead();
		return parameters;
	}
	private GSharpExpression ParseFunctionCall(string identifier)
	{
		TokenAhead();
		var parameters = new List<GSharpExpression>();

		MatchToken(TokenType.OpenParenthesisToken);
		while (true)
		{
			if (CurrentToken.Type == TokenType.CloseParenthesisToken)
			{
				break;
			}
			var expression = ParseExpression();
			parameters.Add(expression);
			if (CurrentToken.Type == TokenType.ColonToken)
			{
				TokenAhead();
			}
		}
		MatchToken(TokenType.CloseParenthesisToken);

		return new FunctionCallExpression(identifier, parameters);
	}
	private GSharpExpression ParseIdentifierOrFunctionCall()
	{
		if (CurrentToken.Type == TokenType.Identifier
		&& LookAhead(1).Type == TokenType.OpenParenthesisToken)
		{
			return ParseFunctionCall(CurrentToken.Text);
		}
		return ParseIdentifier(CurrentToken);
	}
	private GSharpExpression ParseIdentifier(Token identifier)
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

		if (CurrentToken.Type == TokenType.EndOffLineToken)
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
				return ParseIdentifierOrFunctionCall();
			case TokenType.Point:
				return ParsePoint();
			default:
				throw new Exception("! SYNTAX ERROR : Invalid Expression");
		}
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
