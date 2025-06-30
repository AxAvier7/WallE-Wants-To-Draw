using System;

//Clase similar a LexError pero que gestiona errores del Parser
public class ParseException : Exception
{
    public int Line { get; }
    public int Column { get; }

    public ParseException(string message, int line, int column)
        : base(message)
    {
        Line = line;
        Column = column;
    }
}