using UnityEditor;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    private Player player;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 knockbackVelocity = Vector3.zero;
    private bool isGrounded;
    private bool isCrouching = false;
    private bool isSprinting = false;
    private bool isDashing = false;
    private bool lerpCrouch = false;
    [Header("Default Movement")]
    public float speed = 5f;
    public float jumpHeight = 1.25f;
    public float gravity = -12.0f;
    public Camera cam;
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
    [Header("Debug Bow")]
    public Transform arrowSpawnPoint;
    public GameObject arrowPrefab;
    public float arrowSpeed = 15f;
    public float arrowGravity = -9.8f;
    public int arrowPierce = 0;
    [Header("Misc")]
    private float dirLockX = 0f;
    private float dirLockZ = 0f;
    public Animator PlayerAnim_Controller;
    [Header("Player Rotation")]
    public float rotationSpeed = 0.1f;
    [Header("Knockback")]
    public float knockbackGravity = 20f;
    public float knockbackDrag = 15f; // Reduces knockback over time
    public float groundedDragMultiplier = 8f; // Multiplies knockbackDrag. Reduces knockback much more when grounded
    private bool isKnockedBack = false;




    Vector3 moveDirection = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInParent<Player>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        controller = player.CharacterController;



    }



    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
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
            PlayerAnim_Controller.SetBool("isGrounded", isGrounded);
            PlayerAnim_Controller.SetBool("isDashing", isDashing);
            PlayerAnim_Controller.SetFloat("Speed", speedValue, 0.1f, Time.deltaTime); //Triggers the run animation in the state machine
            PlayerAnim_Controller.SetBool("isMoving", speedValue > 0.1f);
            PlayerAnim_Controller.SetFloat("velocityX", dirLockX * speed, 0.1f, Time.deltaTime);
            PlayerAnim_Controller.SetFloat("velocityZ", dirLockZ * speed, 0.1f, Time.deltaTime);
            PlayerAnim_Controller.SetFloat("velocityY", playerVelocity.y, 0.1f, Time.deltaTime);

           
        }

        // If knockback velocity is low enough, remove the rest and set bool to false
        if (knockbackVelocity.magnitude < 0.01f)
        {
            knockbackVelocity = Vector3.zero;
            isKnockedBack = false;
        }

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
            
        } else
        {
            dashCooldown = 0f;
        }

        if (knockbackVelocity != Vector3.zero)
        {
            if (knockbackVelocity.y > 0)
                knockbackVelocity.y -= knockbackGravity * Time.deltaTime;
            else
                knockbackVelocity.y = 0;

            float activeDrag = knockbackDrag;
            if (isGrounded)
                activeDrag *= groundedDragMultiplier;

            Vector3 horizontal = new Vector3(knockbackVelocity.x, 0, knockbackVelocity.z);
            horizontal = Vector3.MoveTowards(horizontal, Vector3.zero, knockbackDrag * Time.deltaTime);
            knockbackVelocity.x = horizontal.x;
            knockbackVelocity.z = horizontal.z;

            if (knockbackVelocity.magnitude < 0.01f)
                knockbackVelocity = Vector3.zero;
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
            else
            {
                speed = 5f;
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
        PlayerAnim_Controller.SetTrigger("Dash");
        if (isKnockedBack) return; // prevent dashing during knockback

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
        }
        else
        {
            moveDirection.x = dirLockX;
            moveDirection.z = dirLockZ;
        }

        controller.Move(transform.TransformDirection(moveDirection) * speed * speedMult * Time.deltaTime);

        if (!isDashing)
        {
            playerVelocity.y += gravity * Time.deltaTime;
        }
        else
        {
            playerVelocity.y = 0f;
        }

        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
        }
        if (playerVelocity.y < -12.0f)
        {
            playerVelocity.y = -12.0f;
        }
        controller.Move(playerVelocity * Time.deltaTime);

        // Apply knockback on top of existing movement
        if (knockbackVelocity != Vector3.zero)
            controller.Move(knockbackVelocity * Time.deltaTime);
    }

    // Rotates the player mesh to face the direction of movement input
    /*
    public void RotatePlayerToMovement(Vector3 moveDir)
    {
        if (moveDir.magnitude > 0.1f && !isDashing)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir, Vector3.up);

            // Slerp to the new rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

           
            Vector3 angles = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0, angles.y, 0); 
        }
    }
    */

    public void Jump()
    {
        if (isGrounded)
        {
            PlayerAnim_Controller.SetTrigger("Jump");
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }

    public void ShootArrow()
    {
        // Grab player camera angle
        Vector3 direction = cam.transform.forward;

        // Set quaternion to camera facing angle, including vertical angle
        Quaternion arrowRotation = Quaternion.LookRotation(direction, Vector3.up);

        // Create arrow with above angle, set velocity
        var arrow = Instantiate(arrowPrefab, arrowSpawnPoint.position, arrowRotation);
        arrow.GetComponent<Rigidbody>().linearVelocity = direction * arrowSpeed;
    }

    public void ApplyKnockback(Vector3 velocity)
    { 
        // Debug.Log("Knockback applied");
        knockbackVelocity = velocity;
        isKnockedBack = true;
    }

    // Helper for purely vertical knockback to keep certain calls simpler
    public void ApplyKnockup(float force)
    {
        // Debug.Log("Knockup applied");
        ApplyKnockback(Vector3.up * force);
    }



}
