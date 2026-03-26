using UnityEngine;


public class SpinY : MonoBehaviour
{
    public float rotationSpeed = 100f; // degrees per second


    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
