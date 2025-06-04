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

