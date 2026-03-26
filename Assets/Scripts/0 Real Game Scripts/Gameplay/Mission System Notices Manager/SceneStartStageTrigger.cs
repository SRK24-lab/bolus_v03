using UnityEngine;
using System.Collections;

public class SceneStartStageTrigger : MonoBehaviour
{
    public GameStage stageToTrigger;
    public float delay = 1f;

    IEnumerator Start()
    {
        // small frame delay so listeners subscribe
        yield return null;

        // actual configurable delay
        yield return new WaitForSeconds(delay);

        GameEvents.TriggerStage(stageToTrigger);
    }
}