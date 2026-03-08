using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    //This has been completely rewritten to make the camera more 3RD person friendly -phil
    public Camera cam;

    //Orbit angles
    private float xRotation = 0f;
    private float yRotation = 0f;

    //Sensitivity
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;

    public bool cameraYawIsIndependent = true;

    //(0, 1.5f, -3f) places the camera behind and above the player this can be changed to look better if we want
    //Note: zoom will change the Z component of this offset 
    public Vector3 cameraOffset = new Vector3(0f, 1.5f, -3f);

    //Where the camera looks at relative to the player's position (height)
    public float lookAtHeight = 1.0f;

    //cam movement smoothing
    public float smoothSpeed = 12f;

    //Zoom controls and stuff
    public float zoomSpeed = 2f;
    public float minZoom = 1f;
    public float maxZoom = 6f;

    private float currentZoom;
    private Vector3 initialCameraOffset;
    private float initialZSign = -1f;
    private Transform playerRoot;
    void Start()
    {
        //Capture initial offset and derive initial zoom from its Z value
        initialCameraOffset = cameraOffset;
        initialZSign = (initialCameraOffset.z == 0f) ? -1f : Mathf.Sign(initialCameraOffset.z);
        currentZoom = Mathf.Abs(initialCameraOffset.z);
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        playerRoot = transform.root;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0f)
        {
            ProcessZoom(scroll);
        }
    }


    public void ProcessZoom(float scrollDelta)
    {
        currentZoom -= scrollDelta * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
    }

    public void ProcessLook(Vector2 input)
    {
        float mouseX = input.x;
        float mouseY = input.y;

        //Update pitch and yaw from input
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        yRotation += (mouseX * Time.deltaTime) * xSensitivity;
        if (yRotation > 360f || yRotation < -360f) yRotation = Mathf.Repeat(yRotation, 360f);

        //effective offset that incorporates zoom
        Vector3 effectiveOffset = new Vector3(
            initialCameraOffset.x,
            initialCameraOffset.y,
            initialZSign * currentZoom
        );

        Vector3 desiredPosition;
        Quaternion camRotation;

        //Incredibly annoying math stuff 
        //Partially AI generated because this part is HARD
        if (cameraYawIsIndependent)
        {
            //Camera orbits around player using xRotation (pitch) and yRotation (yaw)
            camRotation = Quaternion.Euler(xRotation, yRotation, 0f);
            desiredPosition = playerRoot.position + camRotation * effectiveOffset;
            cam.transform.position = Vector3.Lerp(cam.transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
            cam.transform.LookAt(transform.position + Vector3.up * lookAtHeight);
        }
        else
        {
            //Keep the camera pitch, but use the player's yaw for horizontal orientation.
            //Rotate the player body with the mouse horizontal input.
            playerRoot.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);

            //Use player's current yaw so camera stays behind the player
            float playerYaw = playerRoot.eulerAngles.y;
            camRotation = Quaternion.Euler(xRotation, playerYaw, 0f);
            desiredPosition = playerRoot.position + camRotation * effectiveOffset;
            cam.transform.position = Vector3.Lerp(cam.transform.position, desiredPosition, Time.deltaTime * smoothSpeed);
            cam.transform.LookAt(transform.position + Vector3.up * lookAtHeight);

            //Keep internal yaw in sync so toggling modes won't jump
            yRotation = playerYaw;
        }
    }
}
