using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum HomeRegions { Attacking, Defending };



/**
*
*     ********************************  Defending ****************************************************
* 
*/
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
    public override void Enter(GameObject CallingObject)
    {
        Team TeamScript = CallingObject.GetComponent<Team>();

        TeamScript.SetHomeRegions(HomeRegions.Defending);


        TeamScript.UpdateTargetsOfWaitingPlayers();
    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingObject)
    {
        Team TeamScript = CallingObject.GetComponent<Team>();

        if (TeamScript.InControl())
        {
            TeamScript.ChangeState(CallingObject, Attacking.Instance());

        }
    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingObject)
    {

    }
}


/**
*
*     ********************************  PREPARE FOR KICKOFF ****************************************************
* 
*/


public class PrepareForKickoff : State
{
   
    static PrepareForKickoff instance;

    public static PrepareForKickoff Instance()
    {
        if (instance == null)
        {
            instance = new PrepareForKickoff();
        }
        return instance;
    }

    /**
    *   this will execute when the state is entered
    */
    public override void Enter(GameObject CallingObject)
    {

        Team TeamScript = CallingObject.GetComponent<Team>();

        //reset players
        TeamScript.ControllingPlayer = null;
        TeamScript.SupportingPlayer = null;
        TeamScript.RecievingPlayer = null;
        TeamScript.ClosetPlayerToBall = null;


        //send players home
        TeamScript.ReturnAllFieldPlayersToHome();
    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingObject)
    {
        Team TeamScript = CallingObject.GetComponent<Team>();

        //if both teams in position, start the game
        if (TeamScript.AllPlayersAtHome() && TeamScript.Opponents.AllPlayersAtHome())
        {
            TeamScript.ChangeState(CallingObject, Defending.Instance());
        }
    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingObject)
    {
        Team TeamScript = CallingObject.GetComponent<Team>();

        TeamScript.GetPitch().SetGameInPlay(true);
    }

}


/**
*
*     ********************************  ATTACKING ****************************************************
* 
*/
public class Attacking : State
{

    static Attacking instance;

    public static Attacking Instance()
    {
        if (instance == null)
        {
            instance = new Attacking();
        }
        return instance;
    }

    /**
    *   this will execute when the state is entered
    */
    public override void Enter(GameObject CallingObject)
    {
        Team TeamScript = CallingObject.GetComponent<Team>();

        TeamScript.SetHomeRegions(HomeRegions.Attacking);

        TeamScript.UpdateTargetsOfWaitingPlayers();
    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingObject)
    {
        Team TeamScript = CallingObject.GetComponent<Team>();

        if (!TeamScript.InControl())
        {
            TeamScript.ChangeState(CallingObject, Defending.Instance());
            return;
        }

        TeamScript.DetermineBestSupportingPosition();
    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingObject)
    {
        Team TeamScript = CallingObject.GetComponent<Team>();

        TeamScript.SupportingPlayer = null;
    }

}
