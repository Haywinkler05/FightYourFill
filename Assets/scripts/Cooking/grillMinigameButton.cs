using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;



//this script kinda literally just swaps the sprite to show a button is pressed.
public class grillMinigame : MonoBehaviour
{
    private Image img;
    public Sprite defaultSprite;
    public Sprite pressedSprite;
    public AudioSource hitSound;

    [SerializeField]
    private InputAction action;

    //public KeyCode keyPress;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        img = GetComponent<Image>();
        if(hitSound == null) hitSound = GetComponent<AudioSource>();
        
        //button down/on
        action.performed += _ => pressedSwap();
        //button up/off
        action.canceled += _ => defaultSwap();
    
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable() {
        action.Enable();
    }
    private void OnDisable() {
        action.Disable();
    }

    void pressedSwap()
    {
        img.sprite = pressedSprite;
        img.color = Color.white;
        //play sound when pressed
        hitSound.Play();
        //plays only one sound if multiple buttons held/fix by separating actions possibly
        //Debug.Log("hitboxPressed");
    }

    void defaultSwap()
    {
        img.sprite = defaultSprite;
        img.color = Color.black;

        //Debug.Log("hitboxDefault");
    }
}
