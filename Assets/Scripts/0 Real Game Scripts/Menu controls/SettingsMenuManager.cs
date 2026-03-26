using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;


public class SettingsMenuManager : MonoBehaviour
{
    private static SettingsMenuManager activeInstance;

    public RectTransform menuPanel;
    public Button toggleButton;
    private bool isOpen = false;
    public Button closeButton;
    public Button backButton;


    public RectTransform menuPanel2;
    public Button toggleButton2;
    private bool isOpen2 = false;
    public Button closeButton2;
    public Button backButton2;

        public RectTransform menuPanel3;
    public Button toggleButton3;
    private bool isOpen3 = false;
        public Button closeButton3;
        public Button backButton3;

    [Header("Panel 3 Sub-panels")]
    [Tooltip("Shown while the mouse is hovering inside menuPanel3.")]
    public GameObject panel3HoverContent;
    [Tooltip("Shown while the mouse is outside menuPanel3.")]
    public GameObject panel3OutsideContent;

    [Header("Panel 3 Alternate Content")]
    [Tooltip("Shown when bubble 2 is selected. Hover state is ignored in this mode.")]
    public GameObject panel3SingleContent;
    [Tooltip("Optional overlay shown on top of Single Content while hovering over menuPanel3.")]
    public GameObject panel3SingleHoverOverlay;

    [Header("Panel 3 Bubbles")]
    public Image panel3Bubble1;
    public Image panel3Bubble2;
    public Color panel3BubbleActive = Color.white;
    public Color panel3BubbleInactive = new Color(1f, 1f, 1f, 0.35f);


    public RectTransform menuPanel4;
    public Button toggleButton4;
    private bool isOpen4 = false;
    public Button closeButton4;
    public Button backButton4;

    [Header("Question Overlay")]
    public GameObject questionOverlay;
    public float questionOverlayFadeDuration = 0.15f;

    private CanvasGroup questionOverlayCanvasGroup;
    private Coroutine questionOverlayCoroutine;
    private int panel3Mode = 0; // 0 = hover/outside duo, 1 = single static image

    void Awake()
    {
        if (activeInstance != null && activeInstance != this)
        {
            enabled = false;
            return;
        }

        activeInstance = this;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!enabled)
        {
            return;
        }

        // Position the menu panels off-screen above the top using their heights
        if (menuPanel != null)
            menuPanel.anchoredPosition = new Vector2(menuPanel.anchoredPosition.x, menuPanel.rect.height);
        if (menuPanel2 != null)
            menuPanel2.anchoredPosition = new Vector2(menuPanel2.anchoredPosition.x, menuPanel2.rect.height);
        if (menuPanel3 != null)
            menuPanel3.anchoredPosition = new Vector2(menuPanel3.anchoredPosition.x, menuPanel3.rect.height);
        if (menuPanel4 != null)
            menuPanel4.anchoredPosition = new Vector2(menuPanel4.anchoredPosition.x, menuPanel4.rect.height);
        InitializeQuestionOverlay();

       
        // Add listeners to the buttons
        if (toggleButton != null) toggleButton.onClick.AddListener(ToggleMenu);
        if (toggleButton2 != null) toggleButton2.onClick.AddListener(ToggleMenu2);
        if (toggleButton3 != null) toggleButton3.onClick.AddListener(ToggleMenu3);
        if (toggleButton4 != null) toggleButton4.onClick.AddListener(ToggleMenu4);
        if (closeButton != null) closeButton.onClick.AddListener(CloseMenu);
        if (closeButton2 != null) closeButton2.onClick.AddListener(CloseMenu2);
        if (closeButton3 != null) closeButton3.onClick.AddListener(CloseMenu3);
        if (closeButton4 != null) closeButton4.onClick.AddListener(CloseMenu4);
        if (backButton != null) backButton.onClick.AddListener(CloseMenu);
        if (backButton2 != null) backButton2.onClick.AddListener(CloseMenu2);
        if (backButton3 != null) backButton3.onClick.AddListener(CloseMenu3);
        if (backButton4 != null) backButton4.onClick.AddListener(CloseMenu4);


        // Ensure initial interactability state
        // Hide panel3 sub-panels until the panel is opened
        HidePanel3Contents();
        UpdatePanel3Bubbles();
        UpdateToggleInteractability();
    }


    // Update is called once per frame
    void Update()
    {
        if (!enabled)
        {
            return;
        }

        if (isOpen3 && menuPanel3 != null)
        {
            UpdatePanel3HoverState();
        }

        UpdateToggleInteractability();
    }

    void OnDestroy()
    {
        if (activeInstance == this)
        {
            activeInstance = null;
        }
    }

    void UpdatePanel3HoverState()
    {
        if (menuPanel3 == null)
        {
            return;
        }

        HidePanel3Contents();

        Canvas canvas = menuPanel3.GetComponentInParent<Canvas>();
        Camera eventCamera = (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            ? canvas.worldCamera
            : null;

        bool mouseInside = RectTransformUtility.RectangleContainsScreenPoint(
            menuPanel3, Input.mousePosition, eventCamera);

        if (panel3Mode == 1)
        {
            if (panel3SingleContent != null)
                panel3SingleContent.SetActive(true);

            if (panel3SingleHoverOverlay != null)
                panel3SingleHoverOverlay.SetActive(mouseInside);

            return;
        }

        if (panel3HoverContent != null)
            panel3HoverContent.SetActive(mouseInside);
        if (panel3OutsideContent != null)
            panel3OutsideContent.SetActive(!mouseInside);
    }


    void ToggleMenu()
    {
        if (menuPanel == null) return;
        if (isOpen)
        {
            // Close: animate up to off-screen
            StartCoroutine(AnimatePanel(menuPanel, menuPanel.anchoredPosition.y, menuPanel.rect.height));
        }
        else
        {
            // Open: animate down to visible position
            StartCoroutine(AnimatePanel(menuPanel, menuPanel.anchoredPosition.y, 0f));
        }
        isOpen = !isOpen;
        UpdateToggleInteractability();
    }


    void CloseMenu()
    {
        if (menuPanel == null || !isOpen) return;
        StartCoroutine(AnimatePanel(menuPanel, menuPanel.anchoredPosition.y, menuPanel.rect.height));
        isOpen = false;
        UpdateToggleInteractability();
    }



    void ToggleMenu2()
    {
        if (menuPanel2 == null) return;
        if (isOpen2)
        {
            // Close: animate up to off-screen
            StartCoroutine(AnimatePanel(menuPanel2, menuPanel2.anchoredPosition.y, menuPanel2.rect.height));
        }
        else
        {
            // Open: animate down to visible position
            StartCoroutine(AnimatePanel(menuPanel2, menuPanel2.anchoredPosition.y, 0f));
        }
        isOpen2 = !isOpen2;
        UpdateToggleInteractability();
    }


    void CloseMenu2()
    {
        if (menuPanel2 == null || !isOpen2) return;
        StartCoroutine(AnimatePanel(menuPanel2, menuPanel2.anchoredPosition.y, menuPanel2.rect.height));
        isOpen2 = false;
        UpdateToggleInteractability();
    }


    void ToggleMenu3()
    {
        if (menuPanel3 == null) return;
        if (isOpen3)
        {
            // Close: animate up to off-screen
            StartCoroutine(AnimatePanel(menuPanel3, menuPanel3.anchoredPosition.y, menuPanel3.rect.height));
        }
        else
        {
            // Open: animate down to visible position
            StartCoroutine(AnimatePanel(menuPanel3, menuPanel3.anchoredPosition.y, 0f));
        }
        isOpen3 = !isOpen3;

        if (isOpen3)
        {
            UpdatePanel3HoverState();
        }
        else
        {
            HidePanel3Contents();
        }

        UpdateToggleInteractability();
    }


    void CloseMenu3()
    {
        if (menuPanel3 == null || !isOpen3) return;
        StartCoroutine(AnimatePanel(menuPanel3, menuPanel3.anchoredPosition.y, menuPanel3.rect.height));
        isOpen3 = false;
        HidePanel3Contents();
        UpdateToggleInteractability();
    }

    // Bubble 1: use the original hover/outside duo
    public void SelectPanel3Bubble1()
    {
        panel3Mode = 0;
        UpdatePanel3Bubbles();

        if (isOpen3)
        {
            UpdatePanel3HoverState();
        }
    }

    // Bubble 2: show one static image/content panel
    public void SelectPanel3Bubble2()
    {
        panel3Mode = 1;
        UpdatePanel3Bubbles();

        if (isOpen3)
        {
            UpdatePanel3HoverState();
        }
    }

    void HidePanel3Contents()
    {
        if (panel3HoverContent != null) panel3HoverContent.SetActive(false);
        if (panel3OutsideContent != null) panel3OutsideContent.SetActive(false);
        if (panel3SingleContent != null) panel3SingleContent.SetActive(false);
        if (panel3SingleHoverOverlay != null) panel3SingleHoverOverlay.SetActive(false);
    }

    void UpdatePanel3Bubbles()
    {
        if (panel3Bubble1 != null)
            panel3Bubble1.color = panel3Mode == 0 ? panel3BubbleActive : panel3BubbleInactive;

        if (panel3Bubble2 != null)
            panel3Bubble2.color = panel3Mode == 1 ? panel3BubbleActive : panel3BubbleInactive;
    }


    void ToggleMenu4()
    {
        if (questionOverlay != null)
        {
            SetQuestionOverlayVisible(!isOpen4);
        }
        else
        {
            if (menuPanel4 == null) return;
            if (isOpen4)
            {
                // Close: animate up to off-screen
                StartCoroutine(AnimatePanel(menuPanel4, menuPanel4.anchoredPosition.y, menuPanel4.rect.height));
            }
            else
            {
                // Open: animate down to visible position
                StartCoroutine(AnimatePanel(menuPanel4, menuPanel4.anchoredPosition.y, 0f));
            }

            isOpen4 = !isOpen4;
        }

        UpdateToggleInteractability();
    }


    void CloseMenu4()
    {
        if (questionOverlay != null)
        {
            if (!isOpen4) return;
            SetQuestionOverlayVisible(false);
        }
        else
        {
            if (menuPanel4 == null || !isOpen4) return;
            StartCoroutine(AnimatePanel(menuPanel4, menuPanel4.anchoredPosition.y, menuPanel4.rect.height));
            isOpen4 = false;
        }

        UpdateToggleInteractability();
    }


    void UpdateToggleInteractability()

    {
        // Determine open state based on panel active state and position
        bool pauseOpen = menuPanel != null && menuPanel.gameObject.activeInHierarchy && Mathf.Approximately(menuPanel.anchoredPosition.y, 0f);
        bool mapOpen = menuPanel2 != null && menuPanel2.gameObject.activeInHierarchy && Mathf.Approximately(menuPanel2.anchoredPosition.y, 0f);
        bool questionOpen = menuPanel3 != null && menuPanel3.gameObject.activeInHierarchy && Mathf.Approximately(menuPanel3.anchoredPosition.y, 0f);
        bool panel4Open = questionOverlay != null
            ? isOpen4
            : menuPanel4 != null && menuPanel4.gameObject.activeInHierarchy && Mathf.Approximately(menuPanel4.anchoredPosition.y, 0f);

        // Menu button (on pause panel) should always be active when visible
        if (toggleButton != null)
        {
            toggleButton.interactable = true;
        }

        // Other buttons: active when their panel is open (to close it) or when no panels are open
        if (toggleButton2 != null)
        {
            toggleButton2.interactable = mapOpen || (!pauseOpen && !questionOpen && !panel4Open);
        }

        if (toggleButton3 != null)
        {
            toggleButton3.interactable = questionOpen || (!pauseOpen && !mapOpen && !panel4Open);
        }

        if (toggleButton4 != null)
        {
            toggleButton4.interactable = panel4Open || (!pauseOpen && !mapOpen && !questionOpen);
        }
    }

    void SetQuestionOverlayVisible(bool visible)
    {
        isOpen4 = visible;

        if (questionOverlay == null)
        {
            return;
        }

        if (questionOverlayCoroutine != null)
        {
            StopCoroutine(questionOverlayCoroutine);
        }

        questionOverlayCoroutine = StartCoroutine(FadeQuestionOverlay(visible));
    }

    IEnumerator FadeQuestionOverlay(bool visible)
    {
        if (questionOverlay == null)
        {
            yield break;
        }

        if (questionOverlayCanvasGroup == null)
        {
            questionOverlay.SetActive(visible);
            questionOverlayCoroutine = null;
            yield break;
        }

        float duration = Mathf.Max(0f, questionOverlayFadeDuration);
        float startAlpha = questionOverlayCanvasGroup.alpha;
        float targetAlpha = visible ? 1f : 0f;

        if (visible)
        {
            questionOverlay.SetActive(true);
        }

        questionOverlayCanvasGroup.interactable = visible;
        questionOverlayCanvasGroup.blocksRaycasts = visible;

        if (duration <= 0f)
        {
            questionOverlayCanvasGroup.alpha = targetAlpha;
        }
        else
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                questionOverlayCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
                yield return null;
            }

            questionOverlayCanvasGroup.alpha = targetAlpha;
        }

        if (!visible)
        {
            questionOverlayCanvasGroup.interactable = false;
            questionOverlayCanvasGroup.blocksRaycasts = false;
            questionOverlay.SetActive(false);
        }

        questionOverlayCoroutine = null;
    }

    void InitializeQuestionOverlay()
    {
        if (questionOverlay == null)
        {
            return;
        }

        questionOverlayCanvasGroup = questionOverlay.GetComponent<CanvasGroup>();

        if (questionOverlayCanvasGroup == null && questionOverlayFadeDuration > 0f)
        {
            questionOverlayCanvasGroup = questionOverlay.AddComponent<CanvasGroup>();
        }

        if (questionOverlayCanvasGroup != null)
        {
            questionOverlayCanvasGroup.alpha = 0f;
            questionOverlayCanvasGroup.interactable = false;
            questionOverlayCanvasGroup.blocksRaycasts = false;
        }

        questionOverlay.SetActive(false);
    }



    IEnumerator AnimatePanel(RectTransform panel, float startY, float endY)
    {
        float duration = 0.5f; // Adjust animation speed as needed
        float elapsed = 0f;
        Vector2 pos = panel.anchoredPosition;


        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Use easing for smoother animation (optional)
            t = t * t * (3f - 2f * t); // Smooth step
            pos.y = Mathf.Lerp(startY, endY, t);
            panel.anchoredPosition = pos;
            yield return null;
        }


        // Ensure final position
        pos.y = endY;
        panel.anchoredPosition = pos;
    }
}
