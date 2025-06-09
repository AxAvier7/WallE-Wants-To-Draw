public abstract class ASTNode
{
    public int Line { get; protected set; }
    public int Column { get; protected set; }
    public abstract void Accept(IVisitor visitor);
    public abstract void Execute(Context context);
}
