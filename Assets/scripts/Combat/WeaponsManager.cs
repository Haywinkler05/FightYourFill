using UnityEngine;
using System.Collections.Generic;

public class WeaponsManager : MonoBehaviour
{
    // Drag & drop an Inventory in the inspector, or it'll be found at Awake()
    public Inventory inventory;

    public float damage = 0f;
    public float atkcooldown = 0f;

    void Awake()
    {
        if (inventory == null)
        {
            // try same GameObject first
            inventory = GetComponent<Inventory>();
        }

        if (inventory == null)
        {
            // fallback to any Inventory in the scene
            inventory = FindObjectOfType<Inventory>();
        }
    }

    void Update()
    {
        if (inventory == null) return;

        // Example: read values from inventory (replace with actual Inventory members)
        // damage = inventory.weaponDamage;
        // atkcooldown = inventory.attackCooldown;
    }
}
