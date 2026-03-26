using UnityEngine;

public class CameraZoomOnMove : MonoBehaviour
{
    public Transform player;

    public float normalSize = 4f;
    public float zoomedSize = 0.5f;

    public float zoomSmoothTime = 0.5f;
    public float movementThreshold = 0.01f;

    private Camera cam;
    private Vector3 lastPlayerPosition;
    private float zoomVelocity;

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = normalSize;
        lastPlayerPosition = player.position;
    }

    void LateUpdate()
    {
        float movement = Vector3.Distance(player.position, lastPlayerPosition);

        bool isMoving = movement > movementThreshold;

        float targetSize = isMoving ? zoomedSize : normalSize;

        cam.orthographicSize = Mathf.SmoothDamp(
            cam.orthographicSize,
            targetSize,
            ref zoomVelocity,
            zoomSmoothTime
        );

        lastPlayerPosition = player.position;
    }
}