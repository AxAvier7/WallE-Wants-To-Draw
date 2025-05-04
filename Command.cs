public abstract class Command
{
    public virtual void Execute(){}
}

public class Spawn : Command
{
    int x, y;
    public Spawn(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public override void Execute()
    {
        throw new NotImplementedException();
    }
}

public class Color : Command
{
    string color;
    private List<string> aivableColors = new List<string> {"Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Black", "White", "Transparent"};
    public Color(string color)
    {
        this.color = color;
    }
    public override void Execute()
    {
        if(!aivableColors.Contains(color))
            throw new Exception("color no admitido");
        throw new NotImplementedException();
    }
}

public class Size : Command
{
    int k;
    public Size(int k)
    {
        this.k = k;
    }
    public override void Execute()
    {
        if(k%2 == 0) k = k-1;
        throw new NotImplementedException();
    }
}

public class DrawLine : Command
{
    int dirX, dirY, distance;

    public DrawLine(int dirX, int dirY, int distance)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.distance = distance;
    }
    public override void Execute()
    {
        throw new NotImplementedException();
    }
}
public class DrawCircle : Command
{
    int dirX, dirY, radius;
    public DrawCircle(int dirX, int dirY, int radius)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.radius = radius;
    }
    public override void Execute()
    {
        throw new NotImplementedException();
    }
}

public class DrawRectangle : Command
{
    int dirX, dirY, distance, width, height;
    public DrawRectangle(int dirX, int dirY, int distance, int width, int height)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.distance = distance;
        this.width = width;
        this.height = height;
    }

    public override void Execute()
    {
        throw new NotImplementedException();
    }
}

public class Fill : Command
{
    public override void Execute()
    {
        throw new NotImplementedException();
    }
}