using UnityEngine;
using UnityEngine.InputSystem;

public class SubtleMouseMovement : MonoBehaviour
{
    public float moveSpeed = 15f;
    public float maxMove = 35f;

    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector2 startPosition;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        startPosition = rectTransform.anchoredPosition;
    }

    void Update()
    {
        if (Mouse.current == null) return;

        Vector2 mousePosition = Mouse.current.position.ReadValue();

        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint
        );

        // Direction from start position to cursor
        Vector2 direction = localPoint - startPosition;

        // Clamp movement distance
        direction = Vector2.ClampMagnitude(direction, maxMove);

        // Final allowed position
        Vector2 targetPosition = startPosition + direction;

        // Smooth movement
        rectTransform.anchoredPosition = Vector2.Lerp(
            rectTransform.anchoredPosition,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }
}
