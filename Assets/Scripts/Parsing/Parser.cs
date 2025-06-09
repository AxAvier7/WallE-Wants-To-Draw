using System;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    private List<Token> tokens;
    private int currentPosition;

    public List<ASTNode> Parse(List<Token> tokens)
    {
        this.tokens = tokens;
        currentPosition = 0;
        List<ASTNode> nodes = new List<ASTNode>();

        while (currentPosition < tokens.Count)
        {
            //pronto lo cambiare para que sea un try-catch que gestione errores de sintaxis
            nodes.Add(ParseCommand());
        }
        return nodes;
    }

    private Command ParseCommand()
    {
        Token currentToken = tokens[currentPosition];

        switch (currentToken.Type)
        {
            case TokenType.Spawn:
                currentPosition++;
                return ParseSpawn();
            case TokenType.Color:
                currentPosition++;
                return ParseColor();
            case TokenType.Size:
                currentPosition++;
                return ParseSize();
            // case TokenType.DrawLine:
            //     currentPosition++;
            //     return ParseDrawLine();
            default:
                throw new Exception($"Unexpected token: {currentToken}");
        }
    }


    private Command ParseSpawn()
    {
        Consume(TokenType.OpenParenthesis, "Expected '(' after 'spawn'");
        var x = ParseNumber();
        Consume(TokenType.Comma, "Expected ',' after x coordinate");
        var y = ParseNumber();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after y coordinate");
        Debug.Log($"Parsed Spawn command with coordinates ({x}, {y})");
        return new Spawn(x, y);
    }
    
    private Command ParseColor()
    {
        Consume(TokenType.OpenParenthesis, "Expected '(' after 'color'");
        string color;
        switch (tokens[currentPosition].Type)
        {
            case TokenType.Red:
                color = "Red";
                break;
            case TokenType.Blue:
                color = "Blue";
                break;
            case TokenType.Green:
                color = "Green";
                break;
            case TokenType.Yellow:
                color = "Yellow";
                break;
            case TokenType.Orange:
                color = "Orange";
                break;
            case TokenType.Purple:
                color = "Purple";
                break;
            case TokenType.Black:
                color = "Black";
                break;
            case TokenType.White:
                color = "White";
                break;
            case TokenType.Transparent:
                color = "Transparent";
                break;
            default:
                throw new Exception($"Expected a color token, found: {tokens[currentPosition]}");
        }
        currentPosition++;
        Consume(TokenType.ClosedParenthesis, "Expected ')' after color string");
        Debug.Log($"Parsed Color command with color '{color}'");
        return new Color(color);
    }

    private Command ParseSize()
    {
        Consume(TokenType.OpenParenthesis, "Expected '(' after 'size'");
        int size = ParseNumber();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after size number");
        Debug.Log($"Parsed Size command with size {size}");
        return new Size(size);
    }

    private string ParseStringLiteral()
    {
        Token currentToken = tokens[currentPosition];
        if (currentToken.Type != TokenType.String)
        {
            throw new Exception($"Expected string literal, found: {currentToken}");
        }
        string value = currentToken.Value;
        currentPosition++;
        return value;
    }

    private int ParseNumber()
    {
        Token currentToken = tokens[currentPosition];
        if (currentToken.Type != TokenType.Number)
        {
            throw new Exception($"Expected number, found: {currentToken}");
        }
        int value = int.Parse(currentToken.Value);
        currentPosition++;
        return value;
    }

    private void Consume(TokenType openParenthesis, string message)
    {
        if (currentPosition >= tokens.Count || tokens[currentPosition].Type != openParenthesis)
        {
            Debug.LogError(message);
        }
        currentPosition++;
    }
}
