using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class AttackRanger : MonoBehaviour


{

public Transform attackPoint;
public LayerMask enemyLayers; //Used to tell the engine what is an enemy in the game

public float attackRange = 1.5f;

public Animator PlayerAnim_Controller;

    // Update is called once per frame
    void Update()
    {
        
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
