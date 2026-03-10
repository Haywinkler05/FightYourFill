using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    public PlayerInput.OnFootActions onFoot;
    public PlayerInput.UIActions ui;
    private Player player;
    private PlayerMotor motor;
    private playerCombat combat;
    private PlayerLook look;
   // private PauseMenu pause;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        //ui = playerInput.UI;
        player = GetComponentInParent<Player>();
        combat = player.Combat;
        motor = player.Motor;
        look = player.Look;
        //pause = GetComponent<PauseMenu>();

        onFoot.Jump.performed += ctx => motor.Jump();

        onFoot.Crouch.performed += ctx => motor.Crouch();
        onFoot.Sprint.performed += ctx => motor.Sprint();
        onFoot.Dash.performed += ctx => motor.Dash();
        onFoot.ShootArrow.performed += ctx => motor.ShootArrow();
        onFoot.M1Attack.performed += ctx => combat.basicAttack();

        //ui.Pause.performed += ctx => pause.Pause();


    }

    // Update is called once per frame
    void Update()
    {
        // Tell PlayerMotor to move using value from movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
    }

    private void LateUpdate()
    {
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }
    private void OnDisable()
    {
        onFoot.Disable();
    }
}
