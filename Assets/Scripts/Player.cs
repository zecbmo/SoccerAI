using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //The Base Position for the player
    public Vector2 HomePosition;

    //Player Constants
    public float PassingForce = 5.0f;
    public float MaxShootingForce = 5.0f;
    public float DribbleForce = 1.0f;
    public float TurningForce = 0.5f;

    public float ShootingConfidence = 0.8f;
    public float ShootingAccuracy = 0.8f;

    //will be one or minus one // randomised on start
    public int PreferedTurnDir = 1;

    State CurrentState;
    State PreviousState;
    State GlobalState;

    Team PlayersTeam = null;

    public GameObject Ball;
    public GameObject OpponentsGoal;
    SteeringController SteerController;

    public Team GetTeam()
    {
        return PlayersTeam;
    }

    public SteeringController GetSteeringController()
    {
        return SteerController;
    }

    // Use this for initialization
    void Start ()
    {
        if (Random.Range(0, 1) == 0) //randomise the turn Direction
        {
            PreferedTurnDir = -1;
        }

        SteerController = GetComponent<SteeringController>();
        

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

    public bool IsClosestTeamMemberToBall()
    {
        //TODO
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

    public void TrackBall()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.right, Ball.transform.position - transform.position);
    }

    public bool AheadOfAttacker()
    {
        //TODO
        return true;
    }

    public bool IsReadyForNextKick()
    {
        //TODO
        return true;
    }

    //public Vector2 GetShotTarget()
    //{
    //    Vector2 Shot = OpponentsGoal.transform.position;

    //    Shot += Random.insideUnitCircle * (1.0f - ShootingAccuracy);

    //    return Shot;
    //}

    public Vector2 AddNoiseToTarget(Vector2 Target)
    {
        Vector2 Shot = Target;

        Shot += Random.insideUnitCircle * (1.0f - ShootingAccuracy);

        return Shot;
    }

    public bool IsOppenentWithinRadius()
    {

        return true;
    }

    public bool BallInReceivingRange()
    {
        return true;
    }

    public bool IsThreatened()
    {
        //TODO
        return true;
    }
}
