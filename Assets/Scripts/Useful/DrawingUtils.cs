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
        int steps = Mathf.Max(dx,dy);

        if(steps==0)
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
}