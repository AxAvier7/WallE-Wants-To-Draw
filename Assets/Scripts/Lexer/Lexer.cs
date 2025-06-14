using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class Lexer
{
    private readonly string input;
    private int position;
    private int line;
    private int column;
    public List<LexErrors> LexerErrors {get;}
    private readonly Regex[] regexPatterns = {
        new Regex(@"^(<-|==|!=|>=|<=|\|\||&&|!)"), //operadores logicos
        new Regex(@"^(Spawn|GoTo|Color|Size|DrawLine|DrawCircle|DrawRectangle|Fill|GetActualX|GetActualY|GetCanvasSize|GetColorCount|IsBrushColor|IsBrushSize|IsCanvasColor)\b"), //Metodos
        new Regex(@"^[a-zA-Z][a-zA-Z0-9_\-]*"), //cadenas alfanumericas para identificadores o variables
        new Regex(@"^-?\d+"), //numeros
        new Regex(@"^""[^""]*"""), //strings
        new Regex(@"^(\+|\-|\*|/|%|\^|!)"), //operadores aritmeticos
        new Regex(@"^(\[|\]|\(|\)|\,|=|<|>|:)") //simbolos
    };

    public Lexer(string Input){
        input = Input!;
        position = 0;
        line = 1;
        column = 1;
        LexerErrors = new List<LexErrors>();
    }

    public List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        while(position < input.Length)
        {
            char current = input[position];
            bool isStartOfLine = IsStartOfLine(position, input);

            if (char.IsWhiteSpace(input[position]))
            {
                AdvancePosition();
                continue;
            }
            
            if (ThereAreComments(current)) continue;

            bool matched = false;
            
            foreach (var regex in regexPatterns)
            {
                var match = regex.Match(input.Substring(position));
                if (match.Success && match.Index == 0)
                {
                    string value = match.Value;
                    int currentPositionBefore = position;

                    if (isStartOfLine && regex == regexPatterns[2])
                    {
                        int lookaheadPos = position + value.Length;
                        while (lookaheadPos < input.Length && char.IsWhiteSpace(input[lookaheadPos]))
                        {
                            lookaheadPos++;
                        }
                        if (lookaheadPos + 1 < input.Length && input[lookaheadPos] == '<' && input[lookaheadPos + 1] == '-')
                        {
                            tokens.Add(new Token(TokenType.Variable, value, line, column));
                            position += value.Length;
                            column += value.Length;
                            matched = true;
                            break;
                        }
                    }
                    if (!matched && isStartOfLine && regex == regexPatterns[2] && !IsCommandOrKeyword(value))
                    {
                        tokens.Add(new Token(TokenType.Label, value, line, column));
                        position += value.Length;
                        column += value.Length;
                        matched = true;
                        break;
                    }

                    MatchProcessing(match.Value, tokens);
                    position += match.Length;
                    column += match.Length;
                    matched = true;
                    break;
                }
            }

            if (!matched)
            {
                LexerErrors.Add(new LexErrors($"Caracter invalido \"{input[position]}\"", line, column));
                AdvancePosition();
            }
        }
        
        tokens.Add(new Token(TokenType.EOF, "", line, column));
        return tokens;
    }

    private void MatchProcessing(string value, List<Token> tokens)
    {
        if(Token.Tokens.TryGetValue(value, out TokenType type))
            tokens.Add(new Token(type, value, line, column));
        else if(value.StartsWith("\""))
            tokens.Add(new Token(TokenType.String, value, line, column));
        else if(EsID(value))
            tokens.Add(new Token (TokenType.Variable, value, line, column));
        else
            tokens.Add(new Token(TokenType.Number, value, line, position));
    }

    private bool EsID(string value)
    {
        var idRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9_\-]*$");
        return idRegex.IsMatch(value);
    }

    private bool ThereAreComments(char current)
    {
        if(position + 1 < input.Length && current == '/' && input[position+1] == '/')
        {
            LexerErrors!.Add(new LexErrors($"Commentaries Not Allowed", line, column));
            while (position < input.Length && input[position] != '\n')
            {
                position++;
                column++;
            }
            
            if (position < input.Length && input[position] == '\n')
            {
                line++;
                column = 1;
                position++;
            }
            return true;
        }
        return false;
    }

    private bool IsStartOfLine(int position, string input)
    {
        if (position == 0) return true;
        
        for (int i = position - 1; i >= 0; i--)
        {
            if (input[i] == '\n') return true;
            if (!char.IsWhiteSpace(input[i])) return false;
        }
        return true;
    }

    private bool IsCommandOrKeyword(string value)
    {
        string[] reservedWords = {
            "Spawn", "GoTo", "Color", "Size", "DrawLine", "DrawCircle", 
            "DrawRectangle", "Fill", "GetActualX", "GetActualY", "GetCanvasSize", 
            "GetColorCount", "IsBrushColor", "IsBrushSize", "IsCanvasColor",
            "and", "or", "true", "false"
        };

        return reservedWords.Contains(value);
    }

    private void AdvancePosition()
    {
        if (position < input.Length && input[position] == '\n')
        {
            line++;
            column = 1;
        }
        else column++;
        position++;
    }
}