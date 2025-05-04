public class BinaryExpression : ExpressionNode
{
    ExpressionNode left , right;
    Token operador;
    public BinaryExpression(ExpressionNode left, ExpressionNode right, Token operador)
    {
        this.left = left;
        this.right = right;
        this.operador = operador;
    }

    public override void Accept(IVisitor visitor) => visitor.Visit(this);
}

public class ArithmeticNode : BinaryExpression
{
    public ArithmeticNode(ExpressionNode left, ExpressionNode right, Token operador) : base(left, right, operador)
    {}
}

public class ComparisonNode : BinaryExpression
{
    public ComparisonNode(ExpressionNode left, ExpressionNode right, Token operador) : base(left, right, operador)
    {}
}