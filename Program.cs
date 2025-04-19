public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            StreamReader sr = new StreamReader("input.txt");
            string text = sr.ReadToEnd();
            var lexer = new Lexer(text);
            var tokens = lexer.Tokenize(text);
            foreach (var token in tokens)
            {
                Console.WriteLine(token);
            }

            if(lexer.LexerErrors!.Count>0)
                foreach(var error in lexer.LexerErrors)
                    Console.WriteLine(error);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}