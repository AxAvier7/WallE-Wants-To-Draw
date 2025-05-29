public class LexErrors
{
    public LexErrors(string message, int line, int column)
    {
        Message = message;
        Line = line;
        Column = column;
    }

    public string Message{get;}
    public int Line{get;}
    public int Column{get;}
    
    public override string ToString() => $"Error: {Message} en ({Line}, {Column})";
}