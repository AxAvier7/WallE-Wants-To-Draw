using System.Collections.Generic;

public static class ColorManager
{
    private static readonly Dictionary<string, UnityEngine.Color> colorMap = new Dictionary<string, UnityEngine.Color>
    {
        {"Red", UnityEngine.Color.red},
        {"Blue", UnityEngine.Color.blue},
        {"Green", UnityEngine.Color.green},
        {"Yellow", UnityEngine.Color.yellow},
        {"Orange", new UnityEngine.Color(1f, 0.65f, 0f)},
        {"Purple", new UnityEngine.Color(0.5f, 0f, 0.5f)},
        {"Black", UnityEngine.Color.black},
        {"White", UnityEngine.Color.white},
        {"Transparent", new UnityEngine.Color(0,0,0,0)}
    };

    public static UnityEngine.Color GetUnityColor(string colorName)
    {
        if (colorMap.TryGetValue(colorName, out UnityEngine.Color color))
        {
            return color;
        }
        return UnityEngine.Color.white;
    }
    public static bool IsValidColorName(string color)
    {
        return colorMap.ContainsKey(color);
    }
}