namespace GSharpProject;

public class GSharpColorExpression : GSharpExpression
{
    public Token ColorKeyWord { get; }
    public string Color { get; }

    public GSharpColorExpression(Token colorKeyWord, string color)
    {
        Color = color;
        ColorKeyWord = colorKeyWord;
    }
    public override TokenType TokenType => TokenType.Color;

}
