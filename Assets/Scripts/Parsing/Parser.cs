using System;
using System.Collections.Generic;
using UnityEngine;

//Esta es la clase que se encarga de convertir los tokens generados por el lexer en un AST
public class Parser : MonoBehaviour
{
    private List<Token> tokens;
    private int currentPosition;
    private Token currentToken;
    private int currentLine;
    private int currentColumn;
    public List<ParseException> errors = new List<ParseException>();

    //Metodo que recibe todos los tokens y devuelve una lista de nodos
    public List<ASTNode> Parse(List<Token> tokens)
    {
        this.tokens = tokens;
        tokens.RemoveAt(tokens.Count - 1);
        currentPosition = 0;

        if (tokens == null || tokens.Count == 0)
        {
            return new List<ASTNode>();
        }

        currentToken = tokens[0];
        currentLine = currentToken.Line;
        currentColumn = currentToken.Column;

        List<ASTNode> nodes = new List<ASTNode>();

        //Iteramos sobre los tokens y creamos nodos segun el token que se analice
        while (currentPosition < tokens.Count && currentToken != null)
        {
            try //Intenta parsear el token actual
            {
                if (currentToken == null) break;
                if (currentToken.Type == TokenType.Label) //Si es una etiqueta
                {
                    nodes.Add(ParseLabel());
                }
                else if (IsCommandToken(currentToken.Type)) //Si es un comando
                {
                    nodes.Add(ParseCommand());
                }
                else if (currentToken.Type == TokenType.Variable) //Si es una variable
                {
                    nodes.Add(ParseAssignmentOrExpression());
                }
                else if (currentToken.Type == TokenType.GoTo) //Si es un GoTo
                {
                    nodes.Add(ParseGoTo());
                }
                else
                {
                    nodes.Add(ParseExpressionStatement()); //Si es cualquier otra expresion
                }
            }
            catch (ParseException ex) //Si no puede parsear el token actual, lanza una excepcion
            {
                errors.Add(ex);
                SkipToNextLine();
            }
            if (currentPosition >= tokens.Count)
            {
                currentToken = null;
            }
        }
        return nodes;
    }

    //Metodo que parsea los comandos
    private Command ParseCommand()
    {
        //Hacemos un switch para parsear el comando segun su tipo
        return currentToken.Type switch
        {
            TokenType.Spawn => ParseSpawn(),
            TokenType.Color => ParseColor(),
            TokenType.Size => ParseSize(),
            TokenType.DrawLine => ParseDrawLine(),
            TokenType.DrawCircle => ParseDrawCircle(),
            TokenType.DrawRectangle => ParseDrawRectangle(),
            TokenType.Fill => ParseFill(),
            _ => throw new ParseException($"Unexpected command: {currentToken.Type}", currentToken.Line, currentToken.Column)
        };
    }

    #region CommandParsing
    //Metodo que parsea el comando Spawn
    private Command ParseSpawn()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();

        Consume(TokenType.OpenParenthesis, $"Expected '(' after 'Spawn', recieved {currentToken}");
        ExpressionNode x = ParseExpression();
        Consume(TokenType.Comma, "Expected ',' after x coordinate");
        ExpressionNode y = ParseExpression();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after y coordinate");
        return new Spawn(x, y, line, column);
    }

    //Metodo que parsea el comando Color
    private Command ParseColor()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;

        Advance();
        Consume(TokenType.OpenParenthesis, "Expected '(' after 'Color'");
        string color;
        if (ColorManager.ColorTokenToString.TryGetValue(tokens[currentPosition].Type, out color))
        {
            Advance();
        }
        else
        {
            throw new Exception($"Expected a color token, found: {tokens[currentPosition]}");
        }

        Consume(TokenType.ClosedParenthesis, "Expected ')' after color string");
        return new Color(color, line, column);
    }

    //Metodo que parsea el comando Size
    private Command ParseSize()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();

        Consume(TokenType.OpenParenthesis, "Expected '(' after 'Size'");
        ExpressionNode size = ParseExpression();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after size number");
        return new Size(size, line, column);
    }

    //Metodo que parsea el comando DrawLine
    private Command ParseDrawLine()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();

        Consume(TokenType.OpenParenthesis, "Expected '(' after 'Spawn'");
        ExpressionNode dirX = ParseExpression();
        Consume(TokenType.Comma, "Expected ',' after x direction");
        ExpressionNode dirY = ParseExpression();
        Consume(TokenType.Comma, "Expected ',' after y direction");
        ExpressionNode distance = ParseExpression();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after distance");
        return new DrawLine(dirX, dirY, distance, line, column);
    }

    //Metodo que parsea el comando DrawCircle
    private Command ParseDrawCircle()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();

        Consume(TokenType.OpenParenthesis, "Expected '(' after 'DrawCircle'");
        ExpressionNode dirX = ParseExpression();
        Consume(TokenType.Comma, "Expected ',' after dirX");
        ExpressionNode dirY = ParseExpression();
        Consume(TokenType.Comma, "Expected ',' after dirY");
        ExpressionNode radius = ParseExpression();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after radius");
        return new DrawCircle(dirX, dirY, radius, line, column);
    }

    //Metodo que parsea el comando DrawRectangle
    private Command ParseDrawRectangle()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();

        Consume(TokenType.OpenParenthesis, "Expected '(' after 'DrawRectangle'");
        ExpressionNode dirX = ParseExpression();
        Consume(TokenType.Comma, "Expected ',' after x coordinate");
        ExpressionNode dirY = ParseExpression();
        Consume(TokenType.Comma, "Expected ',' after y coordinate");
        ExpressionNode distance = ParseExpression();
        Consume(TokenType.Comma, "Expected ',' after distance");
        ExpressionNode width = ParseExpression();
        Consume(TokenType.Comma, "Expected ',' after width");
        ExpressionNode height = ParseExpression();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after height");
        Debug.Log($"Parsed DrawRectangle command with size ({width}, {height})");
        return new DrawRectangle(dirX, dirY, distance, width, height, line, column);
    }

    //Metodo que parsea el comando Fill
    private Command ParseFill()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();

        Consume(TokenType.OpenParenthesis, "Expected '(' after Fill");
        Consume(TokenType.ClosedParenthesis, "Expected ')' after '('");
        return new Fill(line, column);
    }

    #endregion

    #region Expression Parsing
    //Metodo que parsea cualquier expresion
    private Command ParseExpressionStatement()
    {
        ExpressionNode expr = ParseExpression();
        return new ExpressionStatement(expr, currentToken.Line, currentToken.Column);
    }

    //Metodo alias para ParseBooleanExpression
    private ExpressionNode ParseExpression()
    {
        return ParseBooleanExpression();
    }

    //Metodo que parsea las expresiones aritmeticas
    private ExpressionNode ParseArithmeticExpression()
    {
        ExpressionNode left = ParseTerm();
        while (currentToken != null && currentToken.Type == TokenType.Addition || currentToken.Type == TokenType.Substraction)
        {
            Token op = currentToken;
            Advance();
            ExpressionNode right = ParseTerm();
            left = new ArithmeticBinaryExpressionNode(left, right, op.Type, currentLine, currentColumn);
        }
        return left;
    }

    //Metodo que parsea los terminos de las expresiones aritmeticas
    private ExpressionNode ParseTerm()
    {
        ExpressionNode left = ParseFactor();
        while (currentToken != null && (currentToken.Type == TokenType.Multiplication || currentToken.Type == TokenType.Division || currentToken.Type == TokenType.Module || currentToken.Type == TokenType.Pow))
        {
            Token op = currentToken;
            Advance();
            int line = currentToken.Line;
            int column = currentToken.Column;
            ExpressionNode right = ParseFactor();
            left = new ArithmeticBinaryExpressionNode(left, right, op.Type, line, column);
        }
        return left;
    }

    //Metodo que parsea los factores de las expresiones aritmeticas
    private ExpressionNode ParseFactor()
    {
        if (currentToken == null)
            throw new ParseException("Unexpected end of input", currentLine, currentColumn);

        Debug.Log($"ParseFactor: {currentToken?.Type} '{currentToken?.Value}'");

        //Switch para parsear el tipo de token actual
        switch (currentToken.Type)
        {
            case TokenType.Number:
                return ParseNumber();

            case TokenType.Variable:
                return ParseVariableNode();

            case TokenType.OpenParenthesis:
                return ParseParenthesizedExpression();

            case TokenType.Negation:
                return ParseNegation();

            case TokenType.Substraction:
                return ParseNegativeNumber();

            case TokenType._true:
            case TokenType._false:
                return ParseBoolean();

            case TokenType.String:
                return ParseString();

            case TokenType.GetActualX:
            case TokenType.GetActualY:
            case TokenType.GetCanvasSize:
            case TokenType.GetColorCount:
            case TokenType.IsBrushColor:
            case TokenType.IsBrushSize:
            case TokenType.IsCanvasColor:
                return ParseFunctionCall();

            default:
                throw new ParseException($"Unexpected token in expression: {currentToken.Type}", currentToken.Line, currentToken.Column);
        }
    }
    #endregion

    #region Boolean and Comparison Parsing
    //Metodo que parsea las expresiones booleanas, dando prioridad al OR
    private ExpressionNode ParseBooleanExpression()
    {
        ExpressionNode left = ParseAndExpression();
        while (currentToken != null && (currentToken.Type == TokenType.And || currentToken.Type == TokenType.Or))
        {
            Token op = currentToken;
            Advance();
            ExpressionNode right = ParseAndExpression();
            left = new BooleanBinaryExpressionNode(left, right, op.Type, currentLine, currentColumn);
        }
        return left;
    }

    //Metodo que parsea las expresiones booleanas AND (&&)
    private ExpressionNode ParseAndExpression()
    {
        ExpressionNode left = ParseEquality();

        while (currentToken != null && currentToken.Type == TokenType.And)
        {
            Token op = currentToken;
            Advance();
            ExpressionNode right = ParseEquality();
            left = new BooleanBinaryExpressionNode(left, right, op.Type, left.Line, left.Column);
        }
        return left;
    }

    //Metodo que parsea las igualdades o desigualdades
    private ExpressionNode ParseEquality()
    {
        ExpressionNode left = ParseComparison();

        while (currentToken != null && (currentToken.Type == TokenType.Equals || currentToken.Type == TokenType.Different))
        {
            Token op = currentToken;
            Advance();
            ExpressionNode right = ParseComparison();
            left = new BooleanBinaryExpressionNode(left, right, op.Type, left.Line, left.Column);
        }
        return left;
    }

    //Metodo que parsea las comparaciones (mayor, menor, mayor o igual, menor o igual)
    private ExpressionNode ParseComparison()
    {
        ExpressionNode left = ParseArithmeticExpression();

        while (currentToken != null && (currentToken.Type == TokenType.Major || currentToken.Type == TokenType.MajorEqual || currentToken.Type == TokenType.Minor || currentToken.Type == TokenType.MinorEqual))
        {
            Token op = currentToken;
            Advance();
            ExpressionNode right = ParseArithmeticExpression();
            left = new BooleanBinaryExpressionNode(left, right, op.Type, left.Line, left.Column);
        }
        return left;
    }

    #endregion

    #region Values Parsing
    //Metodo que parsea los valores booleanos
    private ExpressionNode ParseBoolean()
    {
        bool value = currentToken.Type == TokenType._true;
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();
        return new BooleanNode(value, line, column);
    }

    //Metodo que parsea las expresiones entre parentesis
    private ExpressionNode ParseParenthesizedExpression()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;

        Consume(TokenType.OpenParenthesis, "Expected '('");
        ExpressionNode expr = ParseExpression();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after expression");
        return new ParenthesizedExpressionNode(expr, line, column);
    }

    //Metodo que parsea la negacion logica
    private ExpressionNode ParseNegation()
    {
        Token negToken = currentToken;
        Advance();
        ExpressionNode operand = ParseFactor();
        return new LogicalNegationNode(operand, negToken.Line, negToken.Column);
    }

    //Metodo que parsea los numeros negativos
    private ExpressionNode ParseNegativeNumber()
    {
        Advance();
        int value = -int.Parse(currentToken.Value);
        var node = new NumberNode(value, currentToken.Line, currentToken.Column);

        Advance();
        return node;
    }

    //Metodo que parsea los strings
    private ExpressionNode ParseString()
    {
        string value = currentToken.Value.Trim('"');
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();
        return new StringNode(value, line, column);
    }

    //Metodo que parsea los numeros
    private ExpressionNode ParseNumber()
    {
        int value = int.Parse(currentToken.Value);
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();
        return new NumberNode(value, line, column);
    }

    //Metodo que parsea las variables
    private ExpressionNode ParseVariableNode()
    {
        string varName = currentToken.Value;
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();
        return new VariableNode(varName, line, column);
    }

    #endregion

    #region Function Parsing
    //Metodo que parsea las llamadas a funciones
    private ExpressionNode ParseFunctionCall()
    {
        string funcName = currentToken.Value;
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();

        Consume(TokenType.OpenParenthesis, $"Expected '(' after '{funcName}'");
        List<ExpressionNode> args = new List<ExpressionNode>();

        //Hacemos un switch para parsear los argumentos de la funcion segun su nombre
        switch (funcName)
        {
            //Estas no reciben argumentos
            case "GetActualX":
            case "GetActualY":
            case "GetCanvasSize":
                break;

            //A partir de aqui las funciones que reciben argumentos. Dependiendo de qué argumentos reciban, se parsean de una forma u otra
            case "GetColorCount":
                args.Add(ParseString());
                Consume(TokenType.Comma, "Expected ',' after color");
                args.Add(ParseExpression());
                Consume(TokenType.Comma, "Expected ',' after x1");
                args.Add(ParseExpression());
                Consume(TokenType.Comma, "Expected ',' after y1");
                args.Add(ParseExpression());
                Consume(TokenType.Comma, "Expected ',' after x2");
                args.Add(ParseExpression());
                break;

            case "IsBrushColor":
                args.Add(ParseString());
                break;

            case "IsBrushSize":
                args.Add(ParseExpression());
                break;

            case "IsCanvasColor":
                args.Add(ParseString());
                Consume(TokenType.Comma, "Expected ',' after color");
                args.Add(ParseExpression());
                Consume(TokenType.Comma, "Expected ',' after vertical");
                args.Add(ParseExpression());
                break;
        }

        Consume(TokenType.ClosedParenthesis, $"Expected ')' after arguments for '{funcName}'");
        return new FunctionCallNode(funcName, args, line, column);
    }

    #endregion

    #region Assignment and Control Parsing
    //Metodo que parsea las asignaciones de variables o las expresiones
    private Command ParseAssignmentOrExpression()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;
        string variableName = currentToken.Value;
        Advance();

        //Si el siguiente token es <-, se parsea como asignacion
        if (currentPosition < tokens.Count && tokens[currentPosition].Type == TokenType.AssignationArrow)
        {
            Advance();
            ExpressionNode expression = ParseExpression();
            Debug.Log(variableName.ToString());
            return new AssignmentCommand(variableName, expression, line, column);
        }
        Debug.Log(variableName.ToString());
        return new ExpressionStatement(new VariableNode(variableName, line, column), line, column);
    }

    //Metodo que parsea los GoTo
    private Command ParseGoTo()
    {
        Advance();
        Consume(TokenType.OpenBrackets, "Expected '[' after GoTo");
        string label = currentToken.Value;
        Consume(TokenType.Variable, "Expected a label after '['");
        Consume(TokenType.ClosedBrackets, "Expected ']' after label");
        Consume(TokenType.OpenParenthesis, "Expected '(' after label");
        ExpressionNode condition = ParseBooleanExpression();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after condition");
        Debug.Log("Llamando Goto a " + label);
        return new GoToNode(label, condition, currentToken.Line, currentToken.Column);
    }

    //Metodo que parsea las etiquetas
    private LabelNode ParseLabel()
    {
        string labelName = currentToken.Value;
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();
        Debug.Log("label " + labelName);
        return new LabelNode(labelName, line, column);
    }

    #endregion

    #region Helper Methods
    //Metodo que revisa si el token actual es el que se espera en la expresion y si no lanza un error
    private void Consume(TokenType expected, string message)
    {
        if (currentToken == null)
            throw new ParseException($"{message} (reached end of file)", currentLine, currentColumn);

        if (currentToken.Type != expected)
        {
            throw new ParseException($"{message}. Expected {expected}, found {currentToken.Type} ({currentToken.Value}) at line {currentToken.Line}", currentToken.Line, currentToken.Column);
        }

        Advance();
    }

    //Metodo que avanza en la lista de tokens, modifica linea y columna y cambia el token actual
    private void Advance()
    {
        currentPosition++;
        if (currentPosition < tokens.Count)
        {
            currentToken = tokens[currentPosition];
            currentLine = currentToken.Line;
            currentColumn = currentToken.Column;
        }
        else
        {
            currentToken = null;
        }
    }

    //Metodo para saber si lo que se evalua es un comando
    private bool IsCommandToken(TokenType type)
    {
        return type == TokenType.Spawn ||
            type == TokenType.Color ||
            type == TokenType.Size ||
            type == TokenType.DrawLine ||
            type == TokenType.DrawCircle ||
            type == TokenType.DrawRectangle ||
            type == TokenType.Fill;
    }

    //Metodo que cambia de linea
    private void SkipToNextLine()
    {
        if (currentToken == null)
        {
            currentPosition = tokens.Count;
            return;
        }

        int currentLine = currentToken.Line;

        while (currentPosition < tokens.Count && tokens[currentPosition].Line == currentLine)
        {
            currentPosition++;
        }

        if (currentPosition < tokens.Count)
        {
            currentToken = tokens[currentPosition];
            this.currentLine = currentToken.Line;
            this.currentColumn = currentToken.Column;
        }
        else
            currentToken = null;
    }
    #endregion
}