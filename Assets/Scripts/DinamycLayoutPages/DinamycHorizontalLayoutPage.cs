using UnityEngine;

/// <summary>
/// Set the width of the togglesHolder in function of the number of elements.
/// The height of the togglesHolder is constant.
/// </summary>
public class DinamycHorizontalLayoutPage : DinamycLayoutPage {
    #region Public methods
    public override void SetTogglesHolderSize() {
        var toggleSize = togglesHolder.cellSize;
        float height = gridLayout ? TogglesHolderRect.sizeDelta.y : toggleSize.y;
        int numRows = Mathf.Max(Mathf.FloorToInt(height / toggleSize.y), 1);
        int numColumns = (toggles.Count / numRows) + 1;
        float width = numColumns * (toggleSize.x - togglesHolder.spacing.x);

        TogglesHolderRect.sizeDelta = new Vector2(width, height);
    }
    #endregion
}