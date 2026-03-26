using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpSystem : MonoBehaviour
{
    // =========================
    // UI REFERENCES
    // =========================

    // The dialogue box panel
    public GameObject popUpBox;

    // The text inside the dialogue box
    public TMP_Text popUpText;

    // Camera used for raycasting (MUST be assigned in Inspector)
    public Camera cam;

    // (OPTIONAL - currently unused)
    public TMP_Text continueIndicator;

    // =========================
    // HOVER SYSTEM VARIABLES
    // =========================

    // Stores the NPC currently being hovered
    private GameObject currentHover;

    // Color when hovering over NPC
    public Color hoverColor = Color.yellow;

    // Stores original color so we can reset it
    private Color originalColor;

    // =========================
    // DIALOGUE VARIABLES (SIMPLIFIED)
    // =========================

    // Currently active dialogue lines
    private string[] lines;

    // Track current line (NOT USED RIGHT NOW)
    // private int currentLine = 0;

    // Track if dialogue is active
    private bool isActive = false;

    void Start()
    {
        // Hide dialogue box at start
        popUpBox.SetActive(false);
    }

    void Update()
    {
        // Always check for hover (runs every frame)
        HandleHover();

        // Detect click (left mouse)
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }

        // =========================
        // DISABLED CLICK-TO-CONTINUE SYSTEM
        // =========================

        /*
        if (!isActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            // Old system for progressing dialogue
        }
        */
    }

    // =========================
    // HANDLE CLICKING NPCs
    // =========================

    void HandleClick()
    {
        Debug.Log("Clicked!");

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            Debug.Log("Hit object: " + hit.transform.name);

            // Try to find NPCData on this object or its parent
            NPCData npc = hit.transform.GetComponentInParent<NPCData>();

            if (npc != null)
            {
                Debug.Log("NPC FOUND!");

                // Start dialogue using NPC's lines
                StartDialogue(npc.dialogueLines);
            }
        }
    }

    // =========================
    // START DIALOGUE (SIMPLIFIED)
    // =========================

    public void StartDialogue(string[] dialogueLines)
    {
        lines = dialogueLines;
        isActive = true;

        // Show dialogue box
        popUpBox.SetActive(true);

        // Show ONLY the first line (no multi-line system)
        if (lines != null && lines.Length > 0)
        {
            popUpText.text = lines[0];
        }
    }

    // =========================
    // END DIALOGUE (BUTTON USES THIS)
    // =========================

    public void EndDialogue()
    {
        isActive = false;

        // Hide UI
        popUpBox.SetActive(false);

        // Clear text
        popUpText.text = "";

        // Reset hover (optional safety)
        ResetHover();
    }

    // =========================
    // HOVER SYSTEM
    // =========================

    void HandleHover()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            NPCData npc = hit.transform.GetComponentInParent<NPCData>();

            if (npc != null)
            {
                GameObject npcObject = npc.gameObject;

                // If hovering a NEW NPC
                if (currentHover != npcObject)
                {
                    ResetHover();

                    currentHover = npcObject;

                    Renderer renderer = currentHover.GetComponentInChildren<Renderer>();

                    if (renderer != null)
                    {
                        // Store original color
                        originalColor = renderer.material.color;

                        // Apply hover color
                        renderer.material.color = hoverColor;
                    }
                }
                return;
            }
        }

        // If not hovering NPC → reset
        ResetHover();
    }

    // Reset NPC color
    void ResetHover()
    {
        if (currentHover != null)
        {
            Renderer renderer = currentHover.GetComponentInChildren<Renderer>();

            if (renderer != null)
            {
                renderer.material.color = originalColor;
            }

            currentHover = null;
        }
    }

    // =========================
    // DISABLED MULTI-LINE + TYPEWRITER SYSTEM
    // =========================

    /*
    private int currentLine = 0;
    private bool isTyping = false;
    public float typingSpeed = 0.03f;

    IEnumerator TypeLine()
    {
        isTyping = true;

        popUpText.text = "";
        continueIndicator.text = "";

        foreach (char c in lines[currentLine])
        {
            popUpText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        continueIndicator.text = "...";
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine < lines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }
    */
}