using UnityEngine;


public class Enemy : MonoBehaviour
{
    public float Health { get; protected set; }
    public float Damage { get; protected set; }
    public float Drop { get; protected set; }
    public float SightRange { get; protected set; }

    public virtual void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health < 0)
        {
            Die();
        }

    }
    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
