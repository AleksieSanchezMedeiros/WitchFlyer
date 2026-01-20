using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuidePanel : MonoBehaviour
{
    [Header("Navigation")]
    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button closeButton;

    [Header("Pages (each is a panel GameObject)")]
    [SerializeField] private List<GameObject> guidePages = new();

    private int currentIndex;

    private void Awake()
    {
        prevButton.onClick.AddListener(ShowPrevious);
        nextButton.onClick.AddListener(ShowNext);
        closeButton.onClick.AddListener(ClosePanel);
    }

    private void OnEnable()
    {
        currentIndex = Mathf.Clamp(currentIndex, 0, guidePages.Count - 1);
        UpdatePages();
    }

    public void OpenPanel(int startIndex = 0)
    {
        currentIndex = Mathf.Clamp(startIndex, 0, guidePages.Count - 1);
        gameObject.SetActive(true);
        UpdatePages();
    }

    public void ClosePanel() => gameObject.SetActive(false);

    public void ShowPrevious()
    {
        if (currentIndex <= 0) return;
        currentIndex--;
        UpdatePages();
    }

    public void ShowNext()
    {
        if (currentIndex >= guidePages.Count - 1) return;
        currentIndex++;
        UpdatePages();
    }

    private void UpdatePages()
    {
        if (guidePages == null || guidePages.Count == 0) {
            prevButton.interactable = false;
            nextButton.interactable = false;
            return;
        }

        for (int i = 0; i < guidePages.Count; i++) {
            if (guidePages[i] != null) guidePages[i].SetActive(i == currentIndex);
        }

        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < guidePages.Count - 1;
    }
}
