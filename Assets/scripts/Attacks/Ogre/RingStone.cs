using UnityEngine;

public class RingStone : MonoBehaviour
{
    public float damage = 20f;
    public float knockupForce = 30f;

    private bool hasHit = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        if (other.CompareTag("Player"))
        {
            hasHit = true;

            Debug.Log("Ogre Rage Ring has hit " + other.name + " for " + damage.ToString() + " damage!");

            // Damage
            PlayerStats stats = other.GetComponentInChildren<PlayerStats>();
            if (stats != null)
                stats.TakeDamage(damage);

            // Knockup via the player's movement script
            PlayerMotor motor = other.GetComponentInChildren<PlayerMotor>();
            if (motor != null)
                motor.ApplyKnockup(knockupForce);
        }
    }
}