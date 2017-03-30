using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PlayerRoles { Attacker, Defender, GoalKeeper} 

public class Player : MonoBehaviour
{

    public PlayerRoles Role = PlayerRoles.Attacker;

    //The Base Position for the player
    [HideInInspector]
    public Vector2 HomePosition;
    [HideInInspector]
    public Vector2 AttackingPosition;
    [HideInInspector]
    public Vector2 DefendingPosition;

    public GameObject AttackPoint;

    //Player Constants
    public float PassingForce = 5.0f;
    public float MaxShootingForce = 5.0f;
    public float DribbleForce = 1.0f;
    public float TurningForce = 0.5f;
    public float MinPassDistance = 1.0f;


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

    bool ClosestPlayer = false;

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

        HomePosition = transform.position;
        DefendingPosition = transform.position;
        AttackingPosition = AttackPoint.transform.position;

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
       return ClosestPlayer;
    }

    public void ChangeState(GameObject CallingObject, State NewState)
    {
        PreviousState = CurrentState;

        CurrentState.Exit(gameObject);

        CurrentState = NewState;

        CurrentState.Enter(gameObject);
    }

    public bool HandleMessage(Message Telegram)
    {
        if (GlobalState != null)
        {
            if (GlobalState.OnMessage(gameObject, Telegram))
            {
                return true;
            }
        }

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

    public void SetClosestTeamMemberToBall(bool IsClosest)
    {
        ClosestPlayer = IsClosest;
    }

    public PlayerRoles GetRole()
    {
        return Role;
    }

    public float GetMaxSpeed()
    {
        return gameObject.GetComponent<Steer2D.SteeringAgent>().MaxVelocity;
    }

    public bool InHomePosition()
    {
        //TODO
        return true;
    }
}
