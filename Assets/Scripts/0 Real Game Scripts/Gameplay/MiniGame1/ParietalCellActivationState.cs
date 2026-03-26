using UnityEngine;

public class ParietalCellActivationState : StatueActivationBase
{
    [Header("Keys")]
    public GameObject gastrinKey1;
    public GameObject gastrinKey2;

    [Header("References")]
    public ChiefCellActivationState chiefCell;
    public GCellActivationState gCell;

    public GameObject geyser;
    public GameObject ripple;
    public Animator animator;
    public GameStage stageToTrigger;

    private StatueState previousState;
    

    void Start()
    {
        geyser.SetActive(false);
        ripple.SetActive(false);
    } 

    protected override void Update()
    {
        base.Update(); // call base update for counter THEN override
        UpdateState();
    }

    public void UpdateState()
    {
        if (currentState == StatueState.PostActivation) return;
        bool key1Active = gastrinKey1.activeSelf;
        bool key2Active = gastrinKey2.activeSelf;

        //bool oneKeyActive = key1Active ^ key2Active;
        bool noKeysActive = !key1Active && !key2Active;

        // KEY 1 ALWAYS USED ON CHIEF CELL, KEY 2 ALWAYS USED ON PARIETAL CELL.

        // Condition 1: No keys active and G cell not interacted → PreActivation
        if (noKeysActive && gCell.currentState != StatueState.PostActivation)
        {
            currentState = StatueState.PreActivation;
        }

        // Condition 2: Both keys active → Activated
        else if (key1Active && key2Active)
        {
            currentState = StatueState.Activated;
            Debug.Log("Parietal cell is active now.");
        }

        // Condition 3: One key active AND Chief already Post → Activated
        else if (key2Active && chiefCell.currentState == StatueState.PostActivation)
        {
            currentState = StatueState.Activated;
            Debug.Log("Parietal cell is active now.");
        }

        // Condition 4: One key active AND Chief not Post → PostActivation
        else if (!key2Active && chiefCell.currentState != StatueState.PostActivation)
        {
            currentState = StatueState.PostActivation;
            Debug.Log("Parietal cell is post-active now.");
        }

        // Condition 5: No keys active AND G cell Post → PostActivation
        else if (noKeysActive && gCell.currentState == StatueState.PostActivation)
        {
            currentState = StatueState.PostActivation;
            Debug.Log("Parietal cell is post-active now.");
        }

        // Handle geyser visibility AFTER state is determined
        geyser.SetActive(currentState == StatueState.PostActivation);
        ripple.SetActive(currentState == StatueState.PostActivation);

        if (currentState == StatueState.PostActivation)
        {
            animator.SetTrigger("parietal_activated");
        }

        // Trigger stage ONLY when state changes
        if (currentState == StatueState.PostActivation && previousState != StatueState.PostActivation)
        {
            GameEvents.TriggerStage(stageToTrigger);
        }

        previousState = currentState;
        
    
    }

    public override void ActivateStatue() { }

    public override void DeactivateStatue()
    {
        if (currentState == StatueState.Activated)
            currentState = StatueState.PostActivation;
    }
}