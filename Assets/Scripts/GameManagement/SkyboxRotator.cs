using UnityEngine;

public class SkyboxRotator : MonoBehaviour
{
    public float rotationSpeed = 1f; // degrees per second

    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation",
            RenderSettings.skybox.GetFloat("_Rotation") + rotationSpeed * Time.deltaTime);
    }
}
