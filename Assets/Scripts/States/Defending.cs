using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public enum HomeRegions { Attacking, Defending};

//public class Defending : State
//{
//    static Defending instance;

//    public static Defending Instance()
//    {
//        if (instance == null)
//        {
//            instance = new Defending();
//        }
//        return instance;
//    }


//    /**
//    *   this will execute when the state is entered
//    */
//    public override void Enter(GameObject CallingObject)
//    {
//        Team TeamScript = CallingObject.GetComponent<Team>();

//        TeamScript.SetHomeRegions(HomeRegions.Defending);


//        TeamScript.UpdateTargetsOfWaitingPlayers();
//    }

//    /**
//    *   this is the updated fucntion for the state
//    */
//    public override void Excute(GameObject CallingObject)
//    {
//        Team TeamScript = CallingObject.GetComponent<Team>();

//        if (TeamScript.InControl())
//        {
//            TeamScript.ChangeState(Attacking.Instance());

//        }
//    }

//    /**
//    *   this will execute when the state is exited
//    */
//    public override void Exit(GameObject CallingObject)
//    {

//    }
//}
