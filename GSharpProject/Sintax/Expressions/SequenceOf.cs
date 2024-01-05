namespace GSharpProject.Parsing;

public class SequenceOf : GSharpExpression
{
    public readonly string Id;
    public TokenType ElementsType { get; init; }

    public SequenceOf(string id, TokenType elementsType)
    {
        Id = id;
        ElementsType = elementsType;
    }
    public override TokenType TokenType => TokenType.TSequence;
    public override void CheckType(TypedScope typedScope)
    {
        Type contentType;
        switch (ElementsType)
        {
            case TokenType.Point:
                contentType = typeof(Point);
                break;
            case TokenType.Segment:
                contentType = typeof(Segment);
                break;
            case TokenType.Line:
                contentType = typeof(Line);
                break;
            case TokenType.Ray:
                contentType = typeof(Ray);
                break;
            case TokenType.Arc:
                contentType = typeof(Arc);
                break;
            case TokenType.Circle:
                contentType = typeof(Circle);
                break;
            default:
                throw new Exception($"Invalid Sequence of {ElementsType}");


        }
        ExpressionType = new CompoundType(typeof(LiteralSequence), new SingleType(contentType));
    }
}
