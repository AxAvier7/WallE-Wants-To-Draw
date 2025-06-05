using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public abstract class BinaryExpression : ExpressionNode
{
    public ExpressionNode Left { get; }
    public ExpressionNode Right { get; }
    public TokenType Operator { get; }
    public BinaryExpression(ExpressionNode left, ExpressionNode right, TokenType operador, int line, int column)
    {
        Left = left;
        Right = right;
        Operator = operador;
        Line = line;
        Column = column;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

public class ArithmeticBinaryExpressionNode : BinaryExpression
{
    public ArithmeticBinaryExpressionNode(ExpressionNode left, ExpressionNode right, TokenType operador, int line, int column)
        : base(left, right, operador, line, column)
    {
        ValidateOperator(operador);
    }

    public override int Evaluate(Wall_E wall_E, GridManager gridManager, VariableManager variables)
    {
        int leftValue = Left.Evaluate(wall_E, gridManager, variables);
        int rightValue = Right.Evaluate(wall_E, gridManager, variables);

        switch (Operator)
        {
            case TokenType.Addition:
                return leftValue + rightValue;
            case TokenType.Substraction:
                return leftValue - rightValue;
            case TokenType.Multiplication:
                return leftValue * rightValue;
            case TokenType.Division:
                return leftValue / rightValue;
            case TokenType.Pow:
                return (int)Mathf.Pow(leftValue, rightValue);
            case TokenType.Module:
                if (rightValue == 0)
                {
                    Debug.LogError($"Division by zero at line {Line}, column {Column}");
                    break;
                }
                return leftValue % rightValue;
            default:
                Debug.LogError($"Unsupported operator: {Operator} at line {Line}, column {Column}");
                break;
        }
        return 0;
    }

    public void ValidateOperator(TokenType operador)
    {
        if (!IsArithmeticOperator(operador))
            throw new ArgumentException($"Invalid arithemtic operator: {operador}");
    }

    public bool IsArithmeticOperator(TokenType operador)
    {
        return operador == TokenType.Addition || operador == TokenType.Substraction
            || operador == TokenType.Multiplication || operador == TokenType.Division
            || operador == TokenType.Module || operador == TokenType.Pow;
    }
}

public class BooleanBinaryExpressionNode : BinaryExpression
{
    public BooleanBinaryExpressionNode(ExpressionNode left, ExpressionNode right, TokenType operador, int line, int column)
        : base(left, right, operador, line, column)
    {
        ValidateOperator(operador);
    }

    private void ValidateOperator(TokenType operador)
    {
        if (!IsBooleanOperator(operador))
            throw new ArgumentException($"Invalid boolean operator: {operador}");
    }

    private bool IsBooleanOperator(TokenType operador)
    {
        return operador == TokenType.And || operador == TokenType.Or ||
               operador == TokenType.Equals || operador == TokenType.Different ||
               operador == TokenType.Major || operador == TokenType.MajorEqual ||
               operador == TokenType.Minor || operador == TokenType.MinorEqual;
    }

    public override int Evaluate(Wall_E wall_E, GridManager gridManager, VariableManager variables)
    {
        int leftVal = Left.Evaluate(wall_E, gridManager, variables);
        int rightVal = Right.Evaluate(wall_E, gridManager, variables);

        switch (Operator)
        {
            case TokenType.Equals: return leftVal == rightVal ? 1 : 0;
            case TokenType.Different: return leftVal != rightVal ? 1 : 0;
            case TokenType.Major: return leftVal > rightVal ? 1 : 0;
            case TokenType.MajorEqual: return leftVal >= rightVal ? 1 : 0;
            case TokenType.Minor: return leftVal < rightVal ? 1 : 0;
            case TokenType.MinorEqual: return leftVal <= rightVal ? 1 : 0;
            case TokenType.And: return (leftVal != 0 && rightVal != 0) ? 1 : 0;
            case TokenType.Or: return (leftVal != 0 || rightVal != 0) ? 1 : 0;
            default: throw new Exception($"Unsupported boolean operator: {Operator}");
        }
    }
}