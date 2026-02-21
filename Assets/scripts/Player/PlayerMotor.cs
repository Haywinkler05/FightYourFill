using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    private bool isCrouching = false;
    private bool isSprinting = false;
    private bool isDashing = false;
    private bool lerpCrouch = false;
    [Header("Default Movement")]
    public float speed = 5f;
    public float jumpHeight = 1.25f;
    public float gravity = -12.0f;
    [Header("Sprint")]
    public float sprintSpeed = 8f;
    [Header("Dash and Roll")]
    public float dashSpeed = 4f;
    public float rollSpeed = 4f;
    public float dashCooldown = 0f;
    public float speedMult = 1f;
    public float speedMultDecay = 12f;
    [Header("Crouch")]
    public float crouchTimer = 1f;
    public float crouchSpeedMult = 0.8f;
    [Header("Misc")]
    private float dirLockX = 0f;
    private float dirLockZ = 0f;
    public Animator PlayerAnim_Controller; 
    
    Vector3 moveDirection = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //This works now!!!! This was annoying to work out so please don't mess with this before talking to me first. -phil
        //Compute horizontal speed: prefer CharacterController velocity but fall back to intended movement if velocity is not yet updated
        float speedValue = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
        if (Mathf.Approximately(speedValue, 0f))
        {
            //Use moveDirection * speed to estimate current horizontal speed when controller.velocity is not yet updated
            speedValue = new Vector2(moveDirection.x, moveDirection.z).magnitude * speed * speedMult;
        }
        //Debug.Log("Speed " + speedValue);
        if (PlayerAnim_Controller != null)
        {
            PlayerAnim_Controller.SetFloat("Speed", speedValue); //Triggers the run animation in the state machine
        }

        isGrounded = controller.isGrounded;

        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (isCrouching)
            {
                controller.height = Mathf.Lerp(controller.height, 1, p);
            }
            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }

        if (speedMult > 1f)
        {
            speedMult -= (speedMultDecay * Time.deltaTime);
            speed = 5f;
        } else if (speedMult < 1f)
        {
            speedMult = 1f;
            isDashing = false;
            if (isSprinting)
            {
                speed = sprintSpeed;
            }
        }

        if (dashCooldown > 0f)
        {
            dashCooldown -= Time.deltaTime;
            Debug.Log(dashCooldown);
        } else
        {
            dashCooldown = 0f;
        }
    }

    public void Crouch()
    {
        if (isDashing)
        {
            return;
        }
        isCrouching = !isCrouching;
        crouchTimer = 0;
        lerpCrouch = true;
        isSprinting = false;
        speed = 5f;
    }

    public void Sprint()
    {
        if (!isCrouching)
        {
            isSprinting = !isSprinting;
            if (isSprinting && !isDashing)
            {
                speed = 8f;
            }
        }
        else
        {
            isSprinting = false;
            speed = 5f;
        }
    }

    public void Dash()
    {
        if ((dirLockX != 0f || dirLockZ != 0f) && dashCooldown == 0f && (!isCrouching))
        {
            speedMult = dashSpeed;
            isDashing = true;
            dashCooldown = 2f;
            speed = 5f;
        }
        else if ((dirLockX != 0f || dirLockZ != 0f) && dashCooldown == 0f && (isCrouching && isGrounded))
        {
            speedMult = rollSpeed;
            isDashing = true;
            dashCooldown = 1f;
            speed = 5f;
        }
        else
        {
            return;
        }
    }

    // Receives inputs from InputManager script and apply to character controller
    public void ProcessMove(Vector2 input)
    {

        if (!isDashing)
        {
            moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            dirLockX = input.x;
            dirLockZ = input.y;
        } else
        {
            moveDirection.x = dirLockX;
            moveDirection.z = dirLockZ;
        }

        controller.Move(transform.TransformDirection(moveDirection) * speed * speedMult * Time.deltaTime);

        if (!isDashing)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        } else
        {
            playerVelocity.y = 0f;
        }

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        if(playerVelocity.y < -12.0f)
        {
            playerVelocity.y = -12.0f;
        }
        controller.Move(playerVelocity * Time.deltaTime);
        //Debug.Log(playerVelocity.y);
    }
    public void Jump()
    {
        if (isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
}
