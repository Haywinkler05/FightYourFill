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
    [SerializeField] private PlayerStats stats;
    [Header("UI")]
    [SerializeField] private GameObject damagePopupPrefab;
    private int comboStep = 0;
    private float comboTimer = 0f;
    private float lastAttackTime = -Mathf.Infinity;
    private Player player;
    private Animator animator;

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
        if (stats == null)
        {
            stats = GetComponentInParent<PlayerStats>();
            if (stats == null) stats = FindObjectOfType<PlayerStats>();
            Debug.Log("[playerCombat] stats auto-assigned: " + (stats != null));
        }
        if (damagePopupPrefab == null)
        {
            damagePopupPrefab = Resources.Load<GameObject>("DamagePopup");
            if (damagePopupPrefab != null)
                Debug.Log("[playerCombat] Loaded DamagePopup prefab from Resources.");
            else
                Debug.LogError("[playerCombat] DamagePopup prefab is not assigned and could not be loaded from Resources!");
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
        //Gets equipped item multipliers from the equipped hotbar slot
        float damageMult = 1f;
        float cooldownMult = 1f;
        ItemSO equippedSO = null;
        GameObject equippedItem = null;
        int equippedHotbarIndex = 0;
        if (inventory != null)
        {
            //Uses reflection to get equippedHotbarIndex and hotbarSlots
            var invType = inventory.GetType();
            var hotbarSlotsField = invType.GetField("hotbarSlots", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var equippedIndexField = invType.GetField("equippedHotbarIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var hotbarSlots = hotbarSlotsField?.GetValue(inventory) as System.Collections.IList;
            equippedHotbarIndex = equippedIndexField != null ? (int)equippedIndexField.GetValue(inventory) : 0;
            if (hotbarSlots != null && equippedHotbarIndex >= 0 && equippedHotbarIndex < hotbarSlots.Count)
            {
                var slot = hotbarSlots[equippedHotbarIndex];
                var slotType = slot.GetType();
                if ((bool)slotType.GetMethod("HasItem").Invoke(slot, null))
                {
                    equippedSO = (ItemSO)slotType.GetMethod("GetItem").Invoke(slot, null);
                    equippedItem = inventory.GetHandItemInstance();
                    if (equippedSO != null)
                    {
                        damageMult = equippedSO.damageMult != 0 ? equippedSO.damageMult : 1f;
                        cooldownMult = equippedSO.atkCooldownMult != 0 ? equippedSO.atkCooldownMult : 1f;
                    }
                }
            }
        }
        float effectiveCooldown = attackCooldown * cooldownMult;
        if (Time.time - lastAttackTime < effectiveCooldown)
        {
            return;
        }
        lastAttackTime = Time.time;
        comboStep = (comboTimer > 0) ? Mathf.Clamp(comboStep + 1, 1, 3) : 1;
        comboTimer = comboWindow;
        if (animator != null)
        {
            animator.SetTrigger("Attack" + comboStep);
        }
        //Attack validity checks
        //Becareful with messing with this it is very complicated -phil
        if (attackRanger == null) return;
        float range = attackRanger.attackRange;
        Transform attackPoint = attackRanger.attackPoint;
        LayerMask enemyLayers = attackRanger.enemyLayers;
        if (inventory == null) return;
        if (equippedItem == null) return;
        if (attackPoint == null) return;
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, range, enemyLayers);
        int baseDamage = attack1Damage;
        switch (comboStep)
        {
            case 2: baseDamage = attack2Damage; break;
            case 3: baseDamage = attack3Damage; break;
        }
        // Multiply base damage by item damage multiplier, then by PlayerStats damageMultiplier
        float statsDamageMultiplier = stats != null ? stats.damageMultiplier : 1f;
        int finalDamage = Mathf.RoundToInt(baseDamage * damageMult * statsDamageMultiplier);
        foreach (Collider enemyCol in hitEnemies) 
        {
            Enemy enemy = enemyCol.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(finalDamage);
                if (damagePopupPrefab != null)
                {
                    var canvas = FindObjectOfType<Canvas>();
                    GameObject popup = Instantiate(damagePopupPrefab, canvas.transform);
                    var popupScript = popup.GetComponent<DamagePopup>();
                    if (popupScript != null)
                    {
                        popupScript.Setup(finalDamage, enemy.transform.position);
                    }
                }
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