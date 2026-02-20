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
    public float speed = 5f;
    public float dashSpeed = 4f;
    public float speedMult = 1f;
    public float speedMultDecay = 12f;
    public float gravity = -12.0f;
    public float jumpHeight = 1.25f;
    public float crouchTimer = 1f;
    public float dirLockX = 0f;
    public float dirLockZ = 0f;
    public float dashCooldown = 0f;

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
        //This doesn't work I will try to fix soon hopefully -phil
        float speedValue = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;  
        //Debug.Log("Speed " + speedValue);
        if (PlayerAnim_Controller != null)
        {
            PlayerAnim_Controller.SetFloat("Speed", speedValue);
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
            } else
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
                speed = 8f;
            }
        }

        if (dashCooldown > 0f)
        {
            dashCooldown -= (2f * Time.deltaTime);
            Debug.Log(dashCooldown);
        } else
        {
            dashCooldown = 0f;
        }
    }

    public void Crouch()
    {
        isCrouching = !isCrouching;
        crouchTimer = 0;
        lerpCrouch = true;
    }

    public void Sprint()
    {
        isSprinting = !isSprinting;
        if (isSprinting)
        {
            speed = 8f;
        }
        else
        {
            speed = 5f;
        }
    }

    public void Dash()
    {
        if ((dirLockX != 0f || dirLockZ != 0f) && dashCooldown == 0f)
        {
            speedMult = dashSpeed;
            isDashing = true;
            dashCooldown = 3f;
            speed = 5f;
        } else
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
