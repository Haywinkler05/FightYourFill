using UnityEngine;
using UnityEngine.InputSystem;

public class cookingCamera : MonoBehaviour
{
    [SerializeField]
    private InputAction action1;
    [SerializeField]
    private InputAction action2;

    private Animator animator;

    public static cookingCamera refCam;


    void Awake()
    {
        animator = GetComponent<Animator>();
        Debug.Log("Camera Animator got");
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        refCam = this;
        //record if action is pressed.
        //swaps to cam0/main
        action1.performed += _ => cameraPriority(0);
        //swap to cam2/grill
        action2.performed += _ => cameraPriority(1);

        //switch to ui buttons later when grilling is complete or automate when grilling is complete.

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        action1.Enable();
        action2.Enable();
    }
    private void OnDisable() {
        action1.Disable();
        action2.Disable();
    }

    public void cameraPriority(int cam){
        if(cam == 0)
        {
            animator.Play("Cam0");
            Debug.Log("Camera swap 0");
        }
        else if(cam == 1)
        {
            animator.Play("Cam1");
            Debug.Log("Camera swap 1");
        }
        else if(cam == 2)
        {
            animator.Play("Cam2");
            Debug.Log("Camera swap 1");
        }
        else
        {
            animator.Play("Cam0");
            Debug.Log("Camera swap fail");
        }
    }
}
