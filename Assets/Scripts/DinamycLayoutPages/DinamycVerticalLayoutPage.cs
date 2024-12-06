using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// Set the height of the togglesHolder in function of the number of elements.
/// The width of the togglesHolder is constant.
/// </summary>
public class DinamycVerticalLayoutPage : DinamycLayoutPage {

    #region Public methods
    public override void SetTogglesHolderSize() {
        var toggleSize = togglesHolder.cellSize;
        float width = gridLayout? TogglesHolderRect.sizeDelta.x : toggleSize.x;
        int numColumns = Mathf.Max(Mathf.FloorToInt(width / toggleSize.x), 1);
        int numRows = (togglesHolder.GetComponentsInChildren<Toggle>(false).Length / numColumns);// + 1; //(toggles.Count / numColumns) + 1;
        float height = numRows * (toggleSize.y - togglesHolder.spacing.y - togglesHolder.padding.top);

        TogglesHolderRect.sizeDelta = new Vector2(width, height);
    }
    #endregion
}