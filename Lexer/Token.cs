public class Token
{
    public Token(TokenType type, string value, int line, int column)
    {
        Type = type;
        Value = value;
        Line = line;
        Column = column;
    }

    public TokenType Type{get;}
    public string Value{get;}
    public int Line{get;}
    public int Column{get;}

    public override string ToString() => $"Tipo de token: \"{Type}\" | Valor: \"{Value}\" | Linea {Line} | Columna {Column}";

    public static Dictionary<string, TokenType> Tokens = new Dictionary<string, TokenType>(){
        {"Spawn", TokenType.Spawn},
        {"Color", TokenType.Color},
        {"Size", TokenType.Size},
        {"DrawLine", TokenType.DrawLine},
        {"DrawCircle", TokenType.DrawCircle},
        {"DrawRectangle", TokenType.DrawRectangle},
        {"Fill", TokenType.Fill},
        {"GetActualX", TokenType.GetActualX},
        {"GetActualY", TokenType.GetActualY},
        {"GetCanvasSize", TokenType.GetCanvasSize},
        {"GetColorCount", TokenType.GetColorCount},
        {"IsBrushColor", TokenType.IsBrushColor},
        {"GetBrushSize", TokenType.GetBrushSize},
        {"IsCanvasColor", TokenType.IsCanvasColor},
        {"<-", TokenType.AssignationArrow},
        {"GoTo(", TokenType.GoTo},
        {"+", TokenType.Addition},
        {"-", TokenType.Substraction},
        {"/", TokenType.Division},
        {"*", TokenType.Multiplication},
        {"^", TokenType.Pow},
        {"%", TokenType.Module},
        {"and", TokenType.And},
        {"or", TokenType.Or},
        {"==", TokenType.Equals},
        {"!=", TokenType.Different},
        {">=", TokenType.MajorEqual},
        {"<=", TokenType.MinorEqual},
        {">", TokenType.Major},
        {"<", TokenType.Minor},
        {"(", TokenType.OpenParenthesis},
        {")", TokenType.ClosedParenthesis},
        {"[", TokenType.OpenBrackets},
        {"]", TokenType.ClosedBrackets},
        {"\"", TokenType.Quotations},
        {",", TokenType.Comma},

        {"\"Red\"", TokenType.Red},
        {"\"Blue\"", TokenType.Blue},
        {"\"Green\"", TokenType.Green},
        {"\"Yellow\"", TokenType.Yellow},
        {"\"Orange\"", TokenType.Orange},
        {"\"Purple\"", TokenType.Purple},
        {"\"Black\"", TokenType.Black},
        {"\"White\"", TokenType.White},
        {"\"Transparent\"", TokenType.Transparent}
    };
}

public enum TokenType{
    EOF, EOL,

    Spawn, Color, Size, DrawLine,
    DrawCircle, DrawRectangle, Fill,
    GetActualX, GetActualY, GetCanvasSize,
    GetColorCount, IsBrushColor, GetBrushSize,
    IsCanvasColor,

    AssignationArrow,

    GoTo,

    String, Number, Variable, _true, _false,

    Addition, Substraction, Division, Multiplication,
    Pow, Module, //division con resto

    And, Or,

    Equals, Different, MajorEqual, MinorEqual, Minor, Major,

    label,

    OpenParenthesis, ClosedParenthesis, OpenBrackets, ClosedBrackets,
    Quotations, Comma,

    Red, Blue, Green, Yellow, Orange, Purple, Black, White, Transparent,

    Unknown
}