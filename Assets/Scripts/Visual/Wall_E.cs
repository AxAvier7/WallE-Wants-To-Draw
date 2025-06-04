using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class Wall_E
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public string currentColor { get; private set; }
    public int currentBrushSize { get; private set; }

    public Wall_E()
    {
        currentColor = "Transparent";
        currentBrushSize = 1;
    }

    public void SetSpawnPoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void SetColor(string color)
    {
        if (!ColorManager.IsValidColorName(color))
            throw new ArgumentException("Invalid color");
        currentColor = color;
    }

    public void SetBrushSize(int size)
    {
        if (size <= 0)
            throw new ArgumentException("Brush size must be greater than zero");
        currentBrushSize = size % 2 == 0 ? size - 1 : size;
    }
    public void Move(int deltaX, int deltaY)
    {
        X += deltaX;
        Y += deltaY;
    }

    public int GetActualX() => X;
    public int GetActualY() => Y;
}
