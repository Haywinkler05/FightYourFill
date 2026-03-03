using UnityEngine;
using UnityEngine.InputSystem;

public class cookingCamera : MonoBehaviour
{
    [SerializeField]
    private InputAction action;

    private Animator animator;

    private bool camera1 = true;
    private bool camera2 = false;
    private bool camera3 = false;


    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
// if using action input
    private void OnEnable() {
        action.Enable();
    }
    private void OnDisable() {
        action.Disable();
    }

    void cameraPriority(bool camera){
        camera1 = false;
        camera2 = false;
        camera3 = false;
    }
}
