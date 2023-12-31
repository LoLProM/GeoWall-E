namespace GSharpProject;

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

class Lexer
{
    public List<Token> Tokens = new List<Token>();
    int position = 0;
    public Lexer(string textInput)
    {
        textInput = Regex.Replace(textInput, @"\s+", " ");

        Regex SyntaxTokens = new(@"\d+[^\D]|^-{0,1}\d+$|^-{0,1}\d+\.\d+E(\+|-)\d+$|^-{0,1}\d+\.\d+$|\+|\-|\*|\^|%|\(|\)|(=>)|(>=)|(<=)|<[=]{0}|>[=]{0}|!=|;|,|={1,2}|!|\&|\||(\u0022([^\u0022\\]|\\.)*\u0022)|@|[a-zA-Z]+\w*|/|[^\(\)\+\-\*/\^%<>=!&\|,;\s]+");

        var matches = SyntaxTokens.Matches(textInput);

        foreach (Match match in matches)
        {
            var token = GetToken(match);
            Tokens.Add(token);
        }
        Tokens.Add(new Token("", TokenType.END, Tokens.Count - 1, null));
    }
    private Token GetToken(Match match)
    {
        if (IsNumber(match))
        {
            double.TryParse(match.Value, out var value);
            return new Token(match.Value, TokenType.NumberToken, match.Index, value);
        }
        else if (IsString(match))
        {
            return new Token(match.Value, TokenType.StringToken, match.Index, match.Value);
        }
        else if (IsEndOffLine(match))
        {
            return new Token(match.Value, TokenType.EndOffLineToken, match.Index, match.Value);
        }
        else if (IsLetToken(match))
        {
            return new Token(match.Value, TokenType.LetToken, match.Index, match.Value);
        }
        else if (IsInToken(match))
        {
            return new Token(match.Value, TokenType.InToken, match.Index, match.Value);
        }
        else if (IsIfKeyword(match))
        {
            return new Token(match.Value, TokenType.IfKeyword, match.Index, match.Value);
        }
        else if (IsElseKeyword(match))
        {
            return new Token(match.Value, TokenType.ElseKeyword, match.Index, match.Value);
        }
        else if (IsThenKeyword(match))
        {
            return new Token(match.Value, TokenType.ThenKeyword, match.Index, match.Value);
        }
        else if (IsPointObject(match))
        {
            return new Token(match.Value, TokenType.Point, match.Index, match.Value);
        }
        else if (IsLineObject(match))
        {
            return new Token(match.Value, TokenType.Line, match.Index, match.Value);
        }
        else if (IsCircleObject(match))
        {
            return new Token(match.Value, TokenType.Circle, match.Index, match.Value);
        }
        else if (IsRayObject(match))
        {
            return new Token(match.Value, TokenType.Ray, match.Index, match.Value);
        }
        else if (IsSegmentObject(match))
        {
            return new Token(match.Value, TokenType.Segment, match.Index, match.Value);
        }
        else if (IsIdentifier(match))
        {
            return new Token(match.Value, TokenType.Identifier, match.Index, match.Value);
        }

        switch (match.Value)
        {
            case "+":
                return new Token("+", TokenType.PlusToken, match.Index, null);
            case "-":
                return new Token("-", TokenType.MinusToken, match.Index, null);
            case "*":
                return new Token("*", TokenType.MultiplyToken, match.Index, null);
            case "/":
                return new Token("/", TokenType.DivisionToken, match.Index, null);
            case "(":
                return new Token("(", TokenType.OpenParenthesisToken, match.Index, null);
            case ")":
                return new Token(")", TokenType.CloseParenthesisToken, match.Index, null);
            case "{":
                return new Token("{", TokenType.OpenBraceToken, match.Index, null);
            case "}":
                return new Token("}", TokenType.CloseBraceToken, match.Index, null);
            case "!":
                return new Token("!", TokenType.NotToken, match.Index, null);
            case "<":
                return new Token("<", TokenType.LowerToken, match.Index, null);
            case ">":
                return new Token(">", TokenType.BiggerToken, match.Index, null);
            case "<=":
                return new Token("<=", TokenType.LowerOrEqualToken, match.Index, null);
            case ">=":
                return new Token(">=", TokenType.BiggerOrEqualToken, match.Index, null);
            case "!=":
                return new Token("!=", TokenType.NotEqualToken, match.Index, null);
            case "==":
                return new Token("==", TokenType.EqualToken, match.Index, null);
            case "=":
                return new Token("=", TokenType.SingleEqualToken, match.Index, null);
            case "&":
                return new Token("&", TokenType.SingleAndToken, match.Index, null);
            case "|":
                return new Token("|", TokenType.SingleOrToken, match.Index, null);
            case "^":
                return new Token("^", TokenType.ExponentialToken, match.Index, null);
            case "%":
                return new Token("%", TokenType.ModuleToken, match.Index, null);
            case ",":
                return new Token(",", TokenType.ColonToken, match.Index, null);
            case "...":
                return new Token("...", TokenType.ThreePointsToken, match.Index, null);
        }
        throw new Exception($"! LEXICAL ERROR : '{match.Value}' is not a valid token");
    }

    private bool IsSegmentObject(Match match)
    {
        if (match.Value == "segment")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsRayObject(Match match)
    {
        if (match.Value == "ray")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsCircleObject(Match match)
    {
        if (match.Value == "circle")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsLineObject(Match match)
    {
        if (match.Value == "line")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsThenKeyword(Match match)
    {
        if (match.Value == "then")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsPointObject(Match match)
    {
        if (match.Value == "point")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsElseKeyword(Match match)
    {
        if (match.Value == "else")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsIfKeyword(Match match)
    {
        if (match.Value == "if")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsIdentifier(Match match)
    {
        foreach (var letter in match.Value)
        {
            if (!char.IsLetter(letter))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsInToken(Match match)
    {
        if (match.Value == "in")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsLetToken(Match match)
    {
        if (match.Value == "let")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsEndOffLine(Match match)
    {
        if (match.Value == ";")
        {
            position++;
            return true;
        }
        return false;
    }

    private bool IsString(Match match)
    {
        var matchString = match.Value.ToString();
        var count = 0;

        foreach (var character in matchString)
        {
            if (character == '"')
                count++;
            continue;
        }
        if (count == 2)
        {
            position++;
            return true;
        }
        return false;
    }
    private bool IsNumber(Match match)
    {
        var count = 0;
        var position = 0;
        var separator = 0;

        while (char.IsDigit(match.Value[position]))
        {
            position++;
            count++;
            if (position >= match.Value.Length)
            {
                break;
            }
            if (match.Value[position] == '.')
            {
                separator++;
                count++;
                position++;
                continue;
            }
        }
        if (count != match.Value.Length || separator > 1)
        {
            return false;
        }
        else
        {
            position++;
            return true;
        }
    }
}

