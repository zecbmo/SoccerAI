using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Defending : State
{
    static Defending instance;

    public static Defending Instance()
    {
        if (instance == null)
        {
            instance = new Defending();
        }
        return instance;
    }


    /**
    *   this will execute when the state is entered
    */
    public override void Enter(GameObject CallingScript)
    {

    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingScript)
    {
    
    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingScript)
    {

    }
}
