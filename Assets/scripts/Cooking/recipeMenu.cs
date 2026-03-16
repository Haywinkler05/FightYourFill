using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class recipeMenu : MonoBehaviour
{
    
    public int recipeNum;
    public TMP_Text recipeDesc;
    public TMP_Text recipeName;

    public int ingredientNum;
    public TMP_Text ingredientDesc;
    public TMP_Text ingredientName;

    public GameObject selectScreen;
    public GameObject ingredientAssets;
    public GameObject nextButton;
    bool screenOn;
    bool buttonListening = true;

    [SerializeField]
    private InputAction actionSelect;

    public static recipeMenu refR;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        refR = this;

        Cursor.visible = true;
        actionSelect.performed += _ => screenToggle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable() {
        actionSelect.Enable();
    }
    private void OnDisable() {
        actionSelect.Disable();
    }

    public void updateRecipeText(int num)
    {
        recipeNum = num;
        ingredientManager.refI.setRecNum(num);
        switch (num)
        {
            case 0:
            //meat/kebab
            recipeName.text = "Kebab";
            recipeDesc.text = "Somtimes all you need is just a plain slab of meat grilled to perfection.";
            break;
            case 1:
            //sandwich
            recipeName.text = "Sandwich";
            recipeDesc.text = "It's just a normal sandwich. . . right?";
            break;
            case 2:
            //soup
            recipeName.text = "Soup";
            recipeDesc.text = "Savory soup made with only the best ingredients. . . found off the ground. . . from monsters. . .";
            break;
            default:
            //default
            recipeName.text = "Recipe Book";
            recipeDesc.text = "Recipes made by the greatest chef to ever live - Grandon Ramsworth";
            break;
        }
    }

    public void updateIngredientText(int num)
    {
        ingredientNum = num;
        ingredientManager.refI.setIngNum(num);
        switch (num)
        {
        case 0:
        //zombie sausage
        ingredientName.text = "Zombie Sausage";
        ingredientDesc.text = "An astonishingly delectable piece of meat from locally sourced Zombies.";
        break;
        case 1:
        //sandwich
        ingredientName.text = "Orc Meat";
        ingredientDesc.text = "Ethically Grey zone of edible meats. But hey, it tastes like bacon!";
        break;
        case 2:
        //soup
        ingredientName.text = "Spider Filet";
        ingredientDesc.text = "Do not tell the customers where this filet came from.";
        break;
        case 3:
        //soup
        ingredientName.text = "Yeti Ribs";
        ingredientDesc.text = "A fan favourite of customers across the globe!";
        break;
        case 4:
        //soup
        ingredientName.text = "Golem Meat";
        ingredientDesc.text = "What was supposedly a moving rock, provides some of the most delicious meat humanity has ever set their hands upon.";
        break;
        case 5:
        //soup
        ingredientName.text = "???";
        ingredientDesc.text = "??????";
        break;
        default:
        //fall back to meat
        ingredientName.text = "Ingredients";
        ingredientDesc.text = "";
        break;
        }

    }

    public void screenToggle()
    {
        //Debug.Log(screenOn + "screen on");
        if (!screenOn)
        {
            selectScreen.SetActive(true);
            screenOn = true;
        }
        else
        {
            selectScreen.SetActive(false);
            screenOn = false;
        }

    }

    public void setScreenBool(bool value)
    {
        // for button
        screenOn = value;
    }

    public void toggleButtonListen()
    {
        Debug.Log(buttonListening + " recipe button listening");
        if (!buttonListening)
        {
            actionSelect.Enable();
            buttonListening = true;
        }
        else
        {
            actionSelect.Disable();
            buttonListening = false;
        }
    }

    public void resetMenu()
    {
        ingredientAssets.SetActive(false);
        nextButton.SetActive(false);
        updateRecipeText(-1);
        updateIngredientText(-1);
        
    }



}
