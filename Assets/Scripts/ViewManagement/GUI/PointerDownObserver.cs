using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PointerDownObserver : MonoBehaviour, IPointerDownHandler {
    #region Readonly fields
    #endregion

    #region Serialize fields
    #endregion

    #region Private fields
    #endregion

    #region Properties
    #endregion

    #region Events
    [Header("Events")]
    [Tooltip("When The pointer downs, it sends an event.")]
    [SerializeField]
    private UnityEvent OnPress;
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData) {
        OnPress.Invoke(); 
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    #endregion

    #region Coroutines
    #endregion
}