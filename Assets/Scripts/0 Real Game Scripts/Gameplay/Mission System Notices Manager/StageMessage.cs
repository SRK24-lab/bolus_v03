using UnityEngine;

[System.Serializable]
public class StageMessage
{
    public GameStage stage;

    [TextArea(3,5)]
    public string message;

    [Header("Display Settings")]
    public float customDisplayTime = 2.75f;

    public bool useRedOverlay = false;
}