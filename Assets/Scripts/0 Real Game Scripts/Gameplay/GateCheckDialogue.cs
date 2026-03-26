using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class GateCheckDialogue : MonoBehaviour
{
    [Header("Player Control")]
    public MonoBehaviour playerController;

    [Header("Disable While Dialogue Active")]
    public MonoBehaviour[] colliderInteractionScripts;

    [Header("UI Panel")]
    public GameObject dialoguePanel;

    [Header("Dialogue Text")]
    public TMP_Text dialogueText;
    public TMP_Text characterNameText;

    [Header("Dialogue Content")]
    public string characterName;

    [TextArea(2,5)]
    public string page1Text;

    [TextArea(2,5)]
    public string page3Text;

    [Header("Buttons")]
    public Button continueButton;
    public Button backButton;
    public Button leaveButton;

    [Header("Image Drop Page")]
    public RectTransform image1;
    public RectTransform image2;
    public RectTransform image3;

    public float dropDistance = 200f;
    public float dropTime = 0.25f;
    public float delayBetweenDrops = 0.2f;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;
    public float punctuationPause = 0.2f;

    private int pageIndex;
    private bool dialogueActive = false;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private Vector2 img1Start;
    private Vector2 img2Start;
    private Vector2 img3Start;

    void Start()
    {
        dialoguePanel.SetActive(false);

        backButton.gameObject.SetActive(false);
        leaveButton.gameObject.SetActive(false);

        if (continueButton != null)
            continueButton.onClick.AddListener(HandleInput);

        img1Start = image1.anchoredPosition;
        img2Start = image2.anchoredPosition;
        img3Start = image3.anchoredPosition;

        image1.gameObject.SetActive(false);
        image2.gameObject.SetActive(false);
        image3.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!dialogueActive)
            return;

        // Ignore keyboard entirely while panel is open
        return;
    }

    public void OpenPanel()
    {
        dialoguePanel.SetActive(true);
        dialogueActive = true;
        pageIndex = 0;

        // Disable keyboard globally
        if (Keyboard.current != null)
            InputSystem.DisableDevice(Keyboard.current);

        characterNameText.text = characterName;

        if (playerController != null)
            playerController.enabled = false;

        foreach (MonoBehaviour script in colliderInteractionScripts)
        {
            if (script != null)
                script.enabled = false;
        }

        ShowPage();
    }

    void ShowPage()
    {
        dialogueText.text = "";

        // Hide images when leaving page 2
        image1.gameObject.SetActive(false);
        image2.gameObject.SetActive(false);
        image3.gameObject.SetActive(false);

        if (pageIndex == 0)
        {
            continueButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
            leaveButton.gameObject.SetActive(false);

            StartTyping(page1Text);
        }

        else if (pageIndex == 1)
        {
            continueButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
            leaveButton.gameObject.SetActive(false);

            StartCoroutine(PlayImageDropSequence());
        }

        else if (pageIndex == 2)
        {
            continueButton.gameObject.SetActive(false);
            backButton.gameObject.SetActive(true);
            leaveButton.gameObject.SetActive(true);

            StartTyping(page3Text);
        }
    }

    void StartTyping(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeLine(text));
    }

    IEnumerator TypeLine(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;

            if (c == '.' || c == ',' || c == '!' || c == '?')
                yield return new WaitForSeconds(punctuationPause);
            else
                yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    IEnumerator PlayImageDropSequence()
    {
        dialogueText.text = "";

        yield return DropImage(image1, img1Start);
        yield return new WaitForSeconds(delayBetweenDrops);

        yield return DropImage(image2, img2Start);
        yield return new WaitForSeconds(delayBetweenDrops);

        yield return DropImage(image3, img3Start);
    }

    IEnumerator DropImage(RectTransform img, Vector2 target)
    {
        img.gameObject.SetActive(true);

        Vector2 start = target + Vector2.up * dropDistance;
        img.anchoredPosition = start;

        float t = 0;

        while (t < dropTime)
        {
            t += Time.deltaTime;
            float progress = t / dropTime;

            img.anchoredPosition = Vector2.Lerp(start, target, progress);

            yield return null;
        }

        img.anchoredPosition = target;
    }

    void HandleInput()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            isTyping = false;
            return;
        }

        if (pageIndex < 2)
        {
            pageIndex++;
            ShowPage();
        }
    }

    public void ClosePanel()
    {
        dialoguePanel.SetActive(false);
        dialogueActive = false;

        // Re-enable keyboard
        if (Keyboard.current != null)
            InputSystem.EnableDevice(Keyboard.current);

        if (playerController != null)
            playerController.enabled = true;

        foreach (MonoBehaviour script in colliderInteractionScripts)
        {
            if (script != null)
                script.enabled = true;
        }
    }
}