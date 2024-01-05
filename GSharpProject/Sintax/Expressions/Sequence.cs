using System.Collections;

namespace GSharpProject;

//tipo secuencia todas las demas heredan de aqui saben sumarse tienen un count de elementos y tienen sus respectivos elementos ademas de q podemos iterar sobre una secuencia sin importar su tipo
public class Sequence : IEnumerable<object>
{
    public virtual IEnumerable<object> Elements {get;}

    public virtual int Count => Elements.Count();
    public bool IsEmpty()
    {
        return !Elements.Any();
    }

    public Sequence()
    {
        Elements = Enumerable.Empty<object>();
    }
    private Sequence(IEnumerable<object> elements)
    {
        Elements = elements;
    }

    public static Sequence operator +(Sequence sequenceA, Sequence sequenceB)
    {
        return new Sequence(sequenceA.Elements.Concat(sequenceB.Elements));
    }

    public IEnumerator<object> GetEnumerator()
    {
        return Elements.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public Sequence RemainingSequence(int index)
    {
        return new Sequence(Elements.Skip(index));
    }
}