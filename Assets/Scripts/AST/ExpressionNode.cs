using System;
using System.Collections.Generic;

public abstract class ExpressionNode : ASTNode
{
    public abstract int Evaluate(Wall_E wall_E, GridManager gridManager, VariableManager variables);
}

public class NumberNode : ExpressionNode
{
    public int Value;
    public NumberNode(int value)
    {
        Value = value;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);

    public override int Evaluate(Wall_E wall_E, GridManager gridManager, VariableManager variables)
    {
        return Value;
    }
}

public class FunctionCallNode : ExpressionNode
{
    public string FunctionName { get; }
    public List<ExpressionNode> Arguments { get; }
    public FunctionCallNode(string name, List<ExpressionNode> args)
    {
        FunctionName = name;
        Arguments = args;
    }
    public override void Accept(IVisitor visitor) => visitor.Visit(this);

    public override int Evaluate(Wall_E wall_E, GridManager gridManager, VariableManager variables)
    {
        List<int> evaluatedArgs = new List<int>();
        foreach (var arg in Arguments)
        {
            evaluatedArgs.Add(arg.Evaluate(wall_E, gridManager, variables));
        }

        switch (FunctionName)
        {
            case "GetActualX":
                return wall_E.GetActualX();
            case "GetActualY":
                return wall_E.GetActualY();
            case "GetCanvasSize":
                return gridManager.Width;
            // case "GetColorCount":
            //     return new GetColorCount(evaluatedArgs[0], evaluatedArgs[1], evaluatedArgs[2], evaluatedArgs[3], evaluatedArgs[4]).Execute(wall_E, gridManager);
            // case "IsBrushColor":
            //     return new IsBrushColor(evaluatedArgs[0]).Execute(wall_E, gridManager);
            case "IsBrushSize":
                return new IsBrushSize(evaluatedArgs[0]).Execute(wall_E, gridManager, variables);
            // case "IsCanvasColor":
            //     return new IsCanvasColor(evaluatedArgs[0], evaluatedArgs[1], evaluatedArgs[2], evaluatedArgs[3], evaluatedArgs[4]).Execute(wall_E, gridManager);
            default:
                throw new System.Exception($"Unknown function: {FunctionName}");
        }
    }
}

public class VariableNode : ExpressionNode
{
    public string Name { get; }
    public VariableNode(string name)
    {
        Name=name;
    }
    public override void Accept(IVisitor visitor) => visitor.Visit(this);

    public override int Evaluate(Wall_E wall_E, GridManager gridManager, VariableManager variables)
    {
        return variables.GetVariable(Name);
    }
}

public class NegationExpressionNode : ExpressionNode
{
    public ExpressionNode Operand { get; }
    public NegationExpressionNode(ExpressionNode operand)
    {
        Operand = operand;
    }
    public override int Evaluate(Wall_E wall_E, GridManager gridManager, VariableManager variables)
    {
        int operandValue = Operand.Evaluate(wall_E, gridManager, variables);
        return operandValue == 0 ? 1 : 0;
    }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
