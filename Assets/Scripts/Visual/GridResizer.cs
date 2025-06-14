using UnityEngine;
using UnityEngine.UI;

public class GridResizer : MonoBehaviour
{
    private GridLayoutGroup gridLayout;
    private RectTransform rectTransform;

    void Awake()
    {
        gridLayout = GetComponent<GridLayoutGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        ResizeGrid();
    }

    public void ResizeGrid()
    {
        if (rectTransform.rect.width <= 0 || gridLayout.constraintCount <= 0)
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
