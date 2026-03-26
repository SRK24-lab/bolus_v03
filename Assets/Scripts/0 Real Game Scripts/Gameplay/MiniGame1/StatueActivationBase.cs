using UnityEngine;

public abstract class StatueActivationBase : MonoBehaviour
{
    public enum StatueState
    {
        PreActivation,
        Activated,
        PostActivation
    }

    public StatueState currentState = StatueState.PreActivation;

    // GLOBAL COUNTER
    public static int postActivatedStatueCount = 0;

    private StatueState previousState;

    protected virtual void Update()
    {
        // Detect transition into PostActivation
        if (currentState == StatueState.PostActivation &&
            previousState != StatueState.PostActivation)
        {
            postActivatedStatueCount++;

            Debug.Log("Statues completed: " + postActivatedStatueCount);
        }

        previousState = currentState;
    }

    public abstract void ActivateStatue();

    public virtual void DeactivateStatue()
    {
        currentState = StatueState.PostActivation;
    }
}