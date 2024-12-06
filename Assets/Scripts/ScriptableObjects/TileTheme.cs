using UnityEngine;

/// <summary>
/// A TileTheme is an array of tiles that have the same theme of a stage.
/// </summary>
[CreateAssetMenu(fileName = "New TileTheme", menuName = "ScriptableObjects/TileTheme")]
public class TileTheme : ScriptableObject {
    #region Readonly fields
    #endregion

    #region Serialize fields
    [SerializeField]
    private Tile[] tilePrefabs;
    #endregion

    #region Private fields
    #endregion

    #region Properties
    public Tile[] TilePrefabs {
        get => tilePrefabs;
    }
    #endregion

    #region Events
    //[Header("Events")]
    //[Tooltip("")]
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}