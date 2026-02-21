using UnityEngine;


public class Enemy : MonoBehaviour
{
    public float Health { get; protected set; } //Getters and setters for each
    public float Damage { get; protected set; }
    public GameObject Drop { get; protected set; }
    public float SightRange { get; protected set; }

    public virtual void TakeDamage(float damage) //Makes a take damage function that can be modifed due to virtual
    {
        Health -= damage;
        if (Health < 0)
        {
            Die();
        }

    }
    protected virtual void Die() //Destroys the game object but also can be modifed
    {
        Destroy(gameObject);
    }
}
