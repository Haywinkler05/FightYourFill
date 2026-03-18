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

        Debug.Log("Customer orders" + recipeNum + " " + meatNum);

        if (meatNum > diffMeat)
        {
            //keep random order meat in range of what player has
        }
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

        confirmOrder();
        ingredientManager.refI.removeClones();
        recipeMenu.refR.resetMenu();
        randomOrder();
        
    }

    public void confirmOrder()
    {
        //check if order right.
        rINum = ingredientManager.refI.recipeNum;
        mINum = ingredientManager.refI.ingredientNum;

        //if order right then full money(score)
        //Debug.Log("Checking order" + rINum + " " + mINum);
        if(rINum == recipeNum)
        {
            //give player money equal score from grillMinigameManager
            Debug.Log("Recipe is correct!!!");
            
            if(mINum == meatNum)
            {
                Debug.Log("Meat is correct!!!");
                //100% money
                moneySystemCooking.refMoney.updateTotal(grillMinigameManager.instance.getScore());

            }
            else
            {
                Debug.Log("Meat is wrong!!!");
                //80% score
                moneySystemCooking.refMoney.updateTotal(grillMinigameManager.instance.getScore()*8/10);
            }
        }
        else
        {
            //60% score
            Debug.Log("Order is wrong!!!");
            moneySystemCooking.refMoney.updateTotal(grillMinigameManager.instance.getScore()*6/10);
        }

        //reset Nums
        rINum = -1;
        mINum = -1;
        
    }

    






}
