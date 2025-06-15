using System.Collections.Generic;

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
        {"IsBrushSize", TokenType.IsBrushSize},
        {"IsCanvasColor", TokenType.IsCanvasColor},
        {"<-", TokenType.AssignationArrow},
        {"GoTo", TokenType.GoTo},
        {"+", TokenType.Addition},
        {"-", TokenType.Substraction},
        {"/", TokenType.Division},
        {"*", TokenType.Multiplication},
        {"^", TokenType.Pow},
        {"%", TokenType.Module},
        {"&&", TokenType.And},
        {"||", TokenType.Or},
        {"==", TokenType.Equals},
        {"!=", TokenType.Different},
        {"!", TokenType.Negation}, 
        { ">=", TokenType.MajorEqual},
        {"<=", TokenType.MinorEqual},
        {">", TokenType.Major},
        {"<", TokenType.Minor},
        {"true", TokenType._true},
        {"false", TokenType._false},
    
        { "(", TokenType.OpenParenthesis},
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
        {"\"Gray\"", TokenType.Gray},
        {"\"Pink\"", TokenType.Pink},
        {"\"LightBlue\"", TokenType.LightBlue},
        {"\"LightGreen\"", TokenType.LightGreen},
        {"\"Brown\"", TokenType.Brown},
        {"\"LightGray\"", TokenType.LightGray},
        {"\"Transparent\"", TokenType.Transparent}
    };
}

public enum TokenType{
    EOF, EOL,

    Spawn, Color, Size, DrawLine,
    DrawCircle, DrawRectangle, Fill,
    GetActualX, GetActualY, GetCanvasSize,
    GetColorCount, IsBrushColor, IsBrushSize,
    IsCanvasColor,

    AssignationArrow,

    GoTo,

    String, Number, Variable, _true, _false,

    Addition, Substraction, Division, Multiplication,
    Pow, Module, //division con resto

    And, Or,

    Equals, Different, MajorEqual, MinorEqual, Minor, Major, Negation,

    Label,

    OpenParenthesis, ClosedParenthesis, OpenBrackets, ClosedBrackets,
    Quotations, Comma,

    Red, Blue, Green, Yellow, Orange, Purple, Black, White, Gray, Pink, LightBlue, LightGreen, Brown, LightGray, Transparent,

    Unknown
}