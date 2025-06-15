using System.Collections.Generic;
using UnityEngine;

public static class DrawingUtils
{

    public static void DrawLineBresenham(Context context, int x0, int y0, int x1, int y1)
    {
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;
        int currentX = x0;
        int currentY = y0;

        while (true)
        {
            DrawBrushAt(context, currentX, currentY);

            if (currentX == x1 && currentY == y1) break;

            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                currentX += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                currentY += sy;
            }
        }
    }

    public static void DrawBrushAt(Context context, int x, int y)
    {
        string currentColor = context.GridManager.GetPixelColorName(x, y);
        if (currentColor == context.WallE.currentColor) 
            return;
            
        int brushSize = context.WallE.currentBrushSize;
        int halfSize = brushSize / 2;

        for (int i = -halfSize; i <= halfSize; i++)
        {
            for (int j = -halfSize; j <= halfSize; j++)
            {
                int px = x + i;
                int py = y + j;

                if (px >= 0 && px < context.GridManager.Width &&
                    py >= 0 && py < context.GridManager.Height)
                {
                    context.GridManager.SetPixelColor(
                        px,
                        py,
                        context.WallE.currentColor
                    );
                }
            }
        }
    }

    public static void DrawLineBetweenPoints(Context context, int startX, int startY, int endX, int endY)
    {
        int dx = Mathf.Abs(endX - startX);
        int dy = Mathf.Abs(endY - startY);
        int steps = Mathf.Max(dx, dy);

        if (steps == 0)
        {
            DrawingUtils.DrawBrushAt(context, startX, startY);
            return;
        }

        float xIncrement = (endX - startX) / (float)steps;
        float yIncrement = (endY - startY) / (float)steps;

        float x = startX;
        float y = startY;

        for (int i = 0; i <= steps; i++)
        {
            DrawingUtils.DrawBrushAt(context, Mathf.RoundToInt(x), Mathf.RoundToInt(y));
            x += xIncrement;
            y += yIncrement;
        }
    }

    public static bool IsValidDirection(int dx, int dy)
    {
        return (dx == -1 && dy == -1) || // Diagonal Arriba Derecha
               (dx == -1 && dy == 0) || // Izquierda
               (dx == -1 && dy == 1) || // Diagonal Abajo Izquierda
               (dx == 0 && dy == 1) || // Abajo
               (dx == 1 && dy == 1) || // Diagonal Abajo Derecha
               (dx == 1 && dy == 0) || // Derecha
               (dx == 1 && dy == -1) || // Diagonal Arriba Derecha
               (dx == 0 && dy == -1) || // Arriba
               (dx == 0 && dy == 0); //No se mueve
    }

    public static void DrawCirclePoints(Context context, int centerX, int centerY, int radius)
    {
        int x = radius;
        int y = 0;
        int isDrawOver = 1 - x;

        while (x >= y)
        {
            DrawBrushAt(context, centerX + x, centerY + y);
            DrawBrushAt(context, centerX - x, centerY + y);
            DrawBrushAt(context, centerX + x, centerY - y);
            DrawBrushAt(context, centerX - x, centerY - y);
            DrawBrushAt(context, centerX + y, centerY + x);
            DrawBrushAt(context, centerX - y, centerY + x);
            DrawBrushAt(context, centerX + y, centerY - x);
            DrawBrushAt(context, centerX - y, centerY - x);

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

    public static void FloodFill(Context context, int startX, int startY, string targetColor)
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