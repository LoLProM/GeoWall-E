namespace GSharpProject;
public class Token
{
    public string Text { get; }
    public int Position { get; }
    public object Value { get; }
    public TokenType Type { get; }
    public static TokenType FunctionDeclaration { get; internal set; }

    public Token(string text, TokenType type, int position, object value)
    {
        Text = text;
        Type = type;
        Position = position;
        Value = value;
    }
}
