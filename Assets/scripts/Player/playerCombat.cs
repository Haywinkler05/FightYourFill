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
    // Serialized so it can be assigned in the Inspector and survives scene loads.
    [SerializeField] private PlayerStats stats;

    [Header("UI")]
    [SerializeField] private GameObject damagePopupPrefab;

    private int   comboStep       = 0;
    private float comboTimer      = 0f;
    private float lastAttackTime  = -Mathf.Infinity;
    private Player   player;
    private Animator animator;

    void Start()
    {
        player   = GetComponentInParent<Player>();
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

        // Try parent first (same GameObject hierarchy), then scene-wide fallback.
        if (stats == null && player != null)
            stats = player.Health;                       // Player.Health exposes PlayerStats
        if (stats == null)
            stats = GetComponentInParent<PlayerStats>();
        if (stats == null)
            stats = FindObjectOfType<PlayerStats>();

        if (stats == null)
            Debug.LogError("[playerCombat] Could not find PlayerStats! Damage multiplier will default to 1.");
        else
            Debug.Log("[playerCombat] stats assigned: " + stats.gameObject.name);

        if (damagePopupPrefab == null)
        {
            damagePopupPrefab = Resources.Load<GameObject>("DamagePopup");
            if (damagePopupPrefab != null)
                Debug.Log("[playerCombat] Loaded DamagePopup prefab from Resources.");
            else
                Debug.LogError("[playerCombat] DamagePopup prefab not assigned and not found in Resources!");
        }
    }

    void Update()
    {
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f) comboStep = 0;
        }
    }

    public void basicAttack()
    {
        // ---- Read equipped item multipliers via reflection ----
        float    damageMult   = 1f;
        float    cooldownMult = 1f;
        ItemSO   equippedSO   = null;
        GameObject equippedItem = null;

        if (inventory != null)
        {
            var invType            = inventory.GetType();
            var hotbarSlotsField   = invType.GetField("hotbarSlots",        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var equippedIndexField = invType.GetField("equippedHotbarIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var hotbarSlots        = hotbarSlotsField?.GetValue(inventory) as System.Collections.IList;
            int equippedHotbarIndex = equippedIndexField != null ? (int)equippedIndexField.GetValue(inventory) : 0;

            if (hotbarSlots != null && equippedHotbarIndex >= 0 && equippedHotbarIndex < hotbarSlots.Count)
            {
                var slot     = hotbarSlots[equippedHotbarIndex];
                var slotType = slot.GetType();
                if ((bool)slotType.GetMethod("HasItem").Invoke(slot, null))
                {
                    equippedSO   = (ItemSO)slotType.GetMethod("GetItem").Invoke(slot, null);
                    equippedItem = inventory.GetHandItemInstance();
                    if (equippedSO != null)
                    {
                        damageMult   = equippedSO.damageMult      != 0 ? equippedSO.damageMult      : 1f;
                        cooldownMult = equippedSO.atkCooldownMult != 0 ? equippedSO.atkCooldownMult : 1f;
                    }
                }
            }
        }

        // ---- Cooldown check ----
        float effectiveCooldown = attackCooldown * cooldownMult;
        if (Time.time - lastAttackTime < effectiveCooldown) return;

        lastAttackTime = Time.time;
        comboStep  = (comboTimer > 0f) ? Mathf.Clamp(comboStep + 1, 1, 3) : 1;
        comboTimer = comboWindow;

        if (animator != null)
            animator.SetTrigger("Attack" + comboStep);

        // ---- Attack validity checks ----
        // NOTE: keep this ordering — early returns are intentional.
        if (attackRanger == null)  return;
        if (attackRanger.attackPoint == null) return;
        if (inventory    == null)  return;
        if (equippedItem == null)  return;

        float     range       = attackRanger.attackRange;
        Transform attackPoint = attackRanger.attackPoint;
        LayerMask enemyLayers = attackRanger.enemyLayers;

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, range, enemyLayers);

        // ---- Calculate final damage ----
        int baseDamage = attack1Damage;
        switch (comboStep)
        {
            case 2: baseDamage = attack2Damage; break;
            case 3: baseDamage = attack3Damage; break;
        }

        // Apply both the item damage multiplier AND the player stat damage multiplier.
        float statsDamageMultiplier = (stats != null) ? stats.damageMultiplier : 1f;
        int   finalDamage           = Mathf.RoundToInt(baseDamage * damageMult * statsDamageMultiplier);

        Debug.Log($"[playerCombat] Attack{comboStep} | base:{baseDamage} x item:{damageMult} x stats:{statsDamageMultiplier} = {finalDamage}");

        // ---- Apply damage and spawn popups ----
        foreach (Collider enemyCol in hitEnemies)
        {
            Enemy enemy = enemyCol.GetComponentInParent<Enemy>();
            if (enemy == null) continue;

            enemy.TakeDamage(finalDamage);

            if (damagePopupPrefab != null)
            {
                var canvas = FindObjectOfType<Canvas>();
                if (canvas == null) continue;
                GameObject popup      = Instantiate(damagePopupPrefab, canvas.transform);
                DamagePopup popupScript = popup.GetComponent<DamagePopup>();
                if (popupScript != null)
                    popupScript.Setup(finalDamage, enemy.transform.position);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackRanger != null && attackRanger.attackPoint != null)
        {
            Gizmos.color  = Color.red;
            Gizmos.matrix = attackRanger.attackPoint.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, attackRanger.attackRange);
        }
    }
}