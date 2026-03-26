using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float rotationSpeed = 12f;

    [Header("Walking Wobble")]
    [SerializeField] private float wobbleAmount = 10f;
    [SerializeField] private float wobbleSpeed = 15f;

    [Header("Ground Detection")]
    [SerializeField] private float groundCheckDistance = 2f;
    [SerializeField] private float groundOffset = 0.5f;
    [SerializeField] private float groundSphereRadius = 0.3f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Collision")]
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float playerRadius = 0.4f;
    [SerializeField] private float playerHeight = 2f;

    private Vector2 moveInput;
    private bool wasMoving = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    void Update()
    {
        GatherInput();
        HandleWalkingAudio();
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

        // Flipped controls
        moveInput = new Vector2(-horizontal, -vertical).normalized;
    }

    void HandleWalkingAudio()
    {
        bool isMoving = moveInput != Vector2.zero;

        if (isMoving && !wasMoving)
        {
            // Start walking sound
            //AudioManager.Instance?.StartWalking();
        }
        else if (!isMoving && wasMoving)
        {
            // Stop walking sound
            //AudioManager.Instance?.StopWalking();
        }

        wasMoving = isMoving;
    }

    void Move()
    {
        Vector3 move = Vector3.zero;

        if (moveInput != Vector2.zero)
        {
            // Convert input to isometric direction
            Vector3 isoDir = new Vector3(
                moveInput.x - moveInput.y,
                0f,
                moveInput.x + moveInput.y
            ).normalized;

            move = isoDir * moveSpeed * Time.fixedDeltaTime;

            // Rotation
            Quaternion targetRotation = Quaternion.LookRotation(isoDir);

            float wobble = Mathf.Sin(Time.time * wobbleSpeed) * wobbleAmount;
            Quaternion wobbleRotation = Quaternion.Euler(0f, 0f, wobble);

            Quaternion finalRotation = targetRotation * wobbleRotation;

            rb.MoveRotation(
                Quaternion.Slerp(rb.rotation, finalRotation, rotationSpeed * Time.fixedDeltaTime)
            );
        }
        else
        {
            Quaternion upright = Quaternion.Euler(0, rb.rotation.eulerAngles.y, 0);
            rb.MoveRotation(
                Quaternion.Slerp(rb.rotation, upright, rotationSpeed * Time.fixedDeltaTime)
            );
        }

        Vector3 moveDir = move.normalized;
        float moveDistance = move.magnitude;

        Vector3 capsuleBottom = rb.position + Vector3.up * playerRadius;
        Vector3 capsuleTop = rb.position + Vector3.up * (playerHeight - playerRadius);

        // Check for wall blocking
        if (!Physics.CapsuleCast(
            capsuleBottom,
            capsuleTop,
            playerRadius,
            moveDir,
            moveDistance,
            wallLayer,
            QueryTriggerInteraction.Ignore))
        {
            Vector3 newPos = rb.position + move;

            // Ground detection (stairs / slopes)
            if (Physics.SphereCast(
                newPos + Vector3.up * 1f,
                groundSphereRadius,
                Vector3.down,
                out RaycastHit hit,
                groundCheckDistance,
                groundLayer,
                QueryTriggerInteraction.Ignore))
            {
                newPos.y = hit.point.y + groundOffset;
            }

            rb.MovePosition(newPos);
        }
    }
}