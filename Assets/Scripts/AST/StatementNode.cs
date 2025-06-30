public abstract class StatementNode : ASTNode{ }

//Nodo que representa las etiquetas que luego usarán los GoTo
public class LabelNode : StatementNode
{
    public string LabelName { get; }
    public LabelNode(string name, int line, int column)
    {
        LabelName = name;
        Line = line;
        Column = column;
    }

    //los labels no necesitan ejecutarse
    public override void Execute(Context context)
    {
        throw new System.NotImplementedException();
    }
}