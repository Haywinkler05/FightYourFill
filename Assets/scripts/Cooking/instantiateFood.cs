using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class instantiateFood : MonoBehaviour
{
    public GameObject food;
    public bool right;
    private float x,y,z;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //spawnObj(food1);
        if (food == null) food = gameObject;
        x = transform.position.x;
        y = transform.position.y + 0.5f;
        z = transform.position.z;

        if (right == true) x +=0.2f;
        else { x -= 0.2f;}


    }
    void Update()
    {
        
    }


    void OnMouseDown()
    {
        Debug.Log("click");
        spawnObj(food);

    }

    public void spawnObj(GameObject food)
    {
        Debug.Log(x + " " + y + " " +  z + "spawn");
        Instantiate(food, new Vector3 (x,y,z), Quaternion.identity);
    }
}
