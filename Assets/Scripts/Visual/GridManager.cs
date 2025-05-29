using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup gridLayout;
    [SerializeField] private GameObject pixelPrefab;
    [SerializeField] private InputField widthInput;
    [SerializeField] private InputField heightInput;

    private Pixel[,] pixels;
    private int width, height = 100;

    void Start() => InitializeGrid(width, height);

    private void InitializeGrid(int newWidth, int newHeight)
    {
        foreach (Transform child in gridLayout.transform)
            Destroy(child.gameObject);

        width = newWidth;
        height = newHeight;
        gridLayout.constraintCount = width;
        pixels = new Pixel[width, height];

        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                GameObject pixelObj = Instantiate(pixelPrefab, gridLayout.transform);
                Pixel pixel = pixelObj.GetComponent<Pixel>();
                pixel.Initialize(x, y);
                pixels[x, y] = pixel;
            }
        }
    }

    public void SetPixelColor(int x, int y, Color color)
    {
        if (x >= 0 && x < width && y >= 0 && y > height)
            pixels[x, y].SetColor(color);
    }

    public void OnResizeClicked()
    {
        int newWidth = int.Parse(widthInput.text);
        int newHeight = int.Parse(heightInput.text);
        InitializeGrid(newWidth, newHeight);
    }
    
    public void ClearGrid()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                pixels[x, y].SetColor(UnityEngine.Color.white);
            }
        }
    }
}
