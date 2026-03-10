using UnityEngine;

public class playerCombat : MonoBehaviour
{

    [Header("Combo Settings")]
    [SerializeField] private float comboWindow = 0.8f;
    [SerializeField] private float attackCooldown = 0.3f;

    [Header("Damage")]
    [SerializeField] private int attack1Damage = 10;
    [SerializeField] private int attack2Damage = 15;
    [SerializeField] private int attack3Damage = 25;


    private int comboStep = 0;
    private float comboTimer = 0f;
    private float lastAttackTime = -Mathf.Infinity;

    private Player player;
    private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInParent<Player>();
        animator = player.Animator;
    }

    // Update is called once per frame
    void Update()
    {
        if (comboTimer > 0f) {

            comboTimer -= Time.deltaTime;
            if(comboTimer < 0f) comboStep = 0;
                
           }
    }

    public void basicAttack()
    {
        
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;

        
        comboStep = (comboTimer > 0) ? Mathf.Clamp(comboStep + 1, 1, 3) : 1;
        Debug.Log("Current Combo Step: " + comboStep);
        comboTimer = comboWindow;

        animator.SetTrigger("Attack" + comboStep);
       
    }
}
