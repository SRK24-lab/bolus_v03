using System.Collections;
using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    public GameStage stageToTrigger;

    [Header("Delay Before Trigger")]
    public float delay = 3f;

    private bool triggered = false;

    public void TriggerStage()
    {
        if (triggered) return;

        triggered = true;
        StartCoroutine(TriggerWithDelay());
    }

    IEnumerator TriggerWithDelay()
    {
        yield return new WaitForSeconds(delay);

        GameEvents.TriggerStage(stageToTrigger);
    }
}