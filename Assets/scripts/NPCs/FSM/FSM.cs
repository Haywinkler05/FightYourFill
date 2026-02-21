using UnityEngine;
using UnityEngine.AI;

public abstract class FSM : Enemy //Takes from the enemy class
{
    protected IState currentState; //Every child can access the current state
    

    protected virtual void Start() //Will call an intialize states all children should have and enter the current state
    {
        intializeStates();
        currentState.onEnter();
    }

    protected abstract void intializeStates(); //Requires all children to have this function
    public void SetState(IState newState) //Changes the state
    {
        currentState.onExit();
        currentState = newState;
        currentState.onEnter(); 
       
    }


    protected virtual void Update() //Calls the update function of the state
    {
        currentState.update();
    }
}
