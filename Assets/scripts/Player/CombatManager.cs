using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CombatSystem : MonoBehaviour


{

public Transform attackPoint;
public LayerMask enemyLayers; //Used to tell the engine what is an enemy in the game

public float attackRange = 0.5f;
public int attackDamage = 40;

public Animator PlayerAnim_Controller;

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Attack();
            Debug.Log("Attack!");//LOG
        }
        
    }

    void Attack()
    {

        PlayerAnim_Controller.SetTrigger("Attack");//Plays M1 attack animation upon "Attack" parameter trigger

        //Detect enemies within attack range
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        //Damage to enemy
        foreach(Collider enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy_A>().TakeDamage(attackDamage); //Subtracts the "attackDamage" value from Enemy_A health

            Debug.Log("ENEMY HIT!!" + enemy.name);//LOG
        }
    }

void OnDrawGizmosSelected()
{
    if (attackPoint == null)
        return;

    Gizmos.color = Color.red;

    Gizmos.matrix = attackPoint.localToWorldMatrix;
    Gizmos.DrawWireSphere(Vector3.zero, attackRange);
}

}
