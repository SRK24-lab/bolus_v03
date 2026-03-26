using UnityEngine;

public class bgFloat : MonoBehaviour
{
    public float moveAmount = 150f;     // max movement in pixels
    public float moveSpeed = 0.1f;     // speed of drifting

    public float scaleAmount = 0.01f;  // subtle breathing
    public float scaleSpeed = 0.5f;

    private RectTransform rect;
    private Vector2 startPos;
    private Vector3 startScale;

    float seedX;
    float seedY;

    void Start()
    {
        rect = GetComponent<RectTransform>();

        startPos = rect.anchoredPosition;
        startScale = rect.localScale;

        // random seeds so multiple panels move differently
        seedX = Random.Range(0f, 100f);
        seedY = Random.Range(0f, 100f);
    }

    void Update()
    {
        float time = Time.time * moveSpeed;

        // Smooth drifting motion
        float x = (Mathf.PerlinNoise(seedX, time) - 0.5f) * 2f * moveAmount;
        float y = (Mathf.PerlinNoise(seedY, time) - 0.5f) * 2f * moveAmount;

        rect.anchoredPosition = startPos + new Vector2(x, y);


        // Subtle breathing scale
        float scale = 1 + (Mathf.PerlinNoise(seedY + 20, time * scaleSpeed) - 0.5f) * 2f * scaleAmount;
        rect.localScale = startScale * scale;
    }
}