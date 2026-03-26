using UnityEngine;
using System.Collections;

public class MucusCellActivationState : StatueActivationBase
{
    public ChiefCellActivationState chiefCell;
    public ParietalCellActivationState parietalCell;

    private bool interactedOnce = false;

    public GameObject geyser;
    public GameObject ripple;
    public Animator animator;

    public GameStage stageToTrigger1;
    public GameStage stageToTrigger2;

    private float delay = 4f;

    void Start()
    {
        geyser.SetActive(false);
        ripple.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        UpdateState();
    }

    private void UpdateState()
    {
        // LOCKED state (after interaction)
        if (interactedOnce)
        {
            currentState = StatueState.PostActivation;
            geyser.SetActive(true);
            ripple.SetActive(true);
            return;
        }

        // Check if ready to activate
        if (chiefCell.currentState == StatueState.PostActivation &&
            parietalCell.currentState == StatueState.PostActivation)
        {
            currentState = StatueState.Activated;
        }
        else
        {
            currentState = StatueState.PreActivation;
        }
    }

    public override void ActivateStatue()
    {
        if (currentState == StatueState.Activated && !interactedOnce)
        {
            interactedOnce = true;
            currentState = StatueState.PostActivation;

            geyser.SetActive(true);
            ripple.SetActive(true);

            animator.SetTrigger("mucous_activated");

            //AudioManager.Instance.StartFountain();

            StartCoroutine(ActivationSequence());
        }
    }

    IEnumerator ActivationSequence()
    {
        GameEvents.TriggerStage(stageToTrigger1);

        yield return new WaitForSeconds(delay);

        GameEvents.TriggerStage(stageToTrigger2);

        Debug.Log("Mucus cell is post-active now.");
    }

    public override void DeactivateStatue() { }
}