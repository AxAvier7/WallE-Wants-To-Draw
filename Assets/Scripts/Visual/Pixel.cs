using UnityEngine;
using UnityEngine.UI;

public class Pixel : MonoBehaviour
{
    public int X { get; private set; }
    public int Y { get; private set; }
    private Image image;
    private AspectRatioFitter aspectRatioFitter;

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

    public void SetColor(UnityEngine.Color color) => image.color = color;
    public void UpdateColor()
    {
        Canvas.ForceUpdateCanvases();
        image.SetAllDirty();
    }
}
