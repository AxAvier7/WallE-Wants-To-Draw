using System;
using System.Collections.Generic;
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
        if (distance <= 0)
            throw new ArgumentException("Distance must be positive");
        this.dirX = dirX;
        this.dirY = dirY;
        this.distance = distance;
    }
    public override void Execute(Context context)
    {
        if (context.WallE.currentColor == "Transparent")
        {
            context.SetError(Line, "Cannot draw with transparent color");
            return;
        }
        if (!IsValidDirection(dirX, dirY))
            context.SetError(Line, "Invalid direction for drawing line");
        DrawRecursiveLine(context, context.WallE.X, context.WallE.Y, distance);
        context.WallE.Move(dirX * distance, dirY * distance);
        Debug.Log($"Drew a line from ({context.WallE.X},{context.WallE.Y}) to ({context.WallE.X + dirX * distance},{context.WallE.Y + dirY * distance}) with {context.WallE.currentColor}");
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
               (dx == 0 && dy == -1) ||
               (dx == 0 && dy == 0);   // Arriba
    }

    private void DrawRecursiveLine(Context context, int currentX, int currentY, int distance)
    {
        if (distance <= 0) return;

        int nextX = currentX + dirX;
        int nextY = currentY + dirY;

        if (nextX < 0 || nextX >= context.GridManager.Width || nextY < 0 || nextY >= context.GridManager.Height)
            context.SetError(Line, "Drawing line out of bounds");

        DrawingUtils.DrawBrushAt(context, currentX, currentY);
        DrawRecursiveLine(context, nextX, nextY, distance - 1);
    }
}

public class DrawCircle : Command  //Dibuja un circulo con centro en la posicion de WallE y con radio "radius"
{
    int dirX, dirY, radius;
    public DrawCircle(int dirX, int dirY, int radius, int line = 0, int column = 0)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.radius = radius;
        Line = line;
        Column = column;
    }
    public override void Execute(Context context)
    {
        if (context.WallE.currentColor == "Transparent")
        {
            context.WallE.Move(dirX * radius, dirY * radius);
            return;
        }

        int centerX = context.WallE.X + dirX * radius;
        int centerY = context.WallE.Y + dirY * radius;
        if (centerX < 0 || centerX >= context.GridManager.Width || centerY < 0 || centerY >= context.GridManager.Height)
        {
            context.SetError(Line, "Circle center out of bounds");
            return;
        }

        DrawCirclePoints(context, centerX, centerY, radius);
        context.WallE.SetSpawnPoint(centerX, centerY);
    }

    private void DrawCirclePoints(Context context, int centerX, int centerY, int radius)
    {
        int x = radius;
        int y = 0;
        int isDrawOver = 1 - x;

        while (x >= y)
        {
            DrawingUtils.DrawBrushAt(context, centerX + x, centerY + y);
            DrawingUtils.DrawBrushAt(context, centerX - x, centerY + y);
            DrawingUtils.DrawBrushAt(context, centerX + x, centerY - y);
            DrawingUtils.DrawBrushAt(context, centerX - x, centerY - y);
            DrawingUtils.DrawBrushAt(context, centerX + y, centerY + x);
            DrawingUtils.DrawBrushAt(context, centerX - y, centerY + x);
            DrawingUtils.DrawBrushAt(context, centerX + y, centerY - x);
            DrawingUtils.DrawBrushAt(context, centerX - y, centerY - x);

            y++;
            if (isDrawOver <= 0)
                isDrawOver += 2 * y + 1;
            else
            {
                x--;
                isDrawOver += 2 * (y - x) + 1;
            }
        }
    }
}

public class DrawRectangle : Command // Dibuja un rectangulo con esquina superior izquierda en la posicion de WallE y con ancho "width" y alto "height"
{
    int dirX, dirY, distance, width, height;
    public DrawRectangle(int dirX, int dirY, int distance, int width, int height, int line = 0, int column = 0)
    {
        this.dirX = dirX;
        this.dirY = dirY;
        this.distance = distance;
        this.width = width;
        this.height = height;
        Line = line;
        Column = column;
    }

    public override void Execute(Context context)
    {
        if (context.WallE.currentColor == "Transparent")
        {
            context.WallE.Move(dirX * distance, dirY * distance);
            return;
        }

        int centerX = context.WallE.X + dirX * distance;
        int centerY = context.WallE.Y + dirY * distance;

        int halfWidth = width / 2;
        int halfHeight = height / 2;

        int left = centerX - halfWidth;
        int top = centerY - halfHeight;
        int right = centerX + halfWidth;
        int bottom = centerY + halfHeight;

        if (centerX < 0 || centerX >= context.GridManager.Width || centerY < 0 || centerY >= context.GridManager.Height)
        {
            context.SetError(Line, "Circle center out of bounds");
            return;
        }

        DrawingUtils.DrawLineBetweenPoints(context, left, top, right, top);
        DrawingUtils.DrawLineBetweenPoints(context, right, top, right, bottom);
        DrawingUtils.DrawLineBetweenPoints(context, right, bottom, left, bottom);
        DrawingUtils.DrawLineBetweenPoints(context, left, bottom, left, top);

        context.WallE.SetSpawnPoint(centerX, centerY);
    }
}

public class Fill : Command // Pinta con el color actual todos los pixeles del mismo color que la posicion de WallE contiguos a este
{
    public Fill(int line = 0, int column = 0)
    {
        Line = line;
        Column = column;
    }
    public override void Execute(Context context)
    {
        if (context.WallE.currentColor == "Transparent")
            return;

        string targetColor = context.GridManager.GetPixelColorName(context.WallE.X, context.WallE.Y);

        if (targetColor == context.WallE.currentColor)
            return;

        FloodFill(context, context.WallE.X, context.WallE.Y, targetColor);
    }

    private void FloodFill(Context context, int startX, int startY, string targetColor)
    {
        Queue<Vector2Int> qeue = new Queue<Vector2Int>();
        qeue.Enqueue(new Vector2Int(startX, startY));

        Vector2Int[] directions = {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0)
        };

        while (qeue.Count > 0)
        {
            Vector2Int point = qeue.Dequeue();
            int x = point.x;
            int y = point.y;

            if (x < 0 || x >= context.GridManager.Width ||
                y < 0 || y >= context.GridManager.Height)
                continue;

            if (context.GridManager.GetPixelColorName(x, y) != targetColor)
                continue;

            context.GridManager.SetPixelColor(x, y, context.WallE.currentColor);

            foreach (Vector2Int dir in directions)
            {
                qeue.Enqueue(new Vector2Int(x + dir.x, y + dir.y));
            }
        }
    }
}