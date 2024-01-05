namespace GSharpProject;
//Tipo de secuencia literal
public class LiteralSequence : Sequence
{
    private readonly List<object> sequenceElements = new();
    public override IEnumerable<object> Elements => sequenceElements;
    public void AddElement(object element)
    {
        sequenceElements.Add(element);
    }
}