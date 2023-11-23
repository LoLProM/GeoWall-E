namespace GSharpProject;

class StatementCollection : GSharpExpression
{
    public StatementCollection(GSharpExpression statement,StatementCollection? childCollection) 
    {
        Statement = statement;
        ChildCollection = childCollection;
    }

    public GSharpExpression Statement { get; }
    public StatementCollection? ChildCollection { get; }
}