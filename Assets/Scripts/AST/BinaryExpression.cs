public class BinaryExpression : ExpressionNode
{
    ExpressionNode left , right;
    Token Operator;
    public BinaryExpression(ExpressionNode left, ExpressionNode right, Token operador)
    {
        this.left = left;
        this.right = right;
        this.Operator = operador;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}