using UnityEngine;

public class MoveElements : MonoBehaviour
{
    [Header("Element Info")]
    public PharmaceuticalElement elementType;
    public bool isMolecule;

    [Header("Drag Settings")]
    [Range(0.01f, 1f)]
    [Tooltip("Lower = more floaty, Higher = more responsive")]
    public float dragSmoothness = 0.15f;

    [Header("Release Settings")]
    [Range(0.8f, 0.99f)]
    [Tooltip("How much momentum is preserved (higher = slides more)")]
    public float slideDeceleration = 0.95f;

    [Range(0.1f, 5f)]
    [Tooltip("Minimum speed before stopping")]
    public float stopThreshold = 0.1f;

    [Header("Boundary Settings")]
    public bool constrainToCameraBounds = true;
    public float boundaryPadding = 0.1f; // Extra space from edge

    [Header("Gravity Settings")]
    public float dropSpeed;

    private bool isDragging = false;
    private Vector3 offset;
    private float zCoordinate;
    private Vector3 velocity = Vector3.zero;
    private Vector3 targetPosition;
    private bool isSliding = false;

    void OnMouseDown()
    {
        //Stop any sliding immediately
        isSliding = false;
        velocity = Vector3.zero; // Reset velocity to prevent carrying over momentum

        //Store the z-coordinate of the object (distance from camera)
        zCoordinate = Camera.main.WorldToScreenPoint(transform.position).z;

        //Calculate offset between mouse position and object position
        offset = transform.position - GetMouseWorldPos();

        //Set target position immediately to prevent jump
        targetPosition = transform.position;

        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            targetPosition = GetMouseWorldPos() + offset;

            //Constrain to camera bounds if enabled
            if (constrainToCameraBounds)
            {
                targetPosition = ClampToCamera(targetPosition);
            }
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
        isSliding = true; //Start sliding with current velocity
    }

    void Update()
    {
        if (isDragging)
        {
            //Smooth movement while dragging
            transform.position = Vector3.SmoothDamp(
                transform.position,
                targetPosition,
                ref velocity,
                dragSmoothness
            );
        }
        else if (isSliding)
        {
            //Apply deceleration to velocity
            velocity *= slideDeceleration;

            //Move based on velocity
            Vector3 desiredPosition = transform.position + velocity * Time.deltaTime;
            Vector3 newPosition = desiredPosition;

            //Constrain to camera bounds if enabled
            if (constrainToCameraBounds)
            {
                newPosition = ClampToCamera(newPosition);

                //Bounce off edges by reversing velocity
                if (newPosition.x != desiredPosition.x)
                {
                    velocity.x *= -0.5f; // Bounce with energy loss
                }
                if (newPosition.y != desiredPosition.y)
                {
                    velocity.y *= -0.5f; // Bounce with energy loss
                }
            }

            transform.position = newPosition;

            // Stop sliding when velocity is low enough
            if (velocity.magnitude < stopThreshold)
            {
                isSliding = false;
                velocity = Vector3.zero;
            }
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        // Get mouse position on screen
        Vector3 mousePoint = Input.mousePosition;

        // Maintain the same z-coordinate
        mousePoint.z = zCoordinate;

        // Convert screen position to world position
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private Vector3 ClampToCamera(Vector3 position)
    {
        Camera cam = Camera.main;

        // Get the sprite's size for accurate boundary checking
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        float spriteHalfWidth = spriteRenderer ? spriteRenderer.bounds.extents.x : 0.5f;
        float spriteHalfHeight = spriteRenderer ? spriteRenderer.bounds.extents.y : 0.5f;

        // Calculate camera bounds in world space
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        // Clamp position within bounds
        position.x = Mathf.Clamp(
            position.x,
            -camWidth + spriteHalfWidth + boundaryPadding,
            camWidth - spriteHalfWidth - boundaryPadding
        );

        // position.y = Mathf.Clamp(position.y, -camHeight + spriteHalfHeight + boundaryPadding, camHeight - spriteHalfHeight - boundaryPadding);
        return position;
    }
}