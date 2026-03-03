using UnityEditor.Build;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Scripts")]
    [SerializeField] protected Inventory invManagment;
    [SerializeField] protected IKControl IK;
    [SerializeField] protected PlayerHealth health;
    [SerializeField] protected PlayerInteract interact;
    [SerializeField] protected InputManager InputManager;
    [SerializeField] protected PlayerUI UI;
    [SerializeField] protected PlayerMotor motor;
    [SerializeField] protected PlayerLook look;
    [SerializeField] protected CombatSystem combat;
}
