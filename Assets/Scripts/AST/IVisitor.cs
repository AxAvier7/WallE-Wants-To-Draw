public interface IVisitor
{
    void Visit(BinaryExpression node);
    void Visit(FunctionCallNode node);
    void Visit(NumberNode node);
    void Visit(FunctionCallStatement node);
    void Visit(AssignmentNode node);
    void Visit(VariableNode node);
    void Visit(CommandNode node);
    void Visit(GoToNode node);
    void Visit(LabelNode node);
    void Visit(NegationExpressionNode node);
}
