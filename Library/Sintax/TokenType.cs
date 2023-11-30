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
    SemiColonToken,

    //Keywords
    IfKeyword,
    ElseKeyword,
    ThenKeyword,
    AssigmentExpression,

    // Expressions
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
    OpenBraceToken,
    CloseBraceToken,
    ThreePointsToken,
    END
}
