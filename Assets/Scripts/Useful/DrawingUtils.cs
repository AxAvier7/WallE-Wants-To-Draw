using UnityEngine;

public static class DrawingUtils
{
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
    
    public static void DrawLineBetweenPoints(
        Context context, 
        int startX, 
        int startY, 
        int endX, 
        int endY)
    {
        int dx = Mathf.Abs(endX - startX);
        int dy = Mathf.Abs(endY - startY);
        int sx = startX < endX ? 1 : -1;
        int sy = startY < endY ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            DrawingUtils.DrawBrushAt(context, startX, startY);
            
            if (startX == endX && startY == endY)
                break;
            
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                startX += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                startY += sy;
            }
        }
    }
}