using System;

//Clase que representa al WallE
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

    //Ubica al WallE en (x,y)
    public void SetSpawnPoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    //Hace que el color actual sea el recibido por el metodo
    public void SetColor(string color)
    {
        if (!ColorManager.IsValidColorName(color))
            throw new ArgumentException("Invalid color");
        currentColor = color;
    }

    //Modifica el tamaño del pincel para que sea size si es impar o size - 1 si es par
    public void SetBrushSize(int size)
    {
        if (size <= 0)
            throw new ArgumentException("Brush size must be greater than zero");
        currentBrushSize = size % 2 == 0 ? size - 1 : size;
    }

    //Metodo que mueve a WallE
    public void Move(int deltaX, int deltaY)
    {
        X += deltaX;
        Y += deltaY;
    }

    //Metodos que devuelven la x y la y actuales donde se ubique WallE
    public int GetActualX() => X;
    public int GetActualY() => Y;
}
