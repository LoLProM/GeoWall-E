namespace Hulk.Biblioteca.Semantic
{
    public enum SemanticType
    {
        UnaryExpression,
        LiteralExpression,
        BinaryExpression,
        AsignacionVariable,
        VariableExpresion,
        DeclarationExpression,
        LetInContext,
        IfExpression,
        Parentesis,
        Declaracion,
        FunctionCall,
        FuncionDeclaration,
    }
    public enum GSharpTypes
    {
        Number,
        Boolean,
        Void,
        Identifier,
        LetExpresion,
        String,
        FunctionDeclaration,
        FuncionCall
    }
}