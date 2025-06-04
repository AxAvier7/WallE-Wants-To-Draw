using System.Collections.Generic;

public abstract class StatementNode : ASTNode   {}

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
}

public class CommandNode : StatementNode
{
    public string CommandName { get; }
    public List<ExpressionNode> Arguments { get; }

    public CommandNode(string commandName, List<ExpressionNode> arguments, int line = 0, int column = 0)
    {
        CommandName = commandName;
        Arguments = arguments;
        Line = line;
        Column = column;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

public class GoToNode : StatementNode
{
    public string Label { get; }
    public ExpressionNode Condition { get; }
    public GoToNode(string label, ExpressionNode condition, int line = 0, int column = 0)
    {
        Label = label;
        Condition = condition;
        Line = line;
        Column = column;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

public class LabelNode : StatementNode
{
    public string LabelName { get; }
    public LabelNode(string name, int line = 0, int column = 0)
    {
        LabelName = name;
        Line = line;
        Column = column;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

public class FunctionCallStatement : StatementNode
{
    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}