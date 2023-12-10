using GSharpProject;

namespace Hulk.Biblioteca.Semantic
{
    internal sealed class TypeCheckingTree
    {
        public TypeCheckingTree(List<string> variables, List<Semantic_Expression> statements)
        {
            Variables = variables;
            Statements = statements;
        }
        public List<string> Variables { get; }
        public List<Semantic_Expression> Statements{ get; }
    }
}