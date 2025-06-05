using System;
using System.Collections.Generic;
public abstract class ExpressionNode : ASTNode
{
    public abstract int Evaluate(Wall_E wall_E, GridManager gridManager, VariableManager variables);

    internal int Evaluate()
    {
        throw new NotImplementedException();
    }
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

    public override string ToString()
    {
        return Value.ToString();
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
