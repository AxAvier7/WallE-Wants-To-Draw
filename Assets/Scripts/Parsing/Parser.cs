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
            case TokenType.DrawLine:
                currentPosition++;
                return ParseDrawLine();
            case TokenType.DrawCircle:
                currentPosition++;
                return ParseDrawCircle();
            case TokenType.DrawRectangle:
                currentPosition++;
                return ParseDrawRectangle();
            case TokenType.Fill:
                currentPosition++;
                return ParseFill();

            default:
                throw new Exception($"Unexpected token: {currentToken}");
        }
    }

    private Command ParseSpawn()
    {
        Consume(TokenType.OpenParenthesis, "Expected '(' after 'Spawn'");
        var x = ParseNumber();
        Consume(TokenType.Comma, "Expected ',' after x coordinate");
        var y = ParseNumber();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after y coordinate");
        return new Spawn(x, y);
    }
    
    private Command ParseColor()
    {
        Consume(TokenType.OpenParenthesis, "Expected '(' after 'Color'");
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
        return new Color(color);
    }

    private Command ParseSize()
    {
        Consume(TokenType.OpenParenthesis, "Expected '(' after 'Size'");
        int size = ParseNumber();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after size number");
        return new Size(size);
    }

    private Command ParseDrawLine()
    {
        Consume(TokenType.OpenParenthesis, "Expected '(' after 'Spawn'");
        var dirX = ParseNumber();
        Consume(TokenType.Comma, "Expected ',' after x direction");
        var dirY = ParseNumber();
        Consume(TokenType.Comma, "Expected ',' after y direction");
        var distance = ParseNumber();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after distance");
        return new DrawLine(dirX, dirY, distance);
    }

    private Command ParseDrawCircle()
    {
        Consume(TokenType.OpenParenthesis, "Expected '(' after 'DrawCircle'");
        int dirX = ParseNumber();
        Consume(TokenType.Comma, "Expected ',' after dirX");
        var dirY = ParseNumber();
        Consume(TokenType.Comma, "Expected ',' after dirY");
        var radius = ParseNumber();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after radius");
        return new DrawCircle(dirX, dirY, radius);
    }

    private Command ParseDrawRectangle()
    {
        Consume(TokenType.OpenParenthesis, "Expected '(' after 'DrawRectangle'");
        int dirX = ParseNumber();
        Consume(TokenType.Comma, "Expected ',' after x coordinate");
        int dirY = ParseNumber();
        Consume(TokenType.Comma, "Expected ',' after y coordinate");
        int distance = ParseNumber();
        Consume(TokenType.Comma, "Expected ',' after distance");
        int width = ParseNumber();
        Consume(TokenType.Comma, "Expected ',' after width");
        int height = ParseNumber();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after height");
        Debug.Log($"Parsed DrawRectangle command with size ({width}, {height})");
        return new DrawRectangle(dirX, dirY, distance, width, height);
    }

    private Command ParseFill()
    {
        Consume(TokenType.OpenParenthesis, "Expected '(' after Fill");
        Consume(TokenType.ClosedParenthesis, "Expected ')' after '('");
        return new Fill();
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
