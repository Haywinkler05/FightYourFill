using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;




public class grillMinigame : MonoBehaviour
{
    private Image img;
    public Sprite defaultSprite;
    public Sprite pressedSprite;

    public KeyCode keyPress;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyPress))
        {
            img.sprite = pressedSprite;
            img.color = Color.white;
        }

        if (Input.GetKeyUp(keyPress))
        {
            img.sprite = defaultSprite;
            img.color = Color.black;
        }
        
    }
}
