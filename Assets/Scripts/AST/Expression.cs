using UnityEngine;

public abstract class Expression
{
    public virtual ExValue Evaluate(Context context)
    {
        return default;
    }
}

public class GetActualX : Expression // Devuelve la coordenada x actual de WallE
{
    public override ExValue Evaluate(Context context)
    {
        return context.WallE.GetActualX();
    }
}

public class GetActualY : Expression // Devuelve la coordenada x actual de WallE
{
    public override ExValue Evaluate(Context context)
    {
        return context.WallE.GetActualY();
    }
}

public class GetCanvasSize : Expression // Devuelve el ancho/largo delcanvas
{
    public override ExValue Evaluate(Context context)
    {
        return context.GridManager.Width;
    }
}

public class GetColorCount : Expression // Devuelve la cantidad de casillas que hay del color 'color' en el rectangulo formado por las dos esquinas (x1,y1) y (x2,y2)
{
    string color;
    int x1, y1, x2, y2;

    public GetColorCount(string color, int x1, int y1, int x2, int y2)
    {
        this.color = color;
        this.x1 = x1;
        this.y1 = y1;
        this.x2 = x2;
        this.y2 = y2;
    }
    public override ExValue Evaluate(Context context)
    {
        int minX = Mathf.Min(x1, x2);
        int maxX = Mathf.Min(x1, x2);
        int minY = Mathf.Min(y1, y2);
        int maxY = Mathf.Min(y1, y2);

        minX = Mathf.Clamp(minX, 0, context.GridManager.Width - 1);
        maxX = Mathf.Clamp(maxX, 0, context.GridManager.Width - 1);
        minY = Mathf.Clamp(minY, 0, context.GridManager.Height - 1);
        maxY = Mathf.Clamp(maxY, 0, context.GridManager.Height - 1);

        int count = 0;

        for (int x = minX; x < maxX; x++)
        {
            for (int y = minY; y < maxY; y++)
            {
                if (context.GridManager.GetPixelColorName(x, y) == color)
                    count++;
            }
        }
        return count;
    }
}

public class IsBrushColor : Expression // Devuelve 1 si el color actual de la brocha es 'color'. Si no devuelve 0
{
    string color;
    public IsBrushColor(string color)
    {
        this.color = color;
    }
    public override ExValue Evaluate(Context context)
    {
        return context.WallE.currentColor == color? 1 : 0;
    }
}

public class IsBrushSize : Expression  // Devuelve 1 si el tamaño actual de la brocha es 'size'. Si no devuelve 0
{
    int size;
    public IsBrushSize(int size)
    {
        this.size = size;
    }
    public override ExValue Evaluate(Context context)
    {
        return context.WallE.currentBrushSize == size ? 1 : 0;
    }
}

public class IsCanvasColor : Expression // Devuelve 1 si la casilla en (GetActualX + horizontal, GetActualY + vertical) es de color 'color'
{
    string color;
    int vertical, horizontal;
    public IsCanvasColor(string color, int vertical, int horizontal)
    {
        this.color = color;
        this.vertical = vertical;
        this.horizontal = horizontal;
    }
    public override ExValue Evaluate(Context context)
    {
        int checkX = context.WallE.X + horizontal;
        int checkY = context.WallE.Y + vertical;
        if (checkX < 0 || checkY < 0 || checkX >= context.GridManager.Width || checkY >= context.GridManager.Height)
            return 0;
        return context.GridManager.GetPixelColorName(checkX, checkY) == color ? 1 : 0;
    }
}