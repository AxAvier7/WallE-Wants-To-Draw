using System;

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