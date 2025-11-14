using UnityEngine;

public class CursorController : MonoBehaviour
{
    public Texture2D defaultCursor; // Assign your normal cursor texture
    public Texture2D clickCursor;   // Assign your clicked cursor texture
    public Vector2 hotspot = Vector2.zero; // The "tip" of the cursor

    void Start()
    {
        // Set the default cursor at the start
        Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            Cursor.SetCursor(clickCursor, hotspot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            Cursor.SetCursor(defaultCursor, hotspot, CursorMode.Auto);
        }
    }
}