using System.Collections;

namespace GSharpProject;

public abstract class Sequence : IEnumerable<object>
{
    public abstract IEnumerable<object> Elements {get;}

    public abstract Sequence RemainingSequence(int index);

    public abstract int Count{get;}
    public bool IsEmpty()
    {
        return !Elements.Any();
    }

    public IEnumerator<object> GetEnumerator()
    {
        return Elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}