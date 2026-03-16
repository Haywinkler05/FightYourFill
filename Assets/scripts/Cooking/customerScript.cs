using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class customerScript : MonoBehaviour
{
    bool orderProgress;
    

    // public int diffRec = 2;
    public int diffMeat = 6; // min 2, max 6
    public int diffExtra = 1; // disable=0, def=1, max 5
    public int recipeNum, meatNum;
    public int rINum, mINum;
    public int extraNum1, extraNum2;

//customer dialogue
    public GameObject bubble;
    public TMP_Text customerText;

//imgs of meat
    public GameObject zombieSausage;
    public GameObject spiderFilet;
    public GameObject yetiRibs;
    public GameObject orcMeat;
    public GameObject golemMeat;
//imgs of extra ingredients
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //testing
        randomOrder();
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void randomOrder()
    {
        orderProgress = true;
        recipeNum = Random.Range(0,3);
        meatNum = Random.Range(0,5)%diffMeat;
        if (meatNum > diffMeat)
        {
            //keep random order meat in range of what player has
        }
        Debug.Log(meatNum + recipeNum + " meat and recipe number");
        order1Text();

        //extra stuff 

        /* float extraIf = Random.Range(0.0f,1.0f);
        Debug.Log(extraIf);
        if (extraIf <= 0.2f*diffExtra)
        {
            extraNum1 = Random.Range(0,4);
            Debug.Log(extraNum1 + "extra num1");
        } */


    }

    void order1Text()
    {
        bubble.SetActive(true);
        switch(recipeNum)
        {
            case 0:
            //kebab
            customerText.text = "I would like a kebab with:";
            break;
            case 1:
            //sandwich
            customerText.text = "I would like a sandwich with:";
            break;
            case 2:
            //soup
            customerText.text = "I would like soup with:";
            break;
        }
        switch (meatNum)
        {
            case 0:
            zombieSausage.SetActive(true);
            break;
            case 1:
            orcMeat.SetActive(true);
            break;
            case 2:
            spiderFilet.SetActive(true);
            break;
            case 3:
            yetiRibs.SetActive(true);
            break;
            case 4:
            golemMeat.SetActive(true);
            break;
        }
    }

    public void resetCustomer()
    {
        customerText.text = "";
        bubble.SetActive(false);

        zombieSausage.SetActive(false);
        orcMeat.SetActive(false);
        spiderFilet.SetActive(false);
        yetiRibs.SetActive(false);
        golemMeat.SetActive(false);

        ingredientManager.refI.removeClones();
        recipeMenu.refR.resetMenu();

        randomOrder();
        confirmOrder();
    }

    public void confirmOrder()
    {
        //check if order right.
        rINum = ingredientManager.refI.recipeNum;
        mINum = ingredientManager.refI.ingredientNum;

        //if order right then full money(score)
        if(rINum == recipeNum)
        {
            //give player money equal score from grillMinigameManager
            Debug.Log("Recipe is correct!!!");
            if(mINum == meatNum)
            {
                Debug.Log("Meat is correct!!!");
            }
        }
        else
        {
            //give player fraciton of money(6/10)
            Debug.Log("Order is wrong!!!");
        }

        //reset Nums
        rINum = -1;
        mINum = -1;
        
    }

    






}
