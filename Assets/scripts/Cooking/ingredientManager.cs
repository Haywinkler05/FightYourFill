using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ingredientManager : MonoBehaviour
{

    bool spawnDone;

    public List<GameObject> foodList = new List<GameObject>();
    private Vector3[] coords = new Vector3[6];

    public static ingredientManager refI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        refI = this;
        //spawn locations of ingredients/max 6
        coords[0] = new Vector3(3f,7f,-5f);
        coords[1] = new Vector3(0f,7f,-5f);
        coords[2] = new Vector3(-3f,7f,-5f);
        coords[3] = new Vector3(3f,7f,5f);
        coords[4] = new Vector3(0f,7f,5f);
        coords[5] = new Vector3(-3f,7f,5f);

    }

    // Update is called once per frame
    void Update()
    {

        
            if(grillMinigameManager.instance.finishPlay == true && spawnDone == false)
        {
            spawnDone = true;
            Debug.Log("grill finished/starting ingredient assembly");
            spawnIngredients();
        }
        else
        {
            
        }
        
        
        
    }

    void spawnIngredient(GameObject food, Vector3 pos)
    {
        GameObject clone = Instantiate(food, pos, Quaternion.identity);

        //add drag component if needed
        //clone.AddComponent<DragIngredient>();
        //clone.GetComponent<DragIngredient>().yPos = 8f;
        //also add rigidbody and collider later if needed
    }

    void spawnIngredients()
    {
        for(int i = 0; i < foodList.Count; i++)
        {
            spawnIngredient(foodList[i], coords[i]);   
        }
    } 
}
