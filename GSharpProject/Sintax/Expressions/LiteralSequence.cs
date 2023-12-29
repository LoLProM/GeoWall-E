namespace GSharpProject;
public class LiteralSequence : Sequence
{
    private List<object> sequenceElements = new();
    public override IEnumerable<object> Elements => sequenceElements;

    public void AddElement(object element)
    {
        sequenceElements.Add(element);
    }

    public override Sequence RemainingSequence(int index)
    {
        var remainingSequence = new LiteralSequence();
        for (int i = index; i < sequenceElements.Count; i++)
        {
            remainingSequence.AddElement(i);
        }
        return remainingSequence;
    }
}