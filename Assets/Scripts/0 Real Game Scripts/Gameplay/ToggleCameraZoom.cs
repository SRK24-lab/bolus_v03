using UnityEngine;

public class ToggleCameraZoom : MonoBehaviour
{
    [Header("Camera Zoom Script")]
    public CameraZoomOnMove zoomableScript;

    // Call this to enable or disable the zoom script
    public void SetScriptState(bool newState)
    {
        if (zoomableScript != null)
        {
            zoomableScript.enabled = newState;
        }
    }
}