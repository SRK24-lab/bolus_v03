using System.Collections;
using UnityEngine;
using TMPro;

public class GameUIPanelManager : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;
    public TMP_Text panelText;
    public CanvasGroup canvasGroup;

    [Header("Player Control")]
    public MonoBehaviour playerController;

    [Header("Special Effects")]
    public GameObject redOverlay;

    [Header("Fade Settings")]
    public float fadeDuration = 0.75f;
    public float displayTime = 2.75f;

    [Header("Stage Messages")]
    public StageMessage[] stageMessages;





    Coroutine currentRoutine;

    void Start()
    {
        panel.SetActive(false);

        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void OnEnable()
    {
        GameEvents.OnStageTriggered += ShowStageMessage;
    }

    void OnDisable()
    {
        GameEvents.OnStageTriggered -= ShowStageMessage;
    }

    public void ShowStageMessage(GameStage stage)
    {
        StageMessage entry = GetStageMessage(stage);

        if (entry == null)
        {
            Debug.LogWarning("No message defined for stage: " + stage);
            return;
        }

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowPanelRoutine(entry));
    }

    StageMessage GetStageMessage(GameStage stage)
    {
        foreach (StageMessage entry in stageMessages)
        {
            if (entry.stage == stage)
                return entry;
        }

        return null;
    }

    IEnumerator ShowPanelRoutine(StageMessage entry)
    {
        panelText.text = entry.message;

        panel.SetActive(true);


        // disable player control
        if (playerController != null)
            playerController.enabled = false;


        if (redOverlay != null)
            redOverlay.SetActive(entry.useRedOverlay);


        yield return Fade(0, 1);

        yield return new WaitForSeconds(entry.customDisplayTime);

        yield return Fade(1, 0);

        panel.SetActive(false);

        if (redOverlay != null)
            redOverlay.SetActive(false);

        // re-enable player control
        if (playerController != null)
            playerController.enabled = true;
    }


    IEnumerator Fade(float start, float end)
    {
        float timer = 0;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, timer / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = end;
    }

    public void OnStageTriggered(GameStage stage)
    {
        // Show the panel
        panel.SetActive(true);

        // Special case for acid emergency
        if(stage == GameStage.acidEmergency)
        {
            redOverlay.SetActive(true);
        }
        else
        {
            redOverlay.SetActive(false);
        }
    }
}