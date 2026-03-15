using UnityEngine;
using UnityEngine.UI;


public class customerScript : MonoBehaviour
{

    // public int diffRec = 2;
    public int diffMeat = 2; // min 2, max 6
    public int diffExtra = 1; // disable=0, def=1, max 5
    public int recipeNum, meatNum;
    public int extraNum1, extraNum2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //testing

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void randomOrder()
    {
        recipeNum = Random.Range(0,3);
        Debug.Log(recipeNum + "recipe number");

        meatNum = Random.Range(0,6)%diffMeat;
        if (meatNum > diffMeat)
        {
            //keep random order meat in range of what player has
        }
        Debug.Log(meatNum + "meat number");

        float extraIf = Random.Range(0.0f,1.0f);
        Debug.Log(extraIf);
        if (extraIf <= 0.2f*diffExtra)
        {
            extraNum1 = Random.Range(0,4);
            Debug.Log(extraNum1 + "extra num1");
        }
    }

    




}
