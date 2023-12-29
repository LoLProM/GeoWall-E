
namespace GSharpProject;

public class InfiniteSequence : Sequence
{
    public InfiniteSequence(int start)
    {
        Start = start;
    }

    public override int Count => throw new Exception("");
    public override IEnumerable<object> Elements => GetInfiniteSequence();

    private IEnumerable<object> GetInfiniteSequence()
    {
        int index = Start;
        while (true)
        {
            yield return index;
            index++;
        }
    }

    public int Start { get; }

    public override Sequence RemainingSequence(int index)
    {
        return new InfiniteSequence(Start + index);
    }
}