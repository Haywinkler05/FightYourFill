
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Scripts")]
    [SerializeField] private Inventory invManagement;
    [SerializeField] private IKControl iK;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private PlayerInteract interact;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private PlayerUI uI;
    [SerializeField] private PlayerMotor motor;
    [SerializeField] private PlayerLook look;
    [SerializeField] private playerCombat combat;

    [Header("Core Components")]
 
    [SerializeField] private CharacterController characterController;
    [SerializeField] private Animator animator;


    public Inventory InvManagement => invManagement;
    public IKControl IK => iK;
    public PlayerHealth Health => health;
    public PlayerInteract Interact => interact;
    public InputManager InputManager => inputManager;
    public PlayerUI UI => uI;
    public PlayerMotor Motor => motor;
    public PlayerLook Look => look;
    public playerCombat Combat => combat;

    public CharacterController CharacterController => characterController;
    public Animator Animator => animator;


}
