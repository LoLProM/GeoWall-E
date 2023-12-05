namespace GSharpProject;

public enum TokenType
{
    //Tokens
    NumberToken,
    PlusToken,
    MinusToken,
    MultiplyToken,
    DivisionToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    EndOffLineToken,
    NotToken,
    LowerToken,
    BiggerToken,
    LowerOrEqualToken,
    BiggerOrEqualToken,
    NotEqualToken,
    ExponentialToken,
    SingleAndToken,
    SingleOrToken,
    ModuleToken,
    EqualToken,
    SingleEqualToken,
    WrongToken,
    LetToken,
    InToken,
    StringToken,
    ColonToken,
    Identifier,
    OpenBraceToken,
    CloseBraceToken,
    ThreePointsToken,
    END,

    //Keywords
    IfKeyword,
    ElseKeyword,
    ThenKeyword,

    // Expressions
    AssigmentExpression,
    LiteralExpression,
    BinaryExpression,
    UnaryExpression,
    ParenthesizedExpression,
    IfElseExpression,
    Point,
    Line,
    Circle,
    Ray,
    Segment,
    Arc,
}
