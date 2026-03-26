using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

/* YOUR SOUND MANAGER LINE ARE COMMENTED OUT FOR TESTING!! REACTIVATE THEM TO MAKE SOUND WORK*/

public class MiniGame1Dialogue : MonoBehaviour
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
    public Button continueButton;
    [Header("End Buttons")]
    public Button preActivationEndButton;
    public Button activatedEndButton;
    public Button postActivationEndButton;

    [Header("Dialogue Data")]
    public string speakerName;
    public string[] inactiveLines;
    public string[] activeLines;
    public string[] repeatLines;

    [Header("Typing Settings")]
    public float textSpeed = 0.08f;
    public float punctuationPause = 0.3f;

    [Header("Interaction")]
    public Renderer highlightRenderer;
    public Color hoverColor = Color.yellow;
    private Color originalColor;

    private bool playerInRange = false;

    [Header("Statue State")]
    public StatueActivationBase statueState;


    private int index = 0;
    private bool isTypingName = false;
    private bool isTypingLine = false;
    private bool dialogueActive = false;

    private Coroutine typingCoroutine;
    private string[] currentLines;

    private float triggerCooldown = 5f;
    private float lastTriggerTime = -1f;
    private bool canTrigger = true;

    void Start()
    {
        dialoguePanel.SetActive(false);

        continueButton.onClick.AddListener(HandleInput);
        preActivationEndButton.onClick.AddListener(ClosePanel);
        activatedEndButton.onClick.AddListener(ClosePanel);
        postActivationEndButton.onClick.AddListener(ClosePanel);

        if (highlightRenderer != null)
            originalColor = highlightRenderer.material.color;

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
            bool isLastLine = index >= currentLines.Length - 1;

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
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;

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
        
    //AudioManager.Instance.PlayPanelOpen(); // Play panel open sound via manager

    if (dialogueActive) return;

    dialoguePanel.SetActive(true);
    dialogueActive = true;

    if (playerController != null)
        playerController.enabled = false;

    characterName.text = "";
    textComponent.text = "";

    if (characterPortrait != null)
        characterPortrait.SetActive(false);

    continueButton.gameObject.SetActive(false);
    preActivationEndButton.gameObject.SetActive(false);
    activatedEndButton.gameObject.SetActive(false);
    postActivationEndButton.gameObject.SetActive(false);

    switch (statueState.currentState)
    {
        case StatueActivationBase.StatueState.PreActivation:
            currentLines = inactiveLines;
            break;
        case StatueActivationBase.StatueState.Activated:
            currentLines = activeLines;
            break;
        case StatueActivationBase.StatueState.PostActivation:
            currentLines = repeatLines;
            break;
    }


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
            portraitRect.anchoredPosition = Vector2.Lerp(topPosition, portraitStartPos, progress);
            yield return null;
        }

        Vector2 bouncePos = portraitStartPos - Vector2.up * portraitBounceHeight;
        t = 0;
        while (t < portraitBounceTime)
        {
            t += Time.deltaTime;
            float progress = t / portraitBounceTime;
            portraitRect.anchoredPosition = Vector2.Lerp(portraitStartPos, bouncePos, progress);
            yield return null;
        }

        t = 0;
        while (t < portraitBounceTime)
        {
            t += Time.deltaTime;
            float progress = t / portraitBounceTime;
            portraitRect.anchoredPosition = Vector2.Lerp(bouncePos, portraitStartPos, progress);
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

        
        //AudioManager.Instance.StartSpeaking(); // Start speaking sound via manager

        foreach (char c in currentLines[index])
        {
            textComponent.text += c;

            float waitTime = textSpeed;
            if (c == '.' || c == '!' || c == '?') waitTime = punctuationPause;
            else if (c == ',') waitTime = punctuationPause * 0.5f;

            yield return new WaitForSeconds(waitTime);
        }

        
        //AudioManager.Instance.StopSpeaking();// Stop speaking sound via manager

        isTypingLine = false;

        bool isLastLine = index >= currentLines.Length - 1;
        continueButton.gameObject.SetActive(!isLastLine);

        if (isLastLine)
        {
            ShowEndButton();
        }

        if (isLastLine)
        {
            switch (statueState.currentState)
            {
                case StatueActivationBase.StatueState.PreActivation:
                    preActivationEndButton?.gameObject.SetActive(true);
                    break;
                case StatueActivationBase.StatueState.Activated:
                    activatedEndButton?.gameObject.SetActive(true);
                    break;
                case StatueActivationBase.StatueState.PostActivation:
                    postActivationEndButton?.gameObject.SetActive(true);
                    break;
            }
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
            textComponent.text = currentLines[index];
            isTypingLine = false;

            
            //AudioManager.Instance.StopSpeaking();// Stop speaking if interrupted

            if (index >= currentLines.Length - 1)
            {
                ShowEndButton();
            }

            return;
        }

        if (index < currentLines.Length - 1)
            NextLine();
    }

    void NextLine()
    {
        index++;

        continueButton.gameObject.SetActive(false);
        preActivationEndButton.gameObject.SetActive(false);
        activatedEndButton.gameObject.SetActive(false);
        postActivationEndButton.gameObject.SetActive(false);

        typingCoroutine = StartCoroutine(TypeLine());
    }

    void ShowEndButton()
    {
        preActivationEndButton?.gameObject.SetActive(false);
        activatedEndButton?.gameObject.SetActive(false);
        postActivationEndButton?.gameObject.SetActive(false);

        switch (statueState.currentState)
        {
            case StatueActivationBase.StatueState.PreActivation:
                preActivationEndButton?.gameObject.SetActive(true);
                break;
            case StatueActivationBase.StatueState.Activated:
                activatedEndButton?.gameObject.SetActive(true);
                break;
            case StatueActivationBase.StatueState.PostActivation:
                postActivationEndButton?.gameObject.SetActive(true);
                break;
        }
    }

    void ClosePanel()
    {
        if (statueState.currentState == StatueActivationBase.StatueState.Activated)
        statueState.DeactivateStatue();

        if (playerController != null)
            playerController.enabled = true;

        characterName.text = "";
        textComponent.text = "";
        continueButton.gameObject.SetActive(false);
        preActivationEndButton.gameObject.SetActive(false);
        activatedEndButton.gameObject.SetActive(false);
        postActivationEndButton.gameObject.SetActive(false);

        dialoguePanel.SetActive(false);
        dialogueActive = false;

        lastTriggerTime = Time.time; 
        canTrigger = false;

        if (characterPortrait != null)
            characterPortrait.SetActive(false);

        Debug.Log ("closing worked");

        //AudioManager.Instance.PlayPanelClose(); // panel close sound
        //AudioManager.Instance.StopSpeaking();    // stop typing sound
    }

}