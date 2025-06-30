//Clase que representa un nodo del AST
public abstract class ASTNode
{
    public int Line { get; protected set; }
    public int Column { get; protected set; }
    public abstract void Execute(Context context);
}
