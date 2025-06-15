using System;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    private List<Token> tokens;
    private int currentPosition;
    private Token currentToken;
    private int currentLine;
    private int currentColumn;
    public List<ParseException> errors = new List<ParseException>();

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

        while (currentPosition < tokens.Count && currentToken != null)
        {
            try
            {
                if (currentToken == null) break;
                if (currentToken.Type == TokenType.Label)
                {
                    nodes.Add(ParseLabel());
                }
                else if (IsCommandToken(currentToken.Type))
                {
                    nodes.Add(ParseCommand());
                }
                else if (currentToken.Type == TokenType.Variable)
                {
                    nodes.Add(ParseAssignmentOrExpression());
                }
                else if (currentToken.Type == TokenType.GoTo)
                {
                    nodes.Add(ParseGoTo());
                }

                else
                {
                    nodes.Add(ParseExpressionStatement());
                }
            }
            catch (ParseException ex)
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

    private Command ParseCommand()
    {
        return currentToken.Type switch
        {
            TokenType.Spawn => ParseSpawn(),
            TokenType.Color => ParseColor(),
            TokenType.Size => ParseSize(),
            TokenType.DrawLine => ParseDrawLine(),
            TokenType.DrawCircle => ParseDrawCircle(),
            TokenType.DrawRectangle => ParseDrawRectangle(),
            TokenType.Fill => ParseFill(),
            _ => throw new ParseException($"Unexpected command: {currentToken.Type}",
                   currentToken.Line, currentToken.Column)
        };
    }

    #region CommandParsing
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

    private Command ParseColor()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;

        Advance();
        Consume(TokenType.OpenParenthesis, "Expected '(' after 'Color'");
        string color;
        switch (tokens[currentPosition].Type)
        {
            case TokenType.Red:
                color = "Red";
                Advance();
                break;
            case TokenType.Blue:
                color = "Blue";
                Advance();
                break;
            case TokenType.Green:
                color = "Green";
                Advance();
                break;
            case TokenType.Yellow:
                color = "Yellow";
                Advance();
                break;
            case TokenType.Orange:
                color = "Orange";
                Advance();
                break;
            case TokenType.Purple:
                color = "Purple";
                Advance();
                break;
            case TokenType.Black:
                color = "Black";
                Advance();
                break;
            case TokenType.White:
                color = "White";
                Advance();
                break;
            case TokenType.Gray:
                color = "Gray";
                Advance();
                break;
            case TokenType.Pink:
                color = "Pink";
                Advance();
                break;
            case TokenType.LightBlue:
                color = "LightBlue";
                Advance();
                break;
            case TokenType.LightGreen:
                color = "LightGreen";
                Advance();
                break;
            case TokenType.Brown:
                color = "Brown";
                Advance();
                break;
            case TokenType.LightGray:
                color = "LightGray";
                Advance();
                break;
            case TokenType.Transparent:
                color = "Transparent";
                Advance();
                break;
            default:
                throw new Exception($"Expected a color token, found: {tokens[currentPosition]}");
        }

        Consume(TokenType.ClosedParenthesis, "Expected ')' after color string");
        return new Color(color, line, column);
    }

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
    private Command ParseExpressionStatement()
    {
        ExpressionNode expr = ParseExpression();
        return new ExpressionStatement(expr, currentToken.Line, currentToken.Column);
    }

    private ExpressionNode ParseExpression()
    {
        return ParseBooleanExpression();
    }

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

    private ExpressionNode ParseFactor()
    {
        if (currentToken == null)
            throw new ParseException("Unexpected end of input", currentLine, currentColumn);

        Debug.Log($"ParseFactor: {currentToken?.Type} '{currentToken?.Value}'");

        switch (currentToken.Type)
        {
            case TokenType.Number:
                return ParseNumber();

            case TokenType.Variable:
                return ParseVariableNode();

            case TokenType.OpenParenthesis:
                return ParseParenthesizedExpression();

            case TokenType.Substraction:
                return ParseNegation();

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

    private ExpressionNode ParseEquality()
    {
        ExpressionNode left = ParseRelational();

        while (currentToken != null && (currentToken.Type == TokenType.Equals || currentToken.Type == TokenType.Different))
        {
            Token op = currentToken;
            Advance();
            ExpressionNode right = ParseRelational();
            left = new BooleanBinaryExpressionNode(left, right, op.Type, left.Line, left.Column);
        }
        return left;
    }

    private ExpressionNode ParseRelational()
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
    private ExpressionNode ParseBoolean()
    {
        bool value = currentToken.Type == TokenType._true;
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();
        return new BooleanNode(value, line, column);
    }

    private ExpressionNode ParseParenthesizedExpression()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;

        Consume(TokenType.OpenParenthesis, "Expected '('");
        ExpressionNode expr = ParseExpression();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after expression");
        return new ParenthesizedExpressionNode(expr, line, column);
    }
    private ExpressionNode ParseNegation()
    {
        Token negToken = currentToken;
        Advance();
        ExpressionNode operand = ParseFactor();
        return new NegationExpressionNode(operand, negToken.Line, negToken.Column);
    }

    private ExpressionNode ParseLogicalNegation()
    {
        Token notToken = currentToken;
        Advance();
        ExpressionNode operand = ParseFactor();
        return new LogicalNegationNode(operand, notToken.Line, notToken.Column);
    }

    private ExpressionNode ParseString()
    {
        string value = currentToken.Value.Trim('"');
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();
        return new StringNode(value, line, column);
    }

    private ExpressionNode ParseNumber()
    {
        int value = int.Parse(currentToken.Value);
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();
        return new NumberNode(value, line, column);
    }

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
    private ExpressionNode ParseFunctionCall()
    {
        string funcName = currentToken.Value;
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();

        Consume(TokenType.OpenParenthesis, $"Expected '(' after '{funcName}'");
        List<ExpressionNode> args = new List<ExpressionNode>();

        switch (funcName)
        {
            case "GetActualX":
            case "GetActualY":
            case "GetCanvasSize":
                break;

            case "GetColorCount":
                args.Add(ParseExpression());
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
                args.Add(ParseExpression());
                break;

            case "IsBrushSize":
                args.Add(ParseExpression());
                break;

            case "IsCanvasColor":
                args.Add(ParseExpression());
                Consume(TokenType.Comma, "Expected ',' after color");
                args.Add(ParseExpression());
                Consume(TokenType.Comma, "Expected ',' after vertical");
                args.Add(ParseExpression());
                break;
        }

        Consume(TokenType.ClosedParenthesis, $"Expected ')' after arguments for '{funcName}'");
        return new FunctionCallNode(funcName, args, line, column);
    }

    private bool IsFunctionToken(TokenType type)
    {
        return type == TokenType.GetActualX ||
               type == TokenType.GetActualY ||
               type == TokenType.GetCanvasSize ||
               type == TokenType.GetColorCount ||
               type == TokenType.IsBrushColor ||
               type == TokenType.IsBrushSize ||
               type == TokenType.IsCanvasColor;
    }

    #endregion

    #region Assignment and Control Parsing
    private Command ParseAssignmentOrExpression()
    {
        int line = currentToken.Line;
        int column = currentToken.Column;
        string variableName = tokens[currentPosition].Value;
        Advance();

        if (currentPosition < tokens.Count && tokens[currentPosition].Type == TokenType.AssignationArrow)
        {
            Advance();
            ExpressionNode expression = ParseExpression();
            return new AssignmentCommand(variableName, expression, line, column);
        }
        return new ExpressionStatement(new VariableNode(variableName, line, column), line, column);
    }

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
        return new GoToNode(label, condition, currentToken.Line, currentToken.Column);
    }

    private LabelNode ParseLabel()
    {
        string labelName = currentToken.Value;
        int line = currentToken.Line;
        int column = currentToken.Column;
        Advance();
        return new LabelNode(labelName, line, column);
    }

    #endregion

    #region Helper Methods
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