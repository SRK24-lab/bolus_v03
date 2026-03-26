using UnityEngine;

public class GCellActivationState : StatueActivationBase
{
    public GameObject gastrinKey1;
    public GameObject gastrinKey2;
    public GameStage stageToTrigger;

    private bool hasInteracted = false;


    void Start()
    {
        // G Cell statue begins already active
        currentState = StatueState.Activated;
    }

        protected override void Update()
    {
        base.Update(); // call base update for counter THEN override
    }

    public override void ActivateStatue()
    {
        // Not really needed for this statue, but kept for consistency
        currentState = StatueState.Activated;
        Debug.Log ("is activated");
    }

    public override void DeactivateStatue()
    {
        if (hasInteracted) return;

        if (gastrinKey1.activeSelf || gastrinKey2.activeSelf)
        {
            Debug.Log("A key is active");
        }

        hasInteracted = true;
        currentState = StatueState.PostActivation;

        GameEvents.TriggerStage(stageToTrigger);


        Debug.Log ("G cell is post-activated now.");

    }
}