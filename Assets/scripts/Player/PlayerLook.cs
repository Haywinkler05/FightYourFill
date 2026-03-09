using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam; // Keep your camera reference
    public Transform cameraPivot; // The empty object that follows the head
    public Transform playerRoot;

    private float xRotation = 0f;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        // 1. Calculate vertical rotation
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;

        // 2. CLAMP: Tighten this to 80 or 75 if you still see your own shoulders
        xRotation = Mathf.Clamp(xRotation, -60f, 89f);

        // 3. APPLY TO PIVOT: Rotating the parent keeps the camera's local 
        // offset from "swinging" into the neck.
        cameraPivot.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // 4. Horizontal Rotation: Rotate the whole player body
        playerRoot.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
    }
}