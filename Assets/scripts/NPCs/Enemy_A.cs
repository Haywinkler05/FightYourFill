using UnityEngine;

//this is a work in progress -phil
public class Enemy_A : MonoBehaviour
{

    public int maxHealth = 100;//THIS IS IN DEBUG VIEW OF AN OBJECT WITH THIS SCRIPT ATTACHED TO IT!!!!!!
    int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;   
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die() //basic for now for testing
    {
        Debug.Log("ENEMY_A HAS DIED!!!!!!!"); //LOG
    }

}
