using System.Collections.Generic;

public abstract class ExpressionNode : ASTNode
{
    
}

public class NumberNode : ExpressionNode
{
    public int Value;
    public NumberNode(int value)
    {
        Value=value;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
    public override string ToString()
    {
        return Value.ToString();
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
}

public class StringNode : ExpressionNode
{
    public string Value;
    public StringNode(string value)
    {
        Value=value;
    }
    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

public class VariableNode : ExpressionNode
{
    public string Name, Value;
    public VariableNode(string name, string value)
    {
        Name=name;
        Value=value;
    }
    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

