using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Player Constants
    public float PassingForce = 5.0f;
    public float MaxShootingForce = 5.0f;

    State CurrentState;
    State PreviousState;
    State GlobalState;

    Team PlayersTeam = null;

    public Team GetTeam()
    {
        return PlayersTeam;
    }

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void FindSupport()
    {
        //TODO
    }

    public void SetDefaultHomeRegion()
    {
        //TODO
    }


    public bool IsInState(State CheckingState)
    {
        if (CurrentState == CheckingState)
        {
            return true;
        }
        return false;
    }

    public bool BallInKickingRange()
    {
        return true;
    }

    public void ChangeState(GameObject CallingObject, State NewState)
    {
        //TODO    
    }

    public bool HandleMessage(Message Telegram)
    {
        //TODO
        return false;
    }
}
