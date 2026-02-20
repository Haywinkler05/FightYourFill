using UnityEngine;
using UnityEngine.AI;

public interface IState 
{
    void onEnter(); 

    void onExit();

    void update();
}
