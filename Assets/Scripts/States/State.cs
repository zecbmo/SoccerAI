using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    /**
    *   this will execute when the state is entered
    */
    public abstract void Enter(GameObject CallingScript);

    /**
    *   this is the updated fucntion for the state
    */
    public abstract void Excute(GameObject CallingScript);

    /**
    *   this will execute when the state is exited
    */
    public abstract void Exit(GameObject CallingScript);

}
