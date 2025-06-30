using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

//Clase que se encarga de analizar el texto y generar los tokens
public class Lexer
{
    private readonly string input;
    private int position;
    private int line;
    private int column;
    public List<LexErrors> LexerErrors { get; }
    private readonly Regex[] regexPatterns = {
        new Regex(@"^(<-|==|!=|>=|<=|\|\||&&|!)"), //operadores logicos
        new Regex(@"^(Spawn|GoTo|Color|Size|DrawLine|DrawCircle|DrawRectangle|Fill|GetActualX|GetActualY|GetCanvasSize|GetColorCount|IsBrushColor|IsBrushSize|IsCanvasColor)\b"), //Metodos
        new Regex(@"^[a-zA-Z][a-zA-Z0-9_\-]*"), //cadenas alfanumericas para identificadores o variables
        new Regex(@"^-?\d+"), //numeros
        new Regex(@"^""[^""]*"""), //strings
        new Regex(@"^(\+|\-|\*|/|%|\^|!)"), //operadores aritmeticos
        new Regex(@"^(\[|\]|\(|\)|\,|=|<|>|:)") //simbolos
    };

    public Lexer(string Input)
    {
        input = Input!;
        position = 0;
        line = 1;
        column = 1;
        LexerErrors = new List<LexErrors>();
    }

    //Metodo que convierte el texto introducido en Tokens
    public List<Token> Tokenize(string input)
    {
        var tokens = new List<Token>();
        while (position < input.Length)
        {
            char current = input[position];
            bool isStartOfLine = IsStartOfLine(position, input);

            //si ahy un espacio en blanco se continua el ciclo
            if (char.IsWhiteSpace(input[position]))
            {
                AdvancePosition();
                continue;
            }

            //Si hay comentarios se ignoran
            if (ThereAreComments(current)) continue;

            bool matched = false;

            //Se itera entre todos los patrones de expresiones regulares para saber si el token actual coincide con alguno
            foreach (var regex in regexPatterns)
            {
                var match = regex.Match(input.Substring(position));
                if (match.Success && match.Index == 0)
                {
                    string value = match.Value;
                    int currentPositionBefore = position;

                    //Si coincide con un patron alfanumerico y esta al inicio de la linea
                    if (isStartOfLine && regex == regexPatterns[2])
                    {
                        int lookAheadPos = position + value.Length;
                        while (lookAheadPos < input.Length && char.IsWhiteSpace(input[lookAheadPos]))
                        {
                            lookAheadPos++;
                        }
                        //Si tiene una asignacion se guarda el token como variable
                        if (lookAheadPos + 1 < input.Length && input[lookAheadPos] == '<' && input[lookAheadPos + 1] == '-')
                        {
                            tokens.Add(new Token(TokenType.Variable, value, line, column));
                            position += value.Length;
                            column += value.Length;
                            matched = true;
                            break;
                        }
                    }
                    //Si tiene un patron alfanumerico pero no es una Keyword se guarda como etiqueta
                    if (!matched && isStartOfLine && regex == regexPatterns[2] && !IsCommandOrKeyword(value))
                    {
                        tokens.Add(new Token(TokenType.Label, value, line, column));
                        position += value.Length;
                        column += value.Length;
                        matched = true;
                        break;
                    }

                    //Se procesa el resto de tipos de token
                    MatchProcessing(match.Value, tokens);
                    position += match.Length;
                    column += match.Length;
                    matched = true;
                    break;
                }
            }

            // Si no se encontró un token válido se añade un error
            if (!matched)
            {
                LexerErrors.Add(new LexErrors($"Caracter invalido \"{input[position]}\"", line, column));
                AdvancePosition();
            }
        }

        //Se añade el token procesado
        tokens.Add(new Token(TokenType.EOF, "", line, column));
        return tokens;
    }

    //Metodo que procesa distintos tipos de tokens
    private void MatchProcessing(string value, List<Token> tokens)
    {
        if (Token.Tokens.TryGetValue(value, out TokenType type))
            tokens.Add(new Token(type, value, line, column));
        else if (value.StartsWith("\""))
            tokens.Add(new Token(TokenType.String, value, line, column));
        else if (EsID(value))
            tokens.Add(new Token(TokenType.Variable, value, line, column));
        else
            tokens.Add(new Token(TokenType.Number, value, line, position));
    }

    //Metodo para saber si el string dado tiene formato de variable
    private bool EsID(string value)
    {
        var idRegex = new Regex(@"^[a-zA-Z][a-zA-Z0-9_\-]*$");
        return idRegex.IsMatch(value);
    }

    //Metodo booleano para saber si hay comentarios (e ignorarlos)
    private bool ThereAreComments(char current)
    {
        if (position + 1 < input.Length && current == '/' && input[position + 1] == '/')
        {
            // LexerErrors!.Add(new LexErrors($"Commentaries Not Allowed", line, column));
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

    //Metodo booleano para saber si es el inicio de una linea
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

    //Metodo booleano para saber si lo que se analiza es una keyword
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

    //Avanca la posicion
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