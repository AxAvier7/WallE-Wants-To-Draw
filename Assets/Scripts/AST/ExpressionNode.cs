using System.Collections.Generic;

public abstract class ExpressionNode : ASTNode
{
    public abstract ExValue Evaluate(Context context);
    public override void Execute(Context context) => Evaluate(context);
}

public class NumberNode : ExpressionNode
{
    public int Value;
    public NumberNode(int value, int line, int column)
    {
        Value = value;
        Line = line;
        Column = column;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);

    public override ExValue Evaluate(Context context)
    {
        return Value;
    }
}

public class StringNode : ExpressionNode
{
    public string Value;
    public StringNode(string value, int line, int column)
    {
        Value = value;
        Line = line;
        Column = column;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);

    public override ExValue Evaluate(Context context)
    {
        return Value;
    }
}

public class BooleanNode : ExpressionNode
{
    public bool Value;
    public BooleanNode(bool value, int line, int column)
    {
        Value = value;
        Line = line;
        Column = column;
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
    public FunctionCallNode(string name, List<ExpressionNode> args, int line, int column)
    {
        FunctionName = name;
        Arguments = args;
        Line = line;
        Column = column;
    }
    public override void Accept(IVisitor visitor) => visitor.Visit(this);

    public override ExValue Evaluate(Context context)
    {
        List<ExValue> evaluatedArgs = new List<ExValue>();
        foreach (var arg in Arguments)
        {
            evaluatedArgs.Add(arg.Evaluate(context));
        }

        switch (FunctionName)
        {
            case "GetActualX":
                ValidateArgumentCount(0);
                return context.WallE.GetActualX();

            case "GetActualY":
                ValidateArgumentCount(0);
                return context.WallE.GetActualY();

            case "GetCanvasSize":
                ValidateArgumentCount(0);
                return context.GridManager.Width;

            case "GetColorCount":
                ValidateArgumentCount(5);
                return new GetColorCount(evaluatedArgs[0].AsString(), evaluatedArgs[1].AsInt(), evaluatedArgs[2].AsInt(), evaluatedArgs[3].AsInt(), evaluatedArgs[4].AsInt()).Evaluate(context);

            case "IsBrushColor":
                ValidateArgumentCount(1);
                return new IsBrushColor(evaluatedArgs[0].AsString()).Evaluate(context);

            case "IsBrushSize":
                ValidateArgumentCount(1);
                return new IsBrushSize(evaluatedArgs[0].AsInt()).Evaluate(context);

            case "IsCanvasColor":
                ValidateArgumentCount(3);
                return new IsCanvasColor(evaluatedArgs[0].AsString(), evaluatedArgs[1].AsInt(), evaluatedArgs[2].AsInt()).Evaluate(context);

            default:
                throw new System.Exception($"Unknown function: {FunctionName}");
        }
    }

    public void ValidateArgumentCount(int expectedCount)
    {
        if (Arguments.Count != expectedCount)
            throw new System.Exception($"Expected {expectedCount} argument but recieved {Arguments.Count}");
    }
}

public class VariableNode : ExpressionNode
{
    public string Name { get; }
    public VariableNode(string name, int line, int column)
    {
        Name = name;
        Line = line;
        Column = column;
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
    public NegationExpressionNode(ExpressionNode operand, int line, int column)
    {
        Operand = operand;
        Line = line;
        Column = column;
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

public class ParenthesizedExpressionNode : ExpressionNode
{
    public ExpressionNode Expression { get; }
    public ParenthesizedExpressionNode(ExpressionNode expression, int line, int column)
    {
        Expression = expression;
        Line = line;
        Column = column;
    }

    public override ExValue Evaluate(Context context)
    {
        return Expression.Evaluate(context);
    }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}

public class LogicalNegationNode : ExpressionNode
{
    public ExpressionNode Operand { get; }
    public LogicalNegationNode(ExpressionNode operand, int line, int column)
    {
        Operand = operand;
        Line = line;
        Column = column;
    }

    public override ExValue Evaluate(Context context)
    {
        bool operandValue = Operand.Evaluate(context).AsBool();
        return !operandValue;
    }

    public override void Accept(IVisitor visitor)
    {
        visitor.Visit(this);
    }
}