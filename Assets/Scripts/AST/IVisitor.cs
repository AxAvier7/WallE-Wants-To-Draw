public interface IVisitor
{
    void Visit(BinaryExpression node);
    void Visit(FunctionCallNode node);
    void Visit(NumberNode node);
    void Visit(FunctionCallStatement node);
    void Visit(AssignmentNode node);
    void Visit(StringNode node);
    void Visit(VariableNode node);
}
