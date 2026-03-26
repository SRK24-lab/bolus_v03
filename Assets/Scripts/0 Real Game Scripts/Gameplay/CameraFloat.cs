using UnityEngine;

public class CameraFloat : MonoBehaviour
{
    // Adjust these variables in the Inspector to fine-tune the effect
    public float amplitudeX = 0.01f;   // Max distance the camera moves on X axis
    public float amplitudeY = 0.01f;   // Max distance the camera moves on Y axis
    public float frequencyX = 1f;      // Speed of the float on X axis
    public float frequencyY = 1f;      // Speed of the float on Y axis
    public float amplitudeRot = 0.1f;  // Max rotation angle
    public float frequencyRot = 0.5f;  // Speed of the rotation

    private Vector3 initialLocalPosition;
    private Quaternion initialLocalRotation;

    void Start()
    {
        // Store the camera's initial local position and rotation relative to its parent
        initialLocalPosition = transform.localPosition;
        initialLocalRotation = transform.localRotation;
    }

    void Update()
    {
        // Calculate smooth, time-based offsets using sine waves
        float offsetX = Mathf.Sin(Time.time * frequencyX) * amplitudeX;
        float offsetY = Mathf.Sin(Time.time * frequencyY + Mathf.PI / 2) * amplitudeY; // Add PI/2 for slightly different timing
        
        // Apply the position offset
        transform.localPosition = initialLocalPosition + new Vector3(offsetX, offsetY, 0);

        // Calculate a slight rotation offset
        float offsetRot = Mathf.Sin(Time.time * frequencyRot) * amplitudeRot;

        // Apply the rotation offset (optional)
        transform.localRotation = initialLocalRotation * Quaternion.Euler(offsetRot, 0, 0);
    }
}
