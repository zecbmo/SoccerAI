using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
/*
 *  ******************************************** Global State ********************** 
 */

public class GlobalPlayerState : State
{
    static GlobalPlayerState instance;

    public static GlobalPlayerState Instance()
    {
        if (instance == null)
        {
            instance = new GlobalPlayerState();
        }
        return instance;
    }

    /**
    *   this will execute when the state is entered
    */
    public override void Enter(GameObject CallingObject)
    {

    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingObject)
    {

    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingObject)
    {

    }

    public override bool OnMessage(GameObject CallingObject, Message Msg)
    {
        FieldPlayer PlayerScript = CallingObject.GetComponent<FieldPlayer>();


        switch (Msg.Msg)
        {
            case PlayerMessages.ReceiveBall:
                {
                    GCHandle Handle = (GCHandle)Msg.ExtraInfo;
                    Vector2 Pos = (Vector2)Handle.Target;
                           

                    //TODO Behaviour



                    PlayerScript.ChangeState(CallingObject, ReceiveBall.Instance());
                    return true;
                }
            case PlayerMessages.SupportAttacker:
                {
                    if(PlayerScript.IsInState(SupportAttacker.Instance()))
                    {
                        return true;
                    }

                    //TODO Behaviour
                    PlayerScript.ChangeState(CallingObject, SupportAttacker.Instance());
                    return true;
                }
               

            case PlayerMessages.Wait:
                {
                    PlayerScript.ChangeState(CallingObject, Wait.Instance());
                    return true;
                }             

            case PlayerMessages.PassToMe:
                {
                    GameObject Reciever = Msg.Sender;

                    //If there is already a recieving player or the ball isnt in range then cant pass pall to requesting player
                    if (PlayerScript.GetTeam().RecievingPlayer != null || !PlayerScript.BallInKickingRange())
                    {
                        Debug.Log("Can't make requested Pass");
                        return true;
                    }

                    //TODO Kick the Ball


                    GCHandle Handle = GCHandle.Alloc(Reciever.transform.position);
                    System.IntPtr PositionPtr = (System.IntPtr)Handle;

                    Dispatcher.Instance().DispatchMessage(0, CallingObject, Reciever, PlayerMessages.ReceiveBall, PositionPtr);

                    PlayerScript.ChangeState(CallingObject, Wait.Instance());

                    PlayerScript.FindSupport();

                    return true;
                }

            case PlayerMessages.GoHome:
                {
                    PlayerScript.SetDefaultHomeRegion();
                    PlayerScript.ChangeState(CallingObject, ReturnToHomeRegion.Instance());
                    return true;
                }
                
        }


        return false;
    }
}

/*
 *  ******************************************** ChaseBall State ********************** 
 */

public class ChaseBall : State
{
    static ChaseBall instance;

    public static ChaseBall Instance()
    {
        if (instance == null)
        {
            instance = new ChaseBall();
        }
        return instance;
    }

    /**
    *   this will execute when the state is entered
    */
    public override void Enter(GameObject CallingObject)
    {

    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingObject)
    {

    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingObject)
    {

    }
}

/*
 *  ******************************************** Dribble State ********************** 
 */

public class Dribble : State
{
    static Dribble instance;

    public static Dribble Instance()
    {
        if (instance == null)
        {
            instance = new Dribble();
        }
        return instance;
    }

    /**
    *   this will execute when the state is entered
    */
    public override void Enter(GameObject CallingObject)
    {

    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingObject)
    {

    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingObject)
    {

    }
}
/*
 *  ******************************************** ReturnToHomeRegion State ********************** 
 */

public class ReturnToHomeRegion : State
{
    static ReturnToHomeRegion instance;

    public static ReturnToHomeRegion Instance()
    {
        if (instance == null)
        {
            instance = new ReturnToHomeRegion();
        }
        return instance;
    }

    /**
    *   this will execute when the state is entered
    */
    public override void Enter(GameObject CallingObject)
    {

    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingObject)
    {

    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingObject)
    {

    }
}
/*
 *  ******************************************** Wait State ********************** 
 */

public class Wait : State
{
    static Wait instance;

    public static Wait Instance()
    {
        if (instance == null)
        {
            instance = new Wait();
        }
        return instance;
    }

    /**
    *   this will execute when the state is entered
    */
    public override void Enter(GameObject CallingObject)
    {

    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingObject)
    {

    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingObject)
    {

    }
}
/*
 *  ******************************************** KickBall State ********************** 
 */

public class KickBall : State
{
    static KickBall instance;

    public static KickBall Instance()
    {
        if (instance == null)
        {
            instance = new KickBall();
        }
        return instance;
    }

    /**
    *   this will execute when the state is entered
    */
    public override void Enter(GameObject CallingObject)
    {

    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingObject)
    {

    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingObject)
    {

    }
}
/*
 *  ******************************************** ReceiveBall State ********************** 
 */

public class ReceiveBall : State
{
    static ReceiveBall instance;

    public static ReceiveBall Instance()
    {
        if (instance == null)
        {
            instance = new ReceiveBall();
        }
        return instance;
    }

    /**
    *   this will execute when the state is entered
    */
    public override void Enter(GameObject CallingObject)
    {

    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingObject)
    {

    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingObject)
    {

    }
}

/*
 *  ******************************************** SupportAttacker State ********************** 
 */

public class SupportAttacker : State
{
    static SupportAttacker instance;

    public static SupportAttacker Instance()
    {
        if (instance == null)
        {
            instance = new SupportAttacker();
        }
        return instance;
    }

    /**
    *   this will execute when the state is entered
    */
    public override void Enter(GameObject CallingObject)
    {

    }

    /**
    *   this is the updated fucntion for the state
    */
    public override void Excute(GameObject CallingObject)
    {

    }

    /**
    *   this will execute when the state is exited
    */
    public override void Exit(GameObject CallingObject)
    {

    }
}