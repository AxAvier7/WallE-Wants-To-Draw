using UnityEngine;

public abstract class Expression
{
    public virtual ExValue Evaluate(Context context)
    {
        return default;
    }
}

public class NumberLiteral : Expression
{
    private int value;

    public NumberLiteral(int value)
    {
        this.value = value;
    }

    public override ExValue Evaluate(Context context)
    {
        return value;
    }
}

public class StringLiteral : Expression
{
    private string value;

    public StringLiteral(string value)
    {
        this.value = value;
    }

    //Este Evaluate devuelve 0 porque no es necesario que los strings se ejecuten
    public override ExValue Evaluate(Context context)
    {
        return 0;
    }
}

public class GetActualX : Expression
{
    public override ExValue Evaluate(Context context)
    {
        return context.WallE.GetActualX();
    }
}

public class GetActualY : Expression
{
    public override ExValue Evaluate(Context context)
    {
        return context.WallE.GetActualY();
    }
}

public class GetCanvasSize : Expression
{
    public override ExValue Evaluate(Context context)
    {
        return context.GridManager.Width;
    }
}

public class GetColorCount : Expression
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

public class IsBrushColor : Expression
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

public class IsBrushSize : Expression
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

public class IsCanvasColor : Expression
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