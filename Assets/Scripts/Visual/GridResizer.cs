using UnityEngine;
using UnityEngine.UI;

public class GridResizer : MonoBehaviour
{
    private GridLayoutGroup gridLayout;
    private RectTransform rectTransform;
    private bool isInitialized = false;

    void Awake()
    {
        InitializeComponents();
    }

    void Start()
    {
        InitializeComponents();
        ResizeGrid();
    }

    private void OnRectTransformDimensionsChange()
    {
        if (isInitialized)
            ResizeGrid();
    }

    private void InitializeComponents()
    {
        if (isInitialized) return;
        
        gridLayout = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
        
        if (gridLayout == null || rectTransform == null)
            return;
        
        isInitialized = true;
    }

    public void ResizeGrid()
    {
        InitializeComponents();

        if (!isInitialized || rectTransform.rect.width <= 0 || gridLayout.constraintCount <= 0)
            return;

        float availableWidth = rectTransform.rect.width - gridLayout.padding.left - gridLayout.padding.right;
        float availableHeight = rectTransform.rect.height - gridLayout.padding.top - gridLayout.padding.bottom;
        float maxCellSize = Mathf.Min(
            availableWidth / gridLayout.constraintCount,
            availableHeight / gridLayout.constraintCount
        );
        gridLayout.cellSize = new Vector2(maxCellSize, maxCellSize);
    }
}
