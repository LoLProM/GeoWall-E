
using System.Linq;

namespace GSharpProject;

public class RangeSequence : Sequence
{
    public RangeSequence(int start, int end)
    {
        Start = start;
        End = end;
    }

    public override IEnumerable<object> Elements => GetRangeEnumerator();
    private IEnumerable<object> GetRangeEnumerator()
    {
        foreach (var element in Enumerable.Range(Start, End - Start + 1))
        {
            yield return element;
        }
    }

    public int Start { get; }
    public int End { get; }

    public override Sequence RemainingSequence(int index)
    {
        return new RangeSequence(Start + index,End);
    }
}