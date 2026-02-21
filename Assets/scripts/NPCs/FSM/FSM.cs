using UnityEngine;
using UnityEngine.AI;

public abstract class FSM : Enemy
{
    protected IState currentState;
    

    protected virtual void Start()
    {
        intializeStates();
        currentState.onEnter();
    }

    protected abstract void intializeStates();
    public void SetState(IState newState)
    {
        currentState.onExit();
        currentState = newState;
        currentState.onEnter(); 
       
    }


    protected virtual void Update()
    {
        currentState.update();
    }
}
