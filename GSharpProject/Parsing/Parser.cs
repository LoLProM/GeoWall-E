using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace GSharpProject.Parsing;
class Parser
{
	private int position = 0;
	private readonly List<Token> Tokens;
	public Parser(string text)
	{
		var lexer = new Lexer(text);
		Tokens = lexer.Tokens;
	}
	private Token CurrentToken => LookAhead(0);
	private Token NextToken => LookAhead(1);

	private Token LookAhead(int offset)
	{
		var index = position + offset;
		if (index >= Tokens.Count)
		{
			return Tokens[index - 1];
		}
		return Tokens[index];
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
	public GSharpStatementsCollection Parse()
	{
		var statements = ParseGSharpStatementCollection();
		return statements;
	}
	private GSharpStatementsCollection ParseGSharpStatementCollection()
	{
		List<GSharpExpression> statements = new();
		while (CurrentToken.Type is not TokenType.END)
		{
			var expression = ParseExpression();
			statements.Add(expression);
			MatchToken(TokenType.EndOffLineToken);
		}
		return new GSharpStatementsCollection(statements);
	}

	private GSharpExpression ParseAssignmentExpression()
	{
		List<string> identifiers = ParseMatchIdentifiers();
		MatchToken(TokenType.SingleEqualToken);
		var expression = ParseExpression();
		return new AssignmentExpression(identifiers, expression);
	}

	private GSharpSequence ParseSequence()
	{
		var openKey = MatchToken(TokenType.OpenBraceToken);
		if (CurrentToken.Type is TokenType.CloseBraceToken)
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
			return new GSharpRangeSequence(openKey, (GSharpLiteralExpression)firstValue, (GSharpLiteralExpression)valueAfterThreePoints, closeKey);
		}

		var expressions = new List<GSharpExpression>()
		{
			firstValue
		};

		while (CurrentToken.Type is not TokenType.CloseBraceToken)
		{
			MatchToken(TokenType.ColonToken);
			expressions.Add(ParseExpression());
		}

		var close = MatchToken(TokenType.CloseBraceToken);
		return new GSharpLiteralSequence(openKey, expressions.ToArray(), close);
	}
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
	private bool IsAnInfiniteSequence()
	{
		var flag = CurrentToken.Type is TokenType.ThreePointsToken && LookAhead(1).Type is TokenType.CloseBraceToken;
		if (flag)
		{
			return flag;
		}
		return false;
	}
	private List<string> ParseMatchIdentifiers()
	{
		var identifiers = new List<string>
		{
			CurrentToken.Text
		};
		TokenAhead();
		while (CurrentToken.Type is TokenType.ColonToken)
		{
			MatchToken(TokenType.ColonToken);
			identifiers.Add(CurrentToken.Text);
			TokenAhead();
		}
		return identifiers;
	}
	private GSharpExpression ParseIdentifiers()
	{
		var identifier = ParseIdentifier(CurrentToken);

		if (CurrentToken.Type is TokenType.OpenParenthesisToken)
		{
			return ParseFunction(identifier);
		}
		return identifier;
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

		MatchToken(TokenType.SingleEqualToken);

		var functionBody = ParseExpression();
		var identifiers = expressions.Select(exp => ((GSharpLiteralExpression)exp).LiteralToken.Text).ToList();
		var functionDeclaration = new FunctionDeclarationExpression(identifier.LiteralToken.Text, identifiers, functionBody);
		var functionName = functionDeclaration.FunctionName;
		if (!StandardLibrary.Functions.ContainsKey(functionName))
		{
			StandardLibrary.Functions.Add(functionName, functionDeclaration);
		}
		else
		{
			throw new Exception($"! FUNCTION ERROR : Function {functionName} is already defined");
		}
		return functionDeclaration;
	}
	private GSharpLiteralExpression ParseIdentifier(Token identifier)
	{
		TokenAhead();
		return new GSharpLiteralExpression(identifier);
	}
	private GSharpExpression ParseLetInExpression()
	{
		MatchToken(TokenType.LetToken);
		var listOfExpressions = new List<GSharpExpression>();
		while (CurrentToken.Type is not TokenType.InToken)
		{
			var expression = ParseExpression();
			listOfExpressions.Add(expression);
			MatchToken(TokenType.EndOffLineToken);
		}
		MatchToken(TokenType.InToken);
		var inExpression = ParseExpression();
		return new Let_In_Expression(listOfExpressions, inExpression);
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
		if (IsMatchExpression())
		{
			return ParseAssignmentExpression();
		}

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
	private bool IsMatchExpression()
	{
		if (CurrentToken.Type is TokenType.Identifier || CurrentToken.Type is TokenType.Underscore)
		{
			int cont = 0;
			while (true)
			{
				if (LookAhead(cont + 1).Type is TokenType.ColonToken && LookAhead(cont + 2).Type is TokenType.Identifier || LookAhead(cont + 2).Type is TokenType.Underscore)
				{
					cont += 2;
					continue;
				}
				return LookAhead(cont + 1).Type is TokenType.SingleEqualToken;
			}
		}
		return false;
	}
	private GSharpExpression ParsePrimary()
	{
		return CurrentToken.Type switch
		{
			TokenType.LetToken => ParseLetInExpression(),
			TokenType.IfKeyword => ParseIfElseExpression(),
			TokenType.StringToken => ParseString(),
			TokenType.OpenParenthesisToken => ParseParenthesizedExpression(),
			TokenType.NumberToken => ParseNumber(),
			TokenType.DoubleNumber => ParseNumber(),
			TokenType.Measure => ParseMeasure(),
			TokenType.Identifier => ParseIdentifiers(),
			TokenType.Color => ParseColor(),
			TokenType.DrawKeyword => ParseDraw(),
			TokenType.Import => ParseImport(),
			TokenType.Point => ParsePoint(),
			TokenType.Line => ParseLine(),
			TokenType.Arc => ParseArc(),
			TokenType.Segment => ParseSegment(),
			TokenType.Circle => ParseCircle(),
			TokenType.Ray => ParseRay(),
			TokenType.OpenBraceToken => ParseSequence(),
			TokenType.Undefine => ParseUndefine(),

			_ => throw new Exception("! SYNTAX ERROR : Invalid Expression"),
		};
	}

	private GSharpExpression ParseUndefine()
	{
		var undefine = CurrentToken;
		MatchToken(TokenType.Undefine);
		return new GSharpLiteralExpression(undefine);
	}

	private DrawExpression ParseImport()
	{
		throw new NotImplementedException();
	}
	private GSharpExpression ParseMeasure()
	{
		MatchToken(TokenType.Measure);
		int countOfParameters = 0;
		var listOfIdentifiers = new List<GSharpExpression>();

		if (CurrentToken.Type is TokenType.OpenParenthesisToken)
		{
			MatchToken(TokenType.OpenParenthesisToken);
			var expression = ParseExpression();
			listOfIdentifiers.Add(expression);
			while (CurrentToken.Type is TokenType.ColonToken)
			{
				TokenAhead();
				var expressions = ParseExpression();
				listOfIdentifiers.Add(expressions);
				countOfParameters++;
				if (countOfParameters > 2) throw new Exception("Cannot have more than 2 parameters");
			}
		}

		MatchToken(TokenType.CloseParenthesisToken);
		return new MeasureExpression(listOfIdentifiers[0], listOfIdentifiers[1]);
	}
	private GSharpExpression ParsePoint()
	{
		var pointKeyword = MatchToken(TokenType.Point);

		if (CurrentToken.Type is TokenType.SequenceToken)
		{
			MatchToken(TokenType.SequenceToken);
			var id = MatchToken(TokenType.Identifier).Text;
			return new SequenceOf(id, TokenType.Point);
		}

		if (CurrentToken.Type is TokenType.OpenParenthesisToken)
		{
			var countOfParameters = 0;
			var listOfIdentifiers = new List<GSharpExpression>();
			MatchToken(TokenType.OpenParenthesisToken);
			var expression = ParseExpression();
			listOfIdentifiers.Add(expression);
			while (CurrentToken.Type is TokenType.ColonToken)
			{
				TokenAhead();
				var expressions = ParseExpression();
				listOfIdentifiers.Add(expressions);
				countOfParameters++;
				if (countOfParameters > 2) throw new Exception("Cannot have more than 2 parameters");
			}
			MatchToken(TokenType.CloseParenthesisToken);
			return new GSharpPointExpression(pointKeyword, listOfIdentifiers);
		}

		var pointId = MatchToken(TokenType.Identifier);
		return new GSharpPointExpression(pointKeyword, pointId.Text, new Point());
	}
	private GSharpExpression ParseLine()
	{
		var lineKeyword = MatchToken(TokenType.Line);

		if (CurrentToken.Type is TokenType.SequenceToken)
		{
			MatchToken(TokenType.SequenceToken);
			var id = MatchToken(TokenType.Identifier).Text;
			Random rand = new();
			var count = rand.Next();
			var lines = new Line[count];
			for (int i = 0; i < count; i++)
			{
				lines[i] = new Line();
			}
			return new SequenceOf(id, TokenType.Line);
		}

		if (CurrentToken.Type is TokenType.OpenParenthesisToken)
		{
			var countOfParameters = 0;

			var listOfIdentifiers = new List<GSharpExpression>();
			MatchToken(TokenType.OpenParenthesisToken);
			var expression = ParseExpression();
			listOfIdentifiers.Add(expression);
			while (CurrentToken.Type is TokenType.ColonToken)
			{
				TokenAhead();
				var expressions = ParseExpression();
				listOfIdentifiers.Add(expressions);
				countOfParameters++;
				if (countOfParameters > 2) throw new Exception("Cannot have more than 2 parameters");
			}
			MatchToken(TokenType.CloseParenthesisToken);
			return new GSharpLineExpression(lineKeyword, listOfIdentifiers);

		}

		var lineId = MatchToken(TokenType.Identifier);
		return new GSharpLineExpression(lineKeyword, lineId.Text);
	}
	private GSharpExpression ParseSegment()
	{
		var segmentKeyword = MatchToken(TokenType.Segment);

		if (CurrentToken.Type is TokenType.SequenceToken)
		{
			MatchToken(TokenType.SequenceToken);
			var id = MatchToken(TokenType.Identifier).Text;
			Random rand = new();
			var count = rand.Next();
			var segments = new Segment[count];
			for (int i = 0; i < count; i++)
			{
				segments[i] = new Segment();
			}
			return new SequenceOf(id, TokenType.Segment);
		}

		if (CurrentToken.Type is TokenType.OpenParenthesisToken)
		{
			var countOfParameters = 0;

			var listOfIdentifiers = new List<GSharpExpression>();
			MatchToken(TokenType.OpenParenthesisToken);
			var expression = ParseExpression();
			listOfIdentifiers.Add(expression);
			while (CurrentToken.Type is TokenType.ColonToken)
			{
				TokenAhead();
				var expressions = ParseExpression();
				listOfIdentifiers.Add(expressions);
				countOfParameters++;
				if (countOfParameters > 2) throw new Exception("Cannot have more than 2 parameters");
			}
			MatchToken(TokenType.CloseParenthesisToken);
			return new GSharpSegmentExpression(segmentKeyword, listOfIdentifiers);

		}
		var segmentId = MatchToken(TokenType.Identifier);
		return new GSharpSegmentExpression(segmentKeyword, segmentId.Text);
	}
	private GSharpExpression ParseRay()
	{
		var rayKeyword = MatchToken(TokenType.Ray);

		if (CurrentToken.Type is TokenType.SequenceToken)
		{
			MatchToken(TokenType.SequenceToken);
			var id = MatchToken(TokenType.Identifier).Text;
			Random rand = new();
			var count = rand.Next();
			var rays = new Ray[count];
			for (int i = 0; i < count; i++)
			{
				rays[i] = new Ray();
			}
			return new SequenceOf(id, TokenType.Ray);
		}

		if (CurrentToken.Type is TokenType.OpenParenthesisToken)
		{

			var countOfParameters = 0;
			var listOfIdentifiers = new List<GSharpExpression>();
			MatchToken(TokenType.OpenParenthesisToken);
			var expression = ParseExpression();
			listOfIdentifiers.Add(expression);
			while (CurrentToken.Type is TokenType.ColonToken)
			{
				TokenAhead();
				var expressions = ParseExpression();
				listOfIdentifiers.Add(expressions);
				countOfParameters++;
				if (countOfParameters > 2) throw new Exception("Cannot have more than 2 parameters");
			}
			MatchToken(TokenType.CloseParenthesisToken);
			return new GSharpRayExpression(rayKeyword, listOfIdentifiers);

		}
		var rayId = MatchToken(TokenType.Identifier);
		return new GSharpRayExpression(rayKeyword, rayId.Text);
	}
	private GSharpExpression ParseCircle()
	{
		var circleKeyWord = MatchToken(TokenType.Circle);

		if (CurrentToken.Type is TokenType.SequenceToken)
		{
			MatchToken(TokenType.SequenceToken);
			var id = MatchToken(TokenType.Identifier).Text;
			Random rand = new();
			var count = rand.Next();
			var circles = new Circle[count];
			for (int i = 0; i < count; i++)
			{
				circles[i] = new Circle();
			}
			return new SequenceOf(id, TokenType.Circle);
		}

		if (CurrentToken.Type is TokenType.OpenParenthesisToken)
		{
			var countOfParameters = 0;


			var listOfIdentifiers = new List<GSharpExpression>();
			MatchToken(TokenType.OpenParenthesisToken);
			var expression = ParseExpression();
			listOfIdentifiers.Add(expression);
			while (CurrentToken.Type is TokenType.ColonToken)
			{
				TokenAhead();
				var expressions = ParseExpression();
				listOfIdentifiers.Add(expressions);
				countOfParameters++;
				if (countOfParameters > 2) throw new Exception("Cannot have more than 2 parameters");
			}
			MatchToken(TokenType.CloseParenthesisToken);
			return new GSharpCircleExpression(circleKeyWord, listOfIdentifiers);
		}
		var circleId = MatchToken(TokenType.Identifier);
		return new GSharpCircleExpression(circleKeyWord, circleId.Text);
	}

	private GSharpExpression ParseArc()
	{
		var arcKeyWord = MatchToken(TokenType.Arc);

		if (CurrentToken.Type is TokenType.SequenceToken)
		{
			MatchToken(TokenType.SequenceToken);
			var id = MatchToken(TokenType.Identifier).Text;
			Random rand = new();
			var count = rand.Next();
			var arcs = new Arc[count];
			for (int i = 0; i < count; i++)
			{
				arcs[i] = new Arc();
			}
			return new SequenceOf(id, TokenType.Arc);
		}

		if (CurrentToken.Type is TokenType.OpenParenthesisToken)
		{
			var countOfParameters = 0;
			var listOfIdentifiers = new List<GSharpExpression>();
			MatchToken(TokenType.OpenParenthesisToken);
			var expression = ParseExpression();
			listOfIdentifiers.Add(expression);
			while (CurrentToken.Type is TokenType.ColonToken)
			{
				TokenAhead();
				var expressions = ParseExpression();
				listOfIdentifiers.Add(expressions);
				countOfParameters++;
				if (countOfParameters > 4) throw new Exception("Cannot have more than 4 parameters");
			}
			MatchToken(TokenType.CloseParenthesisToken);
			return new GSharpArcExpression(arcKeyWord, listOfIdentifiers);
		}
		var arcId = MatchToken(TokenType.Identifier);
		return new GSharpArcExpression(arcKeyWord, arcId.Text);
	}
	private GSharpExpression ParseColor()
	{
		MatchToken(TokenType.Color);
		WallEColors.ColorDraw!.Push(WallEColors._Colors[CurrentToken.Value.ToString()!]);
		return new GSharpVoidEx();
	}
	private DrawExpression ParseDraw()
	{
		MatchToken(TokenType.DrawKeyword);
		var expression = ParseExpression();
		string message = "";
		WallEColors.InitializeColor();
		if (CurrentToken.Type == TokenType.StringToken)
			message = CurrentToken.Text;

		Color color = WallEColors.ColorDraw.Peek();
		if (message == "")
			return new DrawExpression(expression, color);
		else return new DrawExpression(expression, message, color);
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
		if (CurrentToken.Type is TokenType.NumberToken)
		{
			var number = MatchToken(TokenType.NumberToken);
			var convertedNumber = Convert.ToInt32(number.Value);
			return new GSharpLiteralExpression(number, convertedNumber);
		}
		var dNumber = MatchToken(TokenType.DoubleNumber);
		var doubleNumber = Convert.ToDouble(dNumber.Value);
		return new GSharpLiteralExpression(dNumber, doubleNumber);
	}
}
