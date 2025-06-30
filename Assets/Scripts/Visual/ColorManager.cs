using System.Collections.Generic;

//Clase para facilitar el uso de los colores de Unity
public static class ColorManager
{
    //Diccionario con los colores en string y su representacion en la paleta de colores de Unity
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

    public static readonly Dictionary<TokenType, string> ColorTokenToString = new()
    {
        { TokenType.Red, "Red" },
        { TokenType.Blue, "Blue" },
        { TokenType.Green, "Green" },
        { TokenType.Yellow, "Yellow" },
        { TokenType.Orange, "Orange" },
        { TokenType.Purple, "Purple" },
        { TokenType.Black, "Black" },
        { TokenType.White, "White" },
        { TokenType.Gray, "Gray" },
        { TokenType.Pink, "Pink" },
        { TokenType.LightBlue, "LightBlue" },
        { TokenType.LightGreen, "LightGreen" },
        { TokenType.Brown, "Brown" },
        { TokenType.LightGray, "LightGray" },
        { TokenType.Transparent, "Transparent" }
    };

    public static UnityEngine.Color GetUnityColor(string colorName)
    {
        //out funciona para decirle a TryGetValue que debe asignar un valor a color cuando se ejecute
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