namespace GSharpProject.Parsing;

public class SequenceOf : GSharpExpression
{
	public readonly string Id;
	private TokenType ElementsType { get; init; }

	public SequenceOf(string id, TokenType elementsType)
	{
		Id = id;
		ElementsType = elementsType;
	}
	public override TokenType TokenType => TokenType.TSequence;
	public override void CheckType(TypedScope typedScope)
	{
		switch (ElementsType)
		{
			case TokenType.Point:
			case TokenType.Segment:
			case TokenType.Line:
			case TokenType.Ray:
			case TokenType.Arc:
			case TokenType.Circle:
				ExpressionType = new SingleType(typeof(LiteralSequence));
				break;
			default:
			throw new Exception($"Invalid Sequence of {ElementsType}");
		}
	}
}
