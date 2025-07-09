using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class BookManager : MonoBehaviour
{
    [Header("Canvas Panels")]
    public GameObject compilerCanvas; // "cod"
    public GameObject bookCanvas;     // "carte"

    [Header("Text Components")]
    public TextMeshProUGUI leftPageText;
    public TextMeshProUGUI rightPageText;

    [Header("Buttons")]
    public Button openBookButton;   // <-- The button from the compiler canvas
    public Button nextButton;
    public Button prevButton;
    public Button closeButton;

    [Header("Pages Content")]
    [TextArea(3, 10)]
    public List<string> pages;

    private int currentPage = 0;

    void Start()
    {
        // Hook up button events
        if (openBookButton != null)
            openBookButton.onClick.AddListener(OpenBook);

        nextButton.onClick.AddListener(NextPage);
        prevButton.onClick.AddListener(PreviousPage);
        closeButton.onClick.AddListener(CloseBook);

        bookCanvas.SetActive(false); // Start hidden
    }

    public void OpenBook()
    {
        compilerCanvas.SetActive(false);
        bookCanvas.SetActive(true);
        ShowPages();
    }

    public void CloseBook()
    {
        bookCanvas.SetActive(false);
        compilerCanvas.SetActive(true);
    }

    private void ShowPages()
    {
        leftPageText.text = (currentPage < pages.Count) ? pages[currentPage] : "";
        rightPageText.text = (currentPage + 1 < pages.Count) ? pages[currentPage + 1] : "";

        prevButton.interactable = currentPage > 0;
        nextButton.interactable = currentPage + 2 < pages.Count;
    }

    private void NextPage()
    {
        if (currentPage + 2 < pages.Count)
        {
            currentPage += 2;
            ShowPages();
        }
    }

    private void PreviousPage()
    {
        if (currentPage - 2 >= 0)
        {
            currentPage -= 2;
            ShowPages();
        }
    }
}
