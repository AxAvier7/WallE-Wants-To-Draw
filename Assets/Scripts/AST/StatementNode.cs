using System.Collections.Generic;

public abstract class StatementNode : ASTNode{ }

public class AssignmentNode : StatementNode
{
    public string VariableName { get; }
    public ExpressionNode Expression { get; }

    public AssignmentNode(string variableName, ExpressionNode expression, int line, int column)
    {
        VariableName = variableName;
        Expression = expression;
        Line = line;
        Column = column;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);

    public override void Execute(Context context)
    {
        throw new System.NotImplementedException();
    }
}

public class CommandNode : StatementNode
{
    public string CommandName { get; }
    public List<ExpressionNode> Arguments { get; }

    public CommandNode(string commandName, List<ExpressionNode> arguments, int line, int column)
    {
        CommandName = commandName;
        Arguments = arguments;
        Line = line;
        Column = column;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);

    public override void Execute(Context context)
    {
        throw new System.NotImplementedException();
    }
}

public class LabelNode : StatementNode
{
    public string LabelName { get; }
    public LabelNode(string name, int line, int column)
    {
        LabelName = name;
        Line = line;
        Column = column;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);

    public override void Execute(Context context)
    {
        throw new System.NotImplementedException();
    }
}