using UnityEngine;

public class playerCombat : MonoBehaviour
{
    [Header("Combo Settings")]
    [SerializeField] private float comboWindow = 0.8f;
    // Base time the player must wait between individual attacks in a combo chain.
    // The weapon's atkCooldownMult is multiplied into this each attack.
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

    private int   comboStep      = 0;
    private float comboTimer     = 0f;
    private float lastAttackTime = -Mathf.Infinity;
    // Stores the effective cooldown used for the last attack so the combo
    // window can be set relative to it — prevents the window expiring before
    // the cooldown does on fast or slow weapons.
    private float lastEffectiveCooldown = 0f;

    private Player   player;
    private Animator animator;

    // All attack trigger names in the Animator. Stale triggers are always
    // cleared before a new one is set to prevent state machine queuing bugs.
    private static readonly string[] attackTriggers = { "Attack1", "Attack2", "Attack3" };

    // -----------------------------------------------------------------------
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

        // Try the Player reference first (most reliable), then fall back.
        if (stats == null && player != null)
            stats = player.Health;
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

    // -----------------------------------------------------------------------
    void Update()
    {
        if (comboTimer > 0f)
        {
            comboTimer -= Time.deltaTime;
            if (comboTimer <= 0f)
            {
                // Combo window expired — reset the chain and tell the animator
                // the player is no longer in an attack sequence.
                comboStep = 0;
                if (animator != null)
                    animator.SetBool("isAttacking", false);
            }
        }
    }

    // -----------------------------------------------------------------------
    // Reads the equipped item's multipliers from Inventory via reflection.
    // Returns true if an item is equipped, false otherwise.
    private bool TryGetEquippedMultipliers(out float damageMult, out float cooldownMult, out GameObject equippedItem)
    {
        damageMult   = 1f;
        cooldownMult = 1f;
        equippedItem = null;

        if (inventory == null) return false;

        var invType            = inventory.GetType();
        var hotbarSlotsField   = invType.GetField("hotbarSlots",         System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var equippedIndexField = invType.GetField("equippedHotbarIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var hotbarSlots        = hotbarSlotsField?.GetValue(inventory) as System.Collections.IList;
        int equippedIndex      = equippedIndexField != null ? (int)equippedIndexField.GetValue(inventory) : 0;

        if (hotbarSlots == null || equippedIndex < 0 || equippedIndex >= hotbarSlots.Count)
            return false;

        var slot     = hotbarSlots[equippedIndex];
        var slotType = slot.GetType();

        if (!(bool)slotType.GetMethod("HasItem").Invoke(slot, null))
            return false;

        ItemSO equippedSO = (ItemSO)slotType.GetMethod("GetItem").Invoke(slot, null);
        equippedItem      = inventory.GetHandItemInstance();

        if (equippedSO != null)
        {
            damageMult   = equippedSO.damageMult      != 0f ? equippedSO.damageMult      : 1f;
            cooldownMult = equippedSO.atkCooldownMult != 0f ? equippedSO.atkCooldownMult : 1f;
        }

        return true;
    }

    // -----------------------------------------------------------------------
    // Clears every attack trigger then sets only the desired one.
    // Prevents the Animator from queuing multiple triggers and freezing.
    private void SetAttackTrigger(int step)
    {
        if (animator == null) return;
        foreach (string t in attackTriggers)
            animator.ResetTrigger(t);
        animator.SetBool("isAttacking", true);
        animator.SetTrigger("Attack" + step);
    }

    // -----------------------------------------------------------------------
    public void basicAttack()
    {
        // ---- Read equipped item multipliers ----
        TryGetEquippedMultipliers(out float damageMult, out float cooldownMult, out GameObject equippedItem);

        // ---- Combo timing calculations ----
        //
        // effectiveCooldown    = how long the player must wait before this
        //                        attack registers (base cooldown x weapon mult).
        //
        // effectiveComboWindow = how long after this attack the player can press
        //                        again to continue the chain. Always kept at
        //                        least (effectiveCooldown + a small buffer) so
        //                        a fast weapon cannot collapse the window to zero,
        //                        and at least the base comboWindow so a slow
        //                        weapon still gives the player time to react.
        //
        float effectiveCooldown    = attackCooldown * cooldownMult;
        float effectiveComboWindow = Mathf.Max(comboWindow * cooldownMult, effectiveCooldown + 0.05f);

        // ---- Cooldown gate ----
        float timeSinceLast = Time.time - lastAttackTime;
        if (timeSinceLast < effectiveCooldown)
        {
            Debug.Log($"[playerCombat] Attack blocked by cooldown. " +
                      $"TimeSinceLast: {timeSinceLast:F3}s | " +
                      $"EffectiveCooldown: {effectiveCooldown:F3}s | " +
                      $"Remaining: {(effectiveCooldown - timeSinceLast):F3}s");
            return;
        }

        // ---- Advance combo step ----
        // After step 3 the combo always resets to 1 regardless of how quickly
        // the player attacks. This is both the animation fix (the animator
        // cannot re-enter a state it is already in via the same trigger) and
        // the balance fix (combo 3 cannot be spammed indefinitely).
        if (comboTimer > 0f && comboStep < 3)
        {
            // Still inside the combo window and haven't hit step 3 yet — advance.
            comboStep++;
        }
        else
        {
            // Window expired OR player just finished step 3 — restart from 1.
            // Briefly set isAttacking false so the animator gets a clean exit
            // signal before re-entering Attack1, preventing the motionless bug.
            comboStep  = 0;
            comboTimer = 0f;
            if (animator != null)
                animator.SetBool("isAttacking", false);
            comboStep = 1;
        }
        comboTimer            = effectiveComboWindow;
        lastAttackTime        = Time.time;
        lastEffectiveCooldown = effectiveCooldown;

        Debug.Log($"[playerCombat] Attack{comboStep} registered. " +
                  $"BaseCooldown: {attackCooldown:F3}s | " +
                  $"WeaponMult: {cooldownMult:F2}x | " +
                  $"EffectiveCooldown: {effectiveCooldown:F3}s | " +
                  $"ComboWindow: {effectiveComboWindow:F3}s");

        // ---- Trigger animation (clears stale triggers first) ----
        SetAttackTrigger(comboStep);

        // ---- Attack validity checks ----
        if (attackRanger == null || attackRanger.attackPoint == null) return;
        if (inventory    == null) return;
        if (equippedItem == null) return;

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

        float statsDamageMultiplier = (stats != null) ? stats.damageMultiplier : 1f;
        int   finalDamage           = Mathf.RoundToInt(baseDamage * damageMult * statsDamageMultiplier);

        Debug.Log($"[playerCombat] Damage — base:{baseDamage} x item:{damageMult:F2} x stats:{statsDamageMultiplier:F2} = {finalDamage}");

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
                GameObject  popup       = Instantiate(damagePopupPrefab, canvas.transform);
                DamagePopup popupScript = popup.GetComponent<DamagePopup>();
                if (popupScript != null)
                    popupScript.Setup(finalDamage, enemy.transform.position);
            }
        }
    }

    // -----------------------------------------------------------------------
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