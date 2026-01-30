using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomSpeed = 10f;
    public float minZ = -20f;
    public float maxZ = 0f;

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        Vector3 pos = transform.position;
        pos.z = Mathf.Clamp(pos.z + scroll * zoomSpeed, minZ, maxZ);
        transform.position = pos;
    }
}
