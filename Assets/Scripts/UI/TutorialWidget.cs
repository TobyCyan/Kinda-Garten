using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWidget : ScreenWidget
{
    [SerializeField] private List<TutorialPage> pages = new();
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;

    private int currentPageIndex;

    private void Start()
    {
        currentPageIndex = 0;

        nextButton.onClick.AddListener(OnNextClick);
        backButton.onClick.AddListener(OnBackClick);

        // Hide all pages except the first
        for (int i = 0; i < pages.Count; i++)
        {
            if (i == currentPageIndex)
                pages[i].ShowGroup();
            else
                pages[i].HideGroup();
        }

        UpdateButtons();
    }

    public override void OnButtonClick()
    {

    }

    protected override void OnCloseScreen()
    {
    }

    protected override void OnOpenScreen()
    {
    }

    private void OnNextClick()
    {
        // Last page -> Finish
        if (currentPageIndex == pages.Count - 1)
        {
            FinishTutorial();
            return;
        }

        pages[currentPageIndex].HideGroup();
        currentPageIndex++;
        pages[currentPageIndex].ShowGroup();

        UpdateButtons();
    }

    private void OnBackClick()
    {
        if (currentPageIndex == 0)
            return;

        pages[currentPageIndex].HideGroup();
        currentPageIndex--;
        pages[currentPageIndex].ShowGroup();

        UpdateButtons();
    }

    private void UpdateButtons()
    {
        // Back button only visible after first page
        backButton.gameObject.SetActive(currentPageIndex > 0);

        // Change text on last page
        TextMeshProUGUI buttonText = nextButton.GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText != null)
        {
            buttonText.text = currentPageIndex == pages.Count - 1
                ? "Finish"
                : "Next";
        }
    }

    private void FinishTutorial()
    {
        CloseScreen();
    }
}