using System;
using UnityEngine;

public abstract class Command : StatementNode // Clase base para todos los comandos que Wall-E puede ejecutar
{
    public override void Accept(IVisitor visitor) => visitor.Visit(this);
    
}

public class AssignmentCommand : Command
{
    private string variableName;
    private Expression expression;
    public AssignmentCommand(string variableName, Expression expression)
    {
        this.variableName = variableName;
        this.expression = expression;
    }



    public override void Execute(Context context)
    {
        int value = expression.Evaluate(context).AsInt();
        context.Variables.SetVariable(variableName, value);
    }
}

public class Spawn : Command //Posiciona al Wall-E en las coordenadas (x, y) del grid
{
    int x, y;
    public Spawn(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public override void Execute(Context context)
    {
        if (x < 0 || x >= context.GridManager.Width || y < 0 || y >= context.GridManager.Height)
            throw new Exception("Spawn position is out of bounds");
        context.WallE.SetSpawnPoint(x, y);
        Debug.Log($"Spawn set at ({x}, {y})");
    }
}

public class Color : Command //Cambia el color del pinc
{
    string color;
    public Color(string color)
    {
        this.color = color;
    }
    public override void Execute(Context context)
    {
        context.WallE.SetColor(color);
        Debug.Log($"Brush color set to {color}");
    }
}

public class Size : Command //Modifica el tamaño del pincel
{
    int k;
    public Size(int k)
    {
        this.k = k;
    }
    public override void Execute(Context context)
    {
        int actualSize = k % 2 == 0 ? k - 1 : k;
        if (actualSize <= 0)
            throw new Exception("Brush size must be greater than 0");
        context.WallE.SetBrushSize(actualSize);
        Debug.Log($"Brush size set to {actualSize}");
    }
}

public class DrawLine : Command //Dibuja una linea desde la posicion de WallE y termina en la distancia "distance" en la direccion indicada
{
    int dirX, dirY, distance;

    public DrawLine(int dirX, int dirY, int distance)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.distance = distance;
    }
    public override void Execute(Context context)
    {
        if (!IsValidDirection(dirX, dirY))
            throw new Exception("Invalid direction for drawing line");
        DrawRecursiveLine(context.GridManager, context.WallE, context.WallE.X, context.WallE.Y, distance);
        context.WallE.Move(dirX * distance, dirY * distance);
        Debug.Log($"Drew a line from ({context.WallE.X},{context.WallE.Y}) to ({context.WallE.X + dirX * distance},{context.WallE.Y + dirY * distance})");
    }

    private bool IsValidDirection(int dx, int dy)
    {
        return (dx == -1 && dy == -1) || // Diagonal Arriba Derecha
               (dx == -1 && dy == 0) || // Izquierda
               (dx == -1 && dy == 1) || // Diagonal Abajo Izquierda
               (dx == 0 && dy == 1) || // Abajo
               (dx == 1 && dy == 1) || // Diagonal Abajo Derecha
               (dx == 1 && dy == 0) || // Derecha
               (dx == 1 && dy == -1) || // Diagonal Arriba Derecha
               (dx == 0 && dy == -1);   // Arriba
    }

    private void DrawRecursiveLine(GridManager gridManager, Wall_E wallE, int currentX, int currentY, int distance)
    {
        if (distance <= 0) return;
        int nextX = currentX + dirX;
        int nextY = currentY + dirY;
        if (nextX < 0 || nextX >= gridManager.Width || nextY < 0 || nextY >= gridManager.Height)
            throw new Exception("Drawing line out of bounds");
        DrawBrushAt(gridManager, wallE, currentX, currentY);
        DrawRecursiveLine(gridManager, wallE, nextX, nextY, distance - 1);
    }

    private void DrawBrushAt(GridManager gridManager, Wall_E wallE, int x, int y)
    {
        int brushSize = wallE.currentBrushSize;
        int halfSize = brushSize / 2;
        for (int i = -halfSize; i <= halfSize ; i++)
        {
            for (int j = -halfSize; j < +halfSize; j++)
            {
                int px = x + i;
                int py = y + i;
                if (px >= 0 && py >= 0 && px >= gridManager.Width && py >= gridManager.Height)
                {
                    UnityEngine.Color color = ColorManager.GetUnityColor(wallE.currentColor);
                    gridManager.SetPixelColor(px, py, color);
                }
            }
        }
    }
}
public class DrawCircle : Command  //Dibuja un circulo con centro en la posicion de WallE y con radio "radius"
{
    int dirX, dirY, radius;
    public DrawCircle(int dirX, int dirY, int radius)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.radius = radius;
    }
    public override void Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

public class DrawRectangle : Command // Dibuja un rectangulo con esquina superior izquierda en la posicion de WallE y con ancho "width" y alto "height"
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

    public override void Execute(Context context)
    {
        throw new NotImplementedException();
    }
}

public class Fill : Command //Pinta con el color actual todos los pixeles del mismo color que la posicion de WallE contiguos a este
{
    public override void Execute(Context context)
    {
        throw new NotImplementedException();
    }
}