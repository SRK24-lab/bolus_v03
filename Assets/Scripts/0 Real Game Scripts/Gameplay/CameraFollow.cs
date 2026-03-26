using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float xOffset = -2f;
    public float zOffset= 8.5f;

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 pos = transform.position;
        pos.x = player.position.x + xOffset;
        pos.z = player.position.z + zOffset;

        transform.position = pos;
    }
}