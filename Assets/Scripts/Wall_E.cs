using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

public class Wall_E
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public string currentColor { get; private set; }
    public int currentBrushSize { get; private set; }

    private Dictionary<string, UnityEngine.Color> colorMap = new Dictionary<string, UnityEngine.Color>()
    {
        {"Red", UnityEngine.Color.red},
        {"Green", UnityEngine.Color.green},
        {"Blue", UnityEngine.Color.blue},
        {"Yellow", UnityEngine.Color.yellow},
        {"Black", UnityEngine.Color.black},
        {"White", UnityEngine.Color.white},
        {"Orange", new UnityEngine.Color(1f, 0.65f, 0f)},
        {"Purple", new UnityEngine.Color(0.5f, 0f, 0.5f)},
        {"Transparent", new UnityEngine.Color(0, 0, 0, 0)}
    };

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
        if (!IsValidColor(color))
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

    public bool IsValidColor(string color)
    {
        List<string> availableColors = new List<string> { "Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Black", "White", "Transparent" };
        if (availableColors.Contains(color))
        {
            return true;
        }
        return false;
    }

    public int GetActualX() => X;
    public int GetActualY() => Y;

    public UnityEngine.Color GetUnityColor(string colorName)
    {
        if (string.IsNullOrEmpty(colorName))
        {
            Debug.LogWarning("Color name is null or empty. Returning transparent");
            return colorMap["Transparent"];
        }
        if (colorMap.ContainsKey(colorName))
            return colorMap[colorName];
        Debug.LogWarning("Unknown color. Returning transparent");
        return colorMap["Transparent"];
    }
}
