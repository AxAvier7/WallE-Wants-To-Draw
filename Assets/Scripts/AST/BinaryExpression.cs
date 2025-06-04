using UnityEngine;
using Debug = UnityEngine.Debug;

public class BinaryExpression : ExpressionNode
{
    ExpressionNode Left { get; }
    ExpressionNode Right { get; }
    TokenType Operator { get; }
    public BinaryExpression(ExpressionNode left, ExpressionNode right, TokenType operador, int line, int column)
    {
        Left = left;
        Right = right;
        Operator = operador;
        Line = line;
        Column = column;
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

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}