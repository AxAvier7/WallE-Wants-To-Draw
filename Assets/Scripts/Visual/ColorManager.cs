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
        {"Gray", new UnityEngine.Color(120, 120, 120, 255)},
        {"Pink", new UnityEngine.Color(255, 0, 255, 255)},
        {"LightBlue", new UnityEngine.Color(0, 200, 255, 255)},
        {"LightGreen", new UnityEngine.Color(0, 255, 100, 255)},
        {"Brown", new UnityEngine.Color(85, 45, 0, 255)},
        {"LightGray", new UnityEngine.Color(160, 160, 160, 255)},
        { "Transparent", new UnityEngine.Color(0, 0, 0, 0)}
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