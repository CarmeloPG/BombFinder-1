using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// A DinamicLayoutPage dinamically sets the size of holder of the Toggles
/// in a GridLayoutGroup. It is an abstract class, the way of set the size 
/// must be implemented in their subclasses.
/// The elements of the layout are a subclass of Toggle. When a element change its value
/// the page will send an UnityEvent with the selected element.
/// </summary>
public abstract class DinamycLayoutPage : MonoBehaviour {
    #region Serialize fields
    [SerializeField]
    [Tooltip("Check true if you want to be able select several activeToggles at the same timeReward.")]
    private bool selectionMultiple = false;
    [SerializeField]
    [Tooltip("When selectionMultiple is false, the activeToggles will be included in the toggleGroup.")]
    private ToggleGroup toggleGroup;
    [SerializeField]
    [Tooltip("Check true if you want a grid layout.")]
    protected bool gridLayout;
    [SerializeField]
    protected GridLayoutGroup togglesHolder;
    #endregion

    #region Protected fields
    private RectTransform togglesHolderRect;
    protected List<Toggle> toggles = new List<Toggle>();
    #endregion

    #region Properties
    protected RectTransform TogglesHolderRect {
        get {
            if (togglesHolderRect == null) {
                togglesHolderRect = togglesHolder.GetComponent<RectTransform>();
            }

            return togglesHolderRect;
        }
    }

    public int TogglesCount {
        get => toggles.Count;
    }

    public bool IsSelectionMultipleAllowed {
        get => selectionMultiple;
        set {
            if (value != selectionMultiple) {
                selectionMultiple = value;

                foreach (var toggle in toggles.ToArray()) {
                    if (selectionMultiple) {
                        toggleGroup.UnregisterToggle(toggle);
                    }
                    else {
                        toggleGroup.RegisterToggle(toggle);
                    }
                }
            }
        }
    }
    #endregion

    #region Events
    [Header("Events")]
    //[SerializeField]
    public UnityEvent<Toggle> OnToggleSelected;
    #endregion

    #region Unity methods
    private void Awake() {
        if (togglesHolderRect == null) {
            togglesHolderRect = togglesHolder.GetComponent<RectTransform>();
        }

        /*foreach (var toggle in togglesHolderRect.GetComponentsInChildren<Toggle>(true)) {
            RegisterToggle(toggle);
        }*/
    }

    private void OnDestroy() {
        foreach (var toggle in togglesHolderRect.GetComponentsInChildren<Toggle>(true)) {
            UnregisterToggle(toggle);
            DestroyImmediate(toggle.gameObject);
        }
    }
    #endregion

    #region Public methods
    /// <summary>
    /// Add a toggle to the list.
    /// </summary>
    /// <param name="toggle"> The toggle.</param>
    /// <param name="setSize"> If true, the size of activeTogglesHolder will set its size.</param>
    public void AddToggle(Toggle toggle, bool setSize = true) {
        var toggleTransform = toggle.GetComponent<RectTransform>();

        toggleTransform.SetParent(togglesHolder.transform);
        
        toggleTransform.localPosition = Vector3.zero;
        toggleTransform.localScale = Vector3.one;
        toggleTransform.localEulerAngles = Vector3.zero;

        toggle.gameObject.SetActive(true);
        
        RegisterToggle(toggle);

        if (setSize) {
            SetTogglesHolderSize();
        }

        //Debug.Log("Toggle added: " + toggle.name + " " + transform.parent.parent.name);
    }

    /// <summary>
    /// Add an array of activeToggles to the list.
    /// </summary>
    /// <param name="newToggles"> The array of activeToggles.</param>
    public void AddToggles(Toggle[] newToggles) {
        foreach (var toggle in newToggles) {
            AddToggle(toggle, false);
        }

        SetTogglesHolderSize();
    }

    /// <summary>
    /// It selects a toggle of the toggle list.
    /// </summary>
    /// <param name="toggleName">The name of the toggle.</param>
    public void SelectToggle(string toggleName) {
        Toggle theToggle = toggles.Find(toggle => toggle.name == toggleName);

        if (theToggle != null && !theToggle.isOn) {
            theToggle.isOn = true;
        }
    }

    /// <summary>
    /// Removes a toggle from the page. The toggle will not be destryed-
    /// After to be removed, tranform of the toggle will be child of the root of hierarchy
    /// and it will be disabled.
    /// </summary>
    /// <param name="toggle">The toggle to removed.</param>
    /// <param name="setHolderSize">If true the size of the togglesHolder size will be changed after to remove the toggle .</param>
    public void RemoveToggle(Toggle toggle, bool setHolderSize = true) {
        toggles.Remove(toggle);

        //Debug.Log("Toggle destroyed: " + toggle.name + " " + transform.parent.parent.name);

        UnregisterToggle(toggle);
        DestroyImmediate(toggle.gameObject);

        if (setHolderSize) {
            Invoke(nameof(SetTogglesHolderSize), 2f);
        }
    }

    /// <summary>
    /// Revomes an array of toggle in the page.
    /// </summary>
    /// <param name="togglesArray">The toggles to remove</param>
    /// /// <param name="setHolderSize">If true the size of the togglesHolder size will be changed after to remove the toggles.</param>
    /*public void RemoveToggles(Toggle[] togglesArray, bool setHolderSize = true) {
        foreach (var toggle in togglesArray) {
            RemoveToggle(toggle, false);
        }

        if (setHolderSize) {
            Invoke(nameof(SetTogglesHolderSize), 2f);
        }
    }*/

    /// <summary>
    /// Set the size of itemsHolderTransform.
    /// </summary>
    public abstract void SetTogglesHolderSize();

    /// <summary>
    /// Remove all toggles in the page.
    /// </summary>
    public void Clear() {
        /*int togglesCount = toggles.Count;

        for (int i = 0; i < togglesCount; i++) {
            RemoveToggle(toggles[i], false);
        }*/

        //RemoveToggles(toggles.ToArray(), false);

        while (toggles.Count > 0) {
            RemoveToggle(toggles[0], false);
        }

        Invoke(nameof(SetTogglesHolderSize), 2f);
    }
    #endregion

    #region Protected methods
    #endregion

    #region Private methods
    /// <summary>
    /// Registers a new Toggle in the page:
    ///     1. The toggle is sucribed to the OnToggleSelect event.
    ///     2. The toggle is added to the the activeToggles list.
    ///     3. If it's necesary, the toggle is registered in the toggleGroup.
    /// </summary>
    /// <param name="toggle">The new toggle.</param>
    private void RegisterToggle(Toggle toggle) {
        toggle.onValueChanged.AddListener(delegate {
            OnToggleSelected.Invoke(toggle);
        });

        toggles.Add(toggle);

        if (!selectionMultiple) {
            toggleGroup.RegisterToggle(toggle);
        }
    }


    /// <summary>
    /// Unregisters a Toggle in the page:
    ///     1. The toggle is sucribed to the OnToggleSelect event.
    ///     2. The toggle is added to the the activeToggles list.
    ///     3. If it's necesary, the toggle is registered in the toggleGroup.
    /// </summary>
    /// <param name="toggle">The toggle to remove.</param>
    private void UnregisterToggle(Toggle toggle) {
        toggle.onValueChanged.RemoveAllListeners();

        toggles.Remove(toggle);

        if (!selectionMultiple) {
            toggleGroup.UnregisterToggle(toggle);
        }
    }
    #endregion

    #region Coroutines
    #endregion
}