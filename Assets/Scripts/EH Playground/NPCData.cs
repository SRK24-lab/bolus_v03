using UnityEngine;
// This script is a simple data container for NPC dialogue lines, add this to any NPC GameObject and fill in the dialogue lines in the Inspector. I.e add it to the Statues and Enoki and the dialogue system will automatically pull the lines from here when you click on them.
public class NPCData : MonoBehaviour
{
    // Dialogue lines for this NPC (set in Inspector)
    [TextArea]
    public string[] dialogueLines;
}