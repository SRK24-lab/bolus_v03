using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TabData
{
    public RectTransform tabRoot;          // the tab RectTransform (same as tabs[] entry)
    public GameObject contentRoot;         // optional: the area to hide when this tab is not active
    public GameObject[] contentPanels;    // the sub-panels inside this tab
    public Image[] bubbles;               // dot/bubble indicator images, one per panel

    [HideInInspector] public int currentPanel = 0;
}

public class TP_Tabs : MonoBehaviour
{
    [Header("Main Panel Movement")]
    public RectTransform mainPanel;
    public RectTransform clickBounds;
    public Vector2 closedPos;
    public Vector2 openPos;

    [Header("Tabs")]
    public TabData[] tabs;

    [Header("Bubble Colours")]
    public Color bubbleActive   = Color.white;
    public Color bubbleInactive = new Color(1f, 1f, 1f, 0.35f);

    private bool isOpen       = false;
    private int  activeTabIdx = -1;
    private bool ignoreOutsideClickUntilRelease = false;

    private void Awake()
    {
        if (clickBounds == null)
        {
            clickBounds = mainPanel;
        }

        for (int i = 0; i < tabs.Length; i++)
        {
            SetTabContentVisible(i, false);

            TabData tab = tabs[i];
            if (tab.contentPanels != null && tab.contentPanels.Length > 0)
            {
                tab.currentPanel = Mathf.Clamp(tab.currentPanel, 0, tab.contentPanels.Length - 1);
                UpdateBubbles(tab, tab.currentPanel);
            }
        }
    }

    private void Update()
    {
        if (!isOpen || clickBounds == null)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            ignoreOutsideClickUntilRelease = false;
        }

        if (ignoreOutsideClickUntilRelease)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0) && !IsScreenPointInside(clickBounds, Input.mousePosition))
        {
            ClosePanel();
        }
    }

    // ── Tab opening ──────────────────────────────────────────────────────────

    public void OpenTab(int index)
    {
        if (index < 0 || index >= tabs.Length)
        {
            return;
        }

        if (!isOpen)
        {
            // SFX trigger point (slide down/open starts): play your tab-open sound here.
            AudioManager.Instance.PlayPanelOpen();
            StartCoroutine(MovePanel(openPos));
            isOpen = true;
        }

        ignoreOutsideClickUntilRelease = true;

        for (int i = 0; i < tabs.Length; i++)
        {
            SetTabContentVisible(i, i == index);
        }

        if (tabs[index].tabRoot != null)
        {
            tabs[index].tabRoot.SetAsLastSibling();
        }

        activeTabIdx = index;

        // Restore the last-viewed panel in this tab
        ShowPanel(index, tabs[index].currentPanel);
    }

    public void OpenTabPanel(int tabIndex, int panelIndex)
    {
        OpenTab(tabIndex);
        ShowPanel(tabIndex, panelIndex);
    }

    public void ClosePanel()
    {
        // SFX trigger point (slide up/close starts): play your tab-close sound here.
        AudioManager.Instance.PlayPanelClose();
        StartCoroutine(MovePanel(closedPos));
        isOpen = false;
        ignoreOutsideClickUntilRelease = false;
    }

    // ── Panel switching ──────────────────────────────────────────────────────

    /// <summary>
    /// Switch to a specific panel inside the currently active tab.
    /// Wire bubble buttons to this via: OpenTab sets activeTabIdx first,
    /// then each bubble calls SwitchPanel(panelIndex).
    /// </summary>
    public void SwitchPanel(int panelIndex)
    {
        if (activeTabIdx < 0) return;
        ShowPanel(activeTabIdx, panelIndex);
    }

    /// <summary>
    /// Show panel <panelIndex> inside tab <tabIndex>, hide siblings, update bubbles.
    /// </summary>
    public void ShowPanel(int tabIndex, int panelIndex)
    {
        if (tabIndex < 0 || tabIndex >= tabs.Length)
        {
            return;
        }

        TabData tab = tabs[tabIndex];

        if (tab.contentPanels == null || tab.contentPanels.Length == 0)
        {
            return;
        }

        panelIndex = Mathf.Clamp(panelIndex, 0, tab.contentPanels.Length - 1);
        tab.currentPanel = panelIndex;
        SetTabContentVisible(tabIndex, true);

        for (int i = 0; i < tab.contentPanels.Length; i++)
        {
            if (tab.contentPanels[i] != null)
                tab.contentPanels[i].SetActive(i == panelIndex);
        }

        UpdateBubbles(tab, panelIndex);
    }

    private void SetTabContentVisible(int tabIndex, bool isVisible)
    {
        if (tabIndex < 0 || tabIndex >= tabs.Length)
        {
            return;
        }

        TabData tab = tabs[tabIndex];

        if (tab.contentRoot != null)
        {
            tab.contentRoot.SetActive(isVisible);
        }

        if (!isVisible && tab.contentPanels != null)
        {
            for (int i = 0; i < tab.contentPanels.Length; i++)
            {
                if (tab.contentPanels[i] != null)
                {
                    tab.contentPanels[i].SetActive(false);
                }
            }
        }
    }

    private void UpdateBubbles(TabData tab, int panelIndex)
    {
        for (int i = 0; i < tab.bubbles.Length; i++)
        {
            if (tab.bubbles[i] != null)
                tab.bubbles[i].color = (i == panelIndex) ? bubbleActive : bubbleInactive;
        }
    }

    private bool IsScreenPointInside(RectTransform rectTransform, Vector2 screenPoint)
    {
        Canvas canvas = rectTransform.GetComponentInParent<Canvas>();
        Camera eventCamera = null;

        if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            eventCamera = canvas.worldCamera;
        }

        return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, screenPoint, eventCamera);
    }

    // ── Slide animation ──────────────────────────────────────────────────────

    System.Collections.IEnumerator MovePanel(Vector2 target)
    {
        float time  = 0f;
        Vector2 start = mainPanel.anchoredPosition;

        while (time < 0.25f)
        {
            time += Time.deltaTime;
            mainPanel.anchoredPosition = Vector2.Lerp(start, target, time / 0.25f);
            yield return null;
        }

        mainPanel.anchoredPosition = target;
    }
}