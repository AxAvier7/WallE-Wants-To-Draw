using UnityEngine;
using UnityEngine.UI;

// Clase que representa a los pixeles que se ubican en el grid
public class Pixel : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }
    private Image image;
    private AspectRatioFitter aspectRatioFitter; //componente que ajusta la proporcion del pixel para que este sea cuadrado

    void Awake()
    {
        image = GetComponent<Image>();
        aspectRatioFitter = GetComponent<AspectRatioFitter>();
        aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
        aspectRatioFitter.aspectRatio = 1;

        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void Initialize(int x, int y)
    {
        X = x; Y = y;
        SetColor(UnityEngine.Color.white);
    }

    public void SetColor(UnityEngine.Color color)
    {
        if (image.color == color) return;
        image.color = color;
    }
}