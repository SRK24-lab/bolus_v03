using UnityEngine;
[RequireComponent(typeof(Collider))]

public class PyloricGatekeep : MonoBehaviour


{
    [Header("Stage Settings")]
    public GameStage stageToTrigger;

    [Header("Highlight Settings")]
    public GameObject objectToHighlight;   // the object that visually reacts
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 2f;

    private Color originalColor;
    private Renderer objRenderer;
    private bool triggered = false;

    void Start()
    {
        if (objectToHighlight != null)
        {
            objRenderer = objectToHighlight.GetComponent<Renderer>();
            if (objRenderer != null)
            {
                originalColor = objRenderer.material.color;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player")) // make sure your player has the "Player" tag
        {
            triggered = true;

            // Highlight object
            HighlightObject(true);

            // Trigger game stage dialogue
            GameEvents.TriggerStage(stageToTrigger);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!triggered) return;

        if (other.CompareTag("Player"))
        {
            // Remove highlight when leaving
            HighlightObject(false);
        }
    }

    void HighlightObject(bool enable)
    {
        if (objRenderer == null) return;

        if (enable)
        {
            objRenderer.material.color = highlightColor * highlightIntensity;
        }
        else
        {
            objRenderer.material.color = originalColor;
        }
    }
}
