using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject pixelPrefab;
    [SerializeField] private InputField widthInput;

    private Pixel[,] pixels;
    private string[,] colorNames;
    public int Width { get; private set; }
    public int Height { get; private set; }

    void Start()
    {
        InitializeGrid(10, 10);
        GetComponent<GridResizer>().ResizeGrid();
    }

    private void InitializeGrid(int newWidth, int newHeight)
    {
        foreach (Transform child in gridLayout.transform)
            Destroy(child.gameObject);

        Width = newWidth;
        Height = newHeight;
        gridLayout.constraintCount = Width;
        pixels = new Pixel[Width, Height];
        colorNames = new string[Width, Height];

        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                GameObject pixelObj = Instantiate(pixelPrefab, gridLayout.transform);
                Pixel pixel = pixelObj.GetComponent<Pixel>();
                pixel.Initialize(x, y);
                pixels[x, y] = pixel;
                colorNames[x, y] = "White";
            }
        }
    }

    public void SetPixelColor(int x, int y, UnityEngine.Color color)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
            pixels[x, y].SetColor(color);
    }

    public void SetPixelColor(int x, int y, string colorName)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            UnityEngine.Color color = ColorManager.GetUnityColor(colorName);
            pixels[x, y].SetColor(color);
            colorNames[x, y] = colorName;
            pixels[x, y].UpdateColor();
        }
    }

    public string GetPixelColorName(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return colorNames[x, y];
        }
        return "";
    }

    public void OnResizeClicked()
    {
        int newWidth = int.Parse(widthInput.text);
        if (newWidth > 256)
        {
            Debug.LogError("Exceded the size limit. Maximum: 256");
        }
        InitializeGrid(newWidth, newWidth);
        GetComponent<GridResizer>().ResizeGrid();
    }

    public void ClearGrid()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                pixels[x, y].SetColor(UnityEngine.Color.white);
                colorNames[x, y] = "White";
            }
        }
    }
}