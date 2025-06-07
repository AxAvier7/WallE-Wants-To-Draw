using System;
using System.Collections.Generic;
using UnityEngine;

public class Parser : MonoBehaviour
{
    private List<Token> tokens;
    private int currentPosition;

    public List<Command> Parse(List<Token> tokens)
    {
        this.tokens = tokens;
        currentPosition = 0;
        List<Command> commands = new List<Command>();

        while (currentPosition < tokens.Count)
        {
            //pronto lo cambiare para que sea un try-catch que gestione errores de sintaxis
            commands.Add(ParseCommand());
        }
        return commands;
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
            // case TokenType.Size:
            //     currentPosition++;
            //     return ParseSize();
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
        var color = ParseStringLiteral();
        Consume(TokenType.ClosedParenthesis, "Expected ')' after color string");
        Debug.Log($"Parsed Color command with color '{color}'");
        return new Color(color);
    }

    private string ParseStringLiteral()
    {
        throw new NotImplementedException();
    }

    private int ParseNumber()
    {
        throw new NotImplementedException();
    }

    private void Consume(TokenType openParenthesis, string message)
    {
        throw new NotImplementedException();
    }
}
