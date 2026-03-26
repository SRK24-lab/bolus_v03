using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 12f;

    // Walking wobble (tilt)
    [SerializeField] private float wobbleAmount = 10f;
    [SerializeField] private float wobbleSpeed = 15f;

    // Walking bob (vertical bounce)
    [SerializeField] private float bobAmount = 0.05f;
    [SerializeField] private float bobSpeed = 15f;

    private Vector2 moveInput;
    private float baseY;

    void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        rb.useGravity = false;

        // Fully stabilize physics
        rb.constraints =
            RigidbodyConstraints.FreezeRotation |
            RigidbodyConstraints.FreezePositionY;

        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        baseY = rb.position.y;
    }

    void Update()
    {
        GatherInput();
    }

    void FixedUpdate()
    {
        Move();
    }

    void GatherInput()
    {
        var keyboard = Keyboard.current;

        float horizontal = 0;
        float vertical = 0;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) horizontal -= 1;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) horizontal += 1;

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) vertical += 1;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) vertical -= 1;

        moveInput = new Vector2(horizontal, vertical).normalized;
    }

    void Move()
    {
        if (moveInput == Vector2.zero)
        {
            // Return to upright rotation smoothly
            Quaternion upright = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0);

            rb.MoveRotation(
                Quaternion.Slerp(rb.rotation, upright, rotationSpeed * Time.fixedDeltaTime)
            );

            // Ensure no drifting
            rb.linearVelocity = Vector3.zero;

            return;
        }

        // Convert input to isometric direction
        Vector3 isoDir = new Vector3(
            moveInput.x - moveInput.y,
            0f,
            moveInput.x + moveInput.y
        ).normalized;

        // Base facing rotation
        Quaternion targetRotation = Quaternion.LookRotation(isoDir);

        // Walking wobble
        float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
        Quaternion wobbleRotation = Quaternion.Euler(0f, 0f, wobble);

        Quaternion finalRotation = targetRotation * wobbleRotation;

        rb.MoveRotation(
            Quaternion.Slerp(rb.rotation, finalRotation, rotationSpeed * Time.fixedDeltaTime)
        );

        // Vertical bobbing
        float bob = Mathf.Sin(Time.time * bobSpeed) * bobAmount;

        Vector3 move = isoDir * moveSpeed * Time.fixedDeltaTime;

        Vector3 newPos = rb.position + move;
        newPos.y = baseY + bob;

        rb.MovePosition(newPos);
    }
}