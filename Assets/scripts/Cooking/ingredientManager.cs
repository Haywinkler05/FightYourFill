using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ingredientManager : MonoBehaviour
{

    bool spawnDone;
    public int recipeNum; 
    public int ingredientNum;

    public GameObject confirmButton;
    public GameObject bread,stick,bowl;
    public GameObject meat1,meat2,meat3,meat4,meat5,meat6;
    public GameObject rmeat1,rmeat2,rmeat3,rmeat4,rmeat5,rmeat6;
    public GameObject extra1,extra2,extra3,extra4;
    public GameObject spawner;
    private List<GameObject> foodList = new List<GameObject>();
    // private List<GameObject> recipeList = new List<GameObject>();
    private List<GameObject> meatList = new List<GameObject>();
    private List<GameObject> rMeatList = new List<GameObject>();
    private List<GameObject> cloneList = new List<GameObject>();
    private Vector3[] coords = new Vector3[6];

    public static ingredientManager refI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        refI = this;

        //spawn locations of ingredients/max 6
        coords[0] = new Vector3(-13.7f,4f,-1f);
        coords[1] = new Vector3(-14.7f,4f,-1f);
        coords[2] = new Vector3(-15.7f,4f,-1f);
        coords[3] = new Vector3(-13.7f,4f,2.5f);
        coords[4] = new Vector3(-14.7f,4f,2.5f);
        coords[5] = new Vector3(-15.7f,4f,2.5f);

        rMeatList.Add(rmeat1);
        rMeatList.Add(rmeat2);
        rMeatList.Add(rmeat3);
        rMeatList.Add(rmeat4);
        rMeatList.Add(rmeat5);
        rMeatList.Add(rmeat6);

        meatList.Add(meat1);
        meatList.Add(meat2);
        meatList.Add(meat3);
        meatList.Add(meat4);
        meatList.Add(meat5);
        meatList.Add(meat6);
    }

    // Update is called once per frame
    void Update()
    {
        /* if(grillMinigameManager.instance.finishPlay == true && spawnDone == false)
        {
            
            Debug.Log("grill finished/starting ingredient assembly");
            
            updateIngredientList();
            spawnIngredients();
        }  */
    }

    void spawnIngredient(GameObject food, Vector3 pos)
    {
        GameObject clone = Instantiate(food, pos, Quaternion.identity);
        cloneList.Add(clone);
    }

    public void spawnIngredients()
    {
        spawnDone = true;
        Debug.Log("Spawned" + recipeNum + " " + ingredientNum);
        for(int i = 0; i < foodList.Count; i++)
        {
            spawnIngredient(foodList[i], coords[i]);
            
        }
        foodList.Clear();
    } 

    public void removeClones()
    {
        for(int i = 0; i < cloneList.Count; i++)
        {
            Destroy(cloneList[i]);    
        }
        spawnDone = false;
    }

    

    void spawnMainContainer(int num)
    {
        //spawn the main container for the recipe eg. bowl, bread, stick
        if(num == 0)
        {
            spawnIngredient(bowl, new Vector3(-14.7f,2.8f,.7f));
        }
        else
        {
            spawnIngredient(stick, new Vector3(-14.7f,2.8f,.7f));
        }

    }

    public GameObject spawnGrillMeat()
    {
        GameObject meat = rMeatList[ingredientNum];
        GameObject obj = Instantiate(meat, Vector3.zero, Quaternion.identity, spawner.transform);
        obj.transform.localPosition = Vector3.zero;
        return obj;
    }

    public void setIngNum(int num)
    {
        ingredientNum = num;
    }

    public void setRecNum(int num)
    {
        recipeNum = num;
    }

    public void updateIngredientList()
    {
        removeClones();
        foodList.Add(meatList[ingredientNum]);
        if(recipeNum == 1)
        {
            foodList.Add(bread);
            foodList.Add(bread);
        }
        else if (recipeNum == 2)
        {
            //spawn bowl in middle
            spawnMainContainer(0);
        }
        else
        {
            //spawn stick straight up in middle
            spawnMainContainer(1);
        }
    }

}
