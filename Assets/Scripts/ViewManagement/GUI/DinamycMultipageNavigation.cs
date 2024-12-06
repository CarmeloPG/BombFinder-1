using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// Allows us to divide the content of a long list of Toggles of a DynamicLayoutPage
/// in several DynamicLayoutPage with a fixed number of Toggles and navigate through them.
/// 
/// When a Toggle of any page is selected, it sends an event with the selected Toggle.
/// When we want to go to a worng page, it sends a worng page event.
/// </summary>
public class DinamycMultipageNavigation : MonoBehaviour {
	#region Serialize fields
	[SerializeField]
	private DinamycLayoutPage pagePrefab;
	[SerializeField]
	[Range(1, 100)]
	private int maxTogglesInPage = 25;
	[SerializeField]
	private RectTransform pagesHolder;
	[SerializeField]
	private Button prevPageButton;
	[SerializeField]
	private Button nextPageButton;
	[SerializeField]
	private TMP_InputField currentPageInputField;
	#endregion

	#region Private fields
	private List<DinamycLayoutPage> pages;
	private DinamycLayoutPage currentPage;
	private int currentPageIndex;
	#endregion

	#region Properties
	#endregion

	#region Events
	[Header("Events")]
	[Tooltip("Sends the selected Toggle.")]
	public UnityEvent<Toggle> ToggleSelected;
    #endregion

    #region Unity methods
    #endregion

    #region Public methods
    /// <summary>
    /// It sets the first page.
    /// </summary>
    public void LoadNavigation() {
		if (pages == null) {
			pages = new List<DinamycLayoutPage>();

			CreateNewPage();
		}
    }

	/// <summary>
	/// It selects a toggle of the current page.
	/// </summary>
	/// <param name="toggleName">The name of the toggle.</param>
	public void SelectToggle(string toggleName) {
		currentPage.SelectToggle(toggleName);
	}

	/// <summary>
	/// 
	/// </summary>
    public void OnCurrentPageInputFiledEndEdit() {
		int objetivePage;

		if (int.TryParse(currentPageInputField.text, out objetivePage)) {
			GoToPage(objetivePage);
		}
		else {
			currentPageInputField.text = currentPageIndex.ToString();
		}
	}

	/// <summary>
	/// Go to next page.
	/// </summary>
	public void OnNexPageButtonPress() {
		GoToPage(currentPageIndex + 1);
	}

    /// <summary>
    /// Go to next page.
    /// </summary>
    public void OnPrevPageButtonPress() {
		GoToPage(currentPageIndex - 1);
	}

	/// <summary>
	/// Adds a new toggle in the currentPage.
	/// If the current page is full, it instantiates
	/// a nes page and add the toggle in it.
	/// </summary>
	/// <param name="toggle">The Toggle witch is added.</param>
	public void AddToggle(Toggle toggle) {
		if (currentPage == null || currentPage.TogglesCount >= maxTogglesInPage) {
			CreateNewPage();
        }

        currentPage.AddToggle(toggle);
    }

	/// <summary>
	/// Adds an arry of new toggles in the currentPage.
	/// </summary>
	/// <param name="toggles"></param>
	public void AddToggles(Toggle[] toggles) {
		foreach (var toggle in toggles) {
			AddToggle(toggle);
		}
	}

    /// <summary>
    /// Removes a toggle from the current page.
	/// If current page is empty, it will destroy 
	/// the current page and it will go to 
	/// the previous page, if it exists. 
    /// </summary>
    /// <param name="toggle">The toggle to remove.</param>
    /*public void RemoveToggleFromCurrentPage(Toggle toggle) {
		string id = toggle.GetComponent<PlayObjectInfoBox>().Data.Identifier;

		currentPage.RemoveToggle(toggle);

        if (currentPage.TogglesCount == 0) {
            currentPage.OnToggleSelected.RemoveListener(OnToggleSelect);
            pages.Remove(currentPage);

            Debug.Log("Page destroyed: " + currentPageIndex);

            if (pages.Count > 1) {
				DestroyImmediate(currentPage);

				GoToPage(currentPageIndex - 1);
			}
		}
    }

	/// <summary>
	/// Removes an array of toggles from the current page.
	/// </summary>
	/// <param name="theToggles"></param>
	public void RemoveTogglesFromCurrentPage(Toggle[] theToggles) {
		foreach (var toggle in theToggles) {
			RemoveToggleFromCurrentPage(toggle);
		}
	}*/

	/// <summary>
	/// Removes all pages and their toggles.
	/// </summary>
	public void Clear() {
		DinamycLayoutPage page;

		currentPage.Clear();

		for (int i = 0; i < pages.Count; i++) {
			page = pages[i];

            page.Clear();

            pages.Remove(page);

            if (i > 0) {
				DestroyImmediate(pages[i].gameObject);
            }
        }
	}

	public void SetCurrentPageSize() {
		currentPage.SetTogglesHolderSize();
	}
	#endregion

	#region Protected methods
	#endregion

	#region Private methods
	/// <summary>
	/// It shows the new pages after it has been created.
	/// </summary>
	private void CreateNewPage() {
        var newPage = Instantiate(pagePrefab, pagesHolder);

        newPage.OnToggleSelected.AddListener(OnToggleSelect);

        pages.Add(newPage);
        GoToPage(pages.Count - 1);
    }

    /// <summary>
    /// When a toggle changes its state,
    /// sends a UnityEvent with the toggle.
    /// </summary>
    /// <param name="toggle"></param>
    private void OnToggleSelect(Toggle toggle) {
		ToggleSelected.Invoke(toggle);
	}

	/// <summary>
	/// Sets active the page number index. Index must be
	/// in the bounds of pages list.
	/// </summary>
	/// <param name="index">The index of the new page</param>
	private void GoToPage(int index) {
		int pageIndex = Mathf.Clamp(index, 0, pages.Count - 1);

        if (currentPageIndex > 0) {
            pages[currentPageIndex].gameObject.SetActive(false);
        }

        currentPageIndex = pageIndex;
		currentPage = pages[currentPageIndex];

        currentPage.gameObject.SetActive(true);
        currentPageInputField.SetTextWithoutNotify($"{currentPageIndex + 1} / {pages.Count}");

		prevPageButton.interactable = currentPageIndex > 0;
		nextPageButton.interactable = currentPageIndex < pages.Count - 1;
    }
    #endregion

    #region Coroutines
    #endregion
}