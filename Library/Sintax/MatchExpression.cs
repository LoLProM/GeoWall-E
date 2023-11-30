using System.Collections.Generic;
using System.Xml.Serialization;

namespace GSharpProject;

public class MatchExpression : GSharpExpression
{
    public MatchExpression(List<string> identifiers, GSharpSequence gSharpSequence)
    {
        Identifiers = identifiers;
        GSharpSequence = gSharpSequence;
    }

    public List<string> Identifiers { get; }
    public GSharpSequence GSharpSequence { get; }
}