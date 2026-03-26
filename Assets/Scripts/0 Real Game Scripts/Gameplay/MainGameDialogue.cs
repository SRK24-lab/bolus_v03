using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class MainGameDialogue : MonoBehaviour
{
    [Header("Player Control")]
    public MonoBehaviour playerController;

    [Header("UI Panel")]
    public GameObject dialoguePanel;

    [Header("Portrait")]
    public GameObject characterPortrait;
    public float portraitDropDistance = 200f;
    public float portraitDropTime = 0.25f;
    public float portraitBounceHeight = 20f;
    public float portraitBounceTime = 0.1f;

    private RectTransform portraitRect;
    private Vector2 portraitStartPos;

    [Header("UI References")]
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI textComponent;

    [Header("Buttons")]
    public Button continueButton;
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;

    [Header("Dialogue Data")]
    public string speakerName;
    public string[] dialogueLines;

    [Header("Typing Settings")]
    public float textSpeed = 0.08f;
    public float punctuationPause = 0.3f;

    [Header("Interaction Highlight")]
    public GameObject highlightObject;
    public Color hoverColor = Color.yellow;

    private Renderer highlightRenderer;
    private Color originalColor;

    private int index = 0;
    private bool isTypingName = false;
    private bool isTypingLine = false;
    private bool dialogueActive = false;

    private bool playerInRange = false;

    private Coroutine typingCoroutine;

    private float triggerCooldown = 5f;
    private float lastTriggerTime = -1f;
    private bool canTrigger = true;

    void Start()
    {
        dialoguePanel.SetActive(false);

        if (highlightObject != null)
        {
            highlightRenderer = highlightObject.GetComponent<Renderer>();

            if (highlightRenderer != null)
                originalColor = highlightRenderer.material.color;
        }

        if (continueButton != null)
            continueButton.onClick.AddListener(HandleInput);

        if (button1 != null)
            button1.onClick.AddListener(ClosePanel);

        if (button2 != null)
            button2.onClick.AddListener(ClosePanel);

        if (button3 != null)
            button3.onClick.AddListener(ClosePanel);

        if (button4 != null)
            button4.onClick.AddListener(ClosePanel);

        if (characterPortrait != null)
        {
            portraitRect = characterPortrait.GetComponent<RectTransform>();
            portraitStartPos = portraitRect.anchoredPosition;
            characterPortrait.SetActive(false);
        }
    }

    void Update()
    {
        var keyboard = Keyboard.current;

        if (!dialogueActive && playerInRange)
        {
            bool pressedKey =
                keyboard.spaceKey.wasPressedThisFrame ||
                keyboard.enterKey.wasPressedThisFrame ||
                keyboard.numpadEnterKey.wasPressedThisFrame;

            if (pressedKey)
            {
                OpenPanel();
                return;
            }
        }

        if (!dialogueActive) return;

        if (keyboard != null &&
            (keyboard.spaceKey.wasPressedThisFrame ||
             keyboard.enterKey.wasPressedThisFrame ||
             keyboard.numpadEnterKey.wasPressedThisFrame))
        {
            bool isLastLine = index >= dialogueLines.Length - 1;

            if (isTypingName || isTypingLine || !isLastLine)
            {
                HandleInput();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;

        if (highlightRenderer != null)
            highlightRenderer.material.color = hoverColor;

        Debug.Log ("collision detected");
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = false;
        canTrigger = true;

        if (highlightRenderer != null)
            highlightRenderer.material.color = originalColor;
    }

    void OnMouseDown()
    {
        if (!playerInRange || dialogueActive) return;

        OpenPanel();
    }

    public void OpenPanel()
    {
        if (dialogueActive) return;

        dialoguePanel.SetActive(true);
        dialogueActive = true;

        if (playerController != null)
            playerController.enabled = false;

        characterName.text = "";
        textComponent.text = "";

        if (characterPortrait != null)
            characterPortrait.SetActive(false);

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        if (button1 != null)
            button1.gameObject.SetActive(false);

        if (button2 != null)
            button2.gameObject.SetActive(false);

        if (button3 != null)
            button3.gameObject.SetActive(false);

        if (button4 != null)
            button4.gameObject.SetActive(false);

        index = 0;

        typingCoroutine = StartCoroutine(TypeCharacterName());
    }

    IEnumerator DropPortrait()
    {
        Vector2 topPosition = portraitStartPos + Vector2.up * portraitDropDistance;
        portraitRect.anchoredPosition = topPosition;

        float t = 0;

        while (t < portraitDropTime)
        {
            t += Time.deltaTime;
            float progress = t / portraitDropTime;
            portraitRect.anchoredPosition =
                Vector2.Lerp(topPosition, portraitStartPos, progress);
            yield return null;
        }

        Vector2 bouncePos = portraitStartPos - Vector2.up * portraitBounceHeight;

        t = 0;

        while (t < portraitBounceTime)
        {
            t += Time.deltaTime;
            float progress = t / portraitBounceTime;
            portraitRect.anchoredPosition =
                Vector2.Lerp(portraitStartPos, bouncePos, progress);
            yield return null;
        }

        t = 0;

        while (t < portraitBounceTime)
        {
            t += Time.deltaTime;
            float progress = t / portraitBounceTime;
            portraitRect.anchoredPosition =
                Vector2.Lerp(bouncePos, portraitStartPos, progress);
            yield return null;
        }

        portraitRect.anchoredPosition = portraitStartPos;
    }

    IEnumerator TypeCharacterName()
    {
        isTypingName = true;

        if (characterPortrait != null)
        {
            characterPortrait.SetActive(true);
            StartCoroutine(DropPortrait());
        }

        foreach (char c in speakerName)
        {
            characterName.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTypingName = false;

        StartDialogue();
    }

    void StartDialogue()
    {
        textComponent.text = "";

        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTypingLine = true;

        textComponent.text = "";

        foreach (char c in dialogueLines[index])
        {
            textComponent.text += c;

            float waitTime = textSpeed;

            if (c == '.' || c == '!' || c == '?')
                waitTime = punctuationPause;
            else if (c == ',')
                waitTime = punctuationPause * 0.5f;

            yield return new WaitForSeconds(waitTime);
        }

        isTypingLine = false;

        bool isLastLine = index >= dialogueLines.Length - 1;


        if (continueButton != null)
            continueButton.gameObject.SetActive(!isLastLine);

        if (isLastLine)
        {
            if (button1 != null) button1.gameObject.SetActive(true);
            if (button2 != null) button2.gameObject.SetActive(true);
            if (button3 != null) button3.gameObject.SetActive(true);
            if (button4 != null) button4.gameObject.SetActive(true);
        }
    }

    void HandleInput()
    {
        if (isTypingName)
        {
            StopCoroutine(typingCoroutine);

            characterName.text = speakerName;

            isTypingName = false;

            typingCoroutine = StartCoroutine(TypeLine());

            return;
        }

        if (isTypingLine)
        {
            StopCoroutine(typingCoroutine);

            textComponent.text = dialogueLines[index];

            isTypingLine = false;

            bool isLastLine = index >= dialogueLines.Length - 1;

        if (continueButton != null)
            continueButton.gameObject.SetActive(!isLastLine);

        if (isLastLine)
        {
            if (button1 != null) button1.gameObject.SetActive(true);
            if (button2 != null) button2.gameObject.SetActive(true);
            if (button3 != null) button3.gameObject.SetActive(true);
            if (button4 != null) button4.gameObject.SetActive(true);
        }

            return;
        }

        if (index < dialogueLines.Length - 1)
        {
            NextLine();
        }
    }

    void NextLine()
    {
        index++;

        continueButton.gameObject.SetActive(false);

        typingCoroutine = StartCoroutine(TypeLine());
    }

    public void ClosePanel()
    {
        if (playerController != null)
            playerController.enabled = true;

        characterName.text = "";
        textComponent.text = "";

        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        if (button1 != null)
            button1.gameObject.SetActive(false);

        if (button2 != null)
            button2.gameObject.SetActive(false);

        if (button3 != null)
            button3.gameObject.SetActive(false);

        if (button4 != null)
            button4.gameObject.SetActive(false);

        dialoguePanel.SetActive(false);
        dialogueActive = false;

        lastTriggerTime = Time.time;
        canTrigger = false;

        if (characterPortrait != null)
            characterPortrait.SetActive(false);
    }
}