using System.Collections.Generic;

public abstract class ExpressionNode : ASTNode
{
    public abstract ExValue Evaluate(Context context);
    public override void Execute(Context context) => Evaluate(context);
}

public class NumberNode : ExpressionNode
{
    public int Value;
    public NumberNode(int value)
    {
        Value = value;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);

    public override ExValue Evaluate(Context context)
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

    public override ExValue Evaluate(Context context)
    {
        List<int> evaluatedArgs = new List<int>();
        foreach (var arg in Arguments)
        {
            evaluatedArgs.Add(arg.Evaluate(context).AsInt());
        }

        switch (FunctionName)
        {
            case "GetActualX":
                return context.WallE.GetActualX();
            case "GetActualY":
                return context.WallE.GetActualY();
            case "GetCanvasSize":
                return context.GridManager.Width;
            // case "GetColorCount":
            //     return new GetColorCount(evaluatedArgs[0], evaluatedArgs[1], evaluatedArgs[2], evaluatedArgs[3], evaluatedArgs[4]).Execute(wall_E, gridManager);
            // case "IsBrushColor":
            //     return new IsBrushColor(evaluatedArgs[0]).Execute(wall_E, gridManager);
            case "IsBrushSize":
                return new IsBrushSize(evaluatedArgs[0]).Evaluate(context);
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

    public override ExValue Evaluate(Context context)
    {
        return context.Variables.GetVariable(Name);
    }
}

public class NegationExpressionNode : ExpressionNode
{
    public ExpressionNode Operand { get; }
    public NegationExpressionNode(ExpressionNode operand)
    {
        Operand = operand;
    }
    public override ExValue Evaluate(Context context)
    {
        int operandValue = Operand.Evaluate(context).AsInt();
        return operandValue == 0 ? 1 : 0;
    }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}
