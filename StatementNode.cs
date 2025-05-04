public abstract class StatementNode : ASTNode
{

}

public class AssignmentNode : StatementNode
{
    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

public class FunctionCallStatement : StatementNode
{
    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}