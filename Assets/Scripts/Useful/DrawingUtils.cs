using System.Collections.Generic;
using UnityEngine;

//Clase con metodos usados muchas veces por los Comandos
public static class DrawingUtils
{
    //Algoritmo para dibujar lineas
    public static void DrawLineBresenham(Context context, int x0, int y0, int x1, int y1)
    {
        //Se calcula la distancia que se debe dibujar
        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);

        //Se determina en que sentido se va a dibujar
        int sx = x0 < x1 ? 1 : -1; //izquierda o derecha
        int sy = y0 < y1 ? 1 : -1; //arriba o abajo


        int err = dx - dy; //el error inicial, se usa sobre todo en casos de lineas diagonales
        int currentX = x0;
        int currentY = y0;

        while (true)
        {
            //Se dibuja el pixel actual
            DrawBrushAt(context, currentX, currentY);

            if (currentX == x1 && currentY == y1) break;

            //Se calcula el error para determinar si se debe mover en x o en y
            int e2 = 2 * err;
            //Se mueve por las X
            if (e2 > -dy)
            {
                err -= dy;
                currentX += sx;
            }
            //Se mueve por las Y
            if (e2 < dx)
            {
                err += dx;
                currentY += sy;
            }
        }
    }

    //Metodo que dibuja el pixel que este en (x,y) con el color actual del WallE
    public static void DrawBrushAt(Context context, int x, int y)
    {
        string currentColor = context.GridManager.GetPixelColorName(x, y);
        if (currentColor == context.WallE.currentColor)
            return;

        int brushSize = context.WallE.currentBrushSize;
        int halfSize = brushSize / 2;

        //Con este doble for se dibuja un cuadrado alrededor de (x,y) con el tamaño actual del pincel
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

    //Metodo para dibujar una linea entre los puntos dados
    public static void DrawLineBetweenPoints(Context context, int startX, int startY, int endX, int endY)
    {
        //Se calcula la distancia entre el inicio y el final
        int dx = Mathf.Abs(endX - startX);
        int dy = Mathf.Abs(endY - startY);

        //Se determina cuantos pasos se van a dar
        int steps = Mathf.Max(dx, dy);

        //Si no hay que dar pasos se dibuja el pixel actual
        if (steps == 0)
        {
            DrawBrushAt(context, startX, startY);
            return;
        }

        //Se normaliza el movimiento a realizar en cada eje
        float xIncrement = (endX - startX) / (float)steps;
        float yIncrement = (endY - startY) / (float)steps;

        float x = startX;
        float y = startY;

        //Se dibuja la cantidad de casillas indicadas en los steps
        for (int i = 0; i <= steps; i++)
        {
            DrawBrushAt(context, Mathf.RoundToInt(x), Mathf.RoundToInt(y));
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

    //Metodo llamado por el comando DrawCircle para pintar los puntos que forman el circulo
    public static void DrawCirclePoints(Context context, int centerX, int centerY, int radius)
    {
        int x = radius;
        int y = 0;
        int isDrawOver = 1 - x; //parametro que decide hacia donde se dibujara

        while (x >= y)
        {
            //se dibuja por simetria los puntos
            DrawBrushAt(context, centerX + x, centerY + y);
            DrawBrushAt(context, centerX - x, centerY + y);
            DrawBrushAt(context, centerX + x, centerY - y);
            DrawBrushAt(context, centerX - x, centerY - y);
            DrawBrushAt(context, centerX + y, centerY + x);
            DrawBrushAt(context, centerX - y, centerY + x);
            DrawBrushAt(context, centerX + y, centerY - x);
            DrawBrushAt(context, centerX - y, centerY - x);

            y++;
            if (isDrawOver <= 0) //se dibujara horizontalmente
                isDrawOver += 2 * y + 1;
            else //se dibujara diagonalmente
            {
                x--;
                isDrawOver += 2 * (y - x) + 1;
            }
        }
    }

    //Metodo que llama el Comando Fill y que rellena todas las casillas contiguas a la actual y que tengan el mismo color que esat con otro color
    public static void FloodFill(Context context, int startX, int startY, string targetColor)
    {
        Queue<Vector2Int> qeue = new Queue<Vector2Int>();
        qeue.Enqueue(new Vector2Int(startX, startY)); //guardamos el pixel actual

        Vector2Int[] directions = {
            new Vector2Int(0, 1),
            new Vector2Int(1, 0),
            new Vector2Int(0, -1),
            new Vector2Int(-1, 0)
        };

        while (qeue.Count > 0)
        {
            Vector2Int point = qeue.Dequeue(); //sacamos el primer pixel de la cola
            int x = point.x;
            int y = point.y;

            if (x < 0 || x >= context.GridManager.Width ||
                y < 0 || y >= context.GridManager.Height)
                continue;

            if (context.GridManager.GetPixelColorName(x, y) != targetColor)
                continue;

            context.GridManager.SetPixelColor(x, y, context.WallE.currentColor); //lo pintamos

            foreach (Vector2Int dir in directions) //añadimos todos sus pixeles contiguos a la cola
            {
                qeue.Enqueue(new Vector2Int(x + dir.x, y + dir.y));
            }
        }
    }
}