using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cookingMeatGrill : MonoBehaviour
{

    public GameObject meat;
    public GameObject meatCooked;
    private float x,y,z;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (meat == null) meat = gameObject;
        if (meatCooked == null) meatCooked = gameObject;
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnObj(GameObject food)
    {
        Debug.Log(x + " " + y + " " +  z + "spawn");
        Instantiate(food, new Vector3 (x,y,z), Quaternion.identity);
    }

    void cookMeat (GameObject food)
    {
        spawnObj(food);
        
    }
}
