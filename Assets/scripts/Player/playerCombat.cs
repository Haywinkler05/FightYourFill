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

    [Header("References")]
    [SerializeField] private AttackRanger attackRanger; 
    [SerializeField] private Inventory inventory;

    private int comboStep = 0;
    private float comboTimer = 0f;
    private float lastAttackTime = -Mathf.Infinity;

    private Player player;
    private Animator animator;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GetComponentInParent<Player>();
        animator = player != null ? player.Animator : GetComponent<Animator>();
        if (attackRanger == null)
        {
            attackRanger = FindObjectOfType<AttackRanger>();
            Debug.Log("[playerCombat] attackRanger auto-assigned: " + (attackRanger != null));
        }
        if (inventory == null)
        {
            inventory = FindObjectOfType<Inventory>();
            Debug.Log("[playerCombat] inventory auto-assigned: " + (inventory != null));
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer < 0f) comboStep = 0;
        }
    }

    public void basicAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown)
        {
            Debug.Log("[playerCombat] Attack on cooldown.");
            return;
        }

        lastAttackTime = Time.time;
        comboStep = (comboTimer > 0) ? Mathf.Clamp(comboStep + 1, 1, 3) : 1;
        Debug.Log("[playerCombat] Current Combo Step: " + comboStep);
        comboTimer = comboWindow;
        if (animator != null)
        {
            animator.SetTrigger("Attack" + comboStep);
        }
        else
        {
            Debug.LogWarning("[playerCombat] Animator not found!");
        }

        //Steps for an attack to be valid. There is a lot of logs here for debugging
        //as this is complicated and can break easily. -phil
        //1. Read attack range and attack point from AttackRanger
        if (attackRanger == null)
        {
            Debug.LogWarning("[playerCombat] AttackRanger reference missing!");
            return;
        }
        float range = attackRanger.attackRange;
        Transform attackPoint = attackRanger.attackPoint;
        LayerMask enemyLayers = attackRanger.enemyLayers;
        Debug.Log($"[playerCombat] Attack range: {range}, attackPoint: {attackPoint}, enemyLayers: {enemyLayers}");

        //2. Check if player has an equipped item
        if (inventory == null)
        {
            Debug.LogWarning("[playerCombat] Inventory reference missing!");
            return;
        }
        var equippedItem = inventory.GetHandItemInstance();
        if (equippedItem == null)
        {
            Debug.Log("[playerCombat] No equipped item, attack aborted.");
            return;
        }
        Debug.Log($"[playerCombat] Equipped item: {equippedItem.name}");

        //3. Detect enemies in range
        if (attackPoint == null)
        {
            Debug.LogWarning("[playerCombat] Attack point is null!");
            return;
        }
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, range, enemyLayers);
        Debug.Log($"[playerCombat] Enemies hit: {hitEnemies.Length}");

        //4. Apply damage to each enemy
        int damage = attack1Damage;
        switch (comboStep) //applies damage based on what combo the attack is on
        {
            case 2: damage = attack2Damage; break;
            case 3: damage = attack3Damage; break;
        }
        foreach (Collider enemyCol in hitEnemies) 
        {
            Enemy enemy = enemyCol.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                Debug.Log($"[playerCombat] Damaging enemy: {enemy.name} for {damage} HP");
                enemy.TakeDamage(damage);
            }
            else
            {
                Debug.Log($"[playerCombat] Collider {enemyCol.name} does not have Enemy component.");
            }
        }
    }

    //This is for the debug sphere to visualize the attack range
    void OnDrawGizmosSelected()
    {
        if (attackRanger != null && attackRanger.attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = attackRanger.attackPoint.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, attackRanger.attackRange);
        }
    }
}
