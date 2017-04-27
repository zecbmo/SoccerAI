using System.Collections;
using System.Collections.Generic;
using AI.Fuzzy.Library;
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

    public float KickDelay = 0.5f;
    private float NextKick = 0f;
    public float KickingDistance = 0.2f;

    public float ShootingConfidence = 0.8f;
    public float ShootingAccuracy = 0.8f;
    public float ShootingRange = 2.0f;

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

    public float TextOffSet = 0.5f;
    public GameObject StateTextObject;
    public bool DebugOn = false;
    public bool FuzzyDebugOn = false;

    public Team GetTeam()
    {
        return PlayersTeam;
    }
    public void SetTeam(Team team)
    {
        PlayersTeam = team;
    }
    public SteeringController GetSteeringController()
    {
        return SteerController;
    }


    //Fuzzy logic system
    MamdaniFuzzySystem FuzzySystemShoot = new MamdaniFuzzySystem();
    MamdaniFuzzySystem FuzzySystemPass = new MamdaniFuzzySystem();


    FuzzyVariable FuzzyDistanceFromGoal = new FuzzyVariable("DistFromGoal", 0.0, 20.0);
    FuzzyVariable FuzzyShootingConfidence = new FuzzyVariable("ShootingConf", 0.0, 1.0);
    FuzzyVariable FuzzyShootingForce = new FuzzyVariable("ShootForce", 1.0, 6.0);

    FuzzyVariable FuzzyDistanceToPassingPlayer = new FuzzyVariable("DistFromPlayer", 0.0, 20.0);
    FuzzyVariable FuzzyPassingForceIn = new FuzzyVariable("PassingForceIn", 1.0, 6.0);
    FuzzyVariable FuzzyPassingForceOut = new FuzzyVariable("PassingForceOut", 1.0, 6.0);

  


    void SetUpFuzzyLogic()
    {
        //Fuzzy input variables
        FuzzyDistanceFromGoal.Terms.Add(new FuzzyTerm("CloseToGoal",new TriangularMembershipFunction(-1,0,1)));
        FuzzyDistanceFromGoal.Terms.Add(new FuzzyTerm("MediumToGoal", new TriangularMembershipFunction(0, 2, 4)));
        FuzzyDistanceFromGoal.Terms.Add(new FuzzyTerm("FarToGoal", new TriangularMembershipFunction(2, 4, 40)));
        FuzzySystemShoot.Input.Add(FuzzyDistanceFromGoal);

        FuzzyShootingConfidence.Terms.Add(new FuzzyTerm("LowConf", new TriangularMembershipFunction(0.1, 0.3, 0.5)));
        FuzzyShootingConfidence.Terms.Add(new FuzzyTerm("MedConf", new TriangularMembershipFunction(0.3, 0.6, 0.8)));
        FuzzyShootingConfidence.Terms.Add(new FuzzyTerm("HighConf", new TriangularMembershipFunction(0.7, 1, 1.3)));
        FuzzySystemShoot.Input.Add(FuzzyShootingConfidence);

        //Fuzzy output variables
        FuzzyShootingForce.Terms.Add(new FuzzyTerm("LowShot", new TriangularMembershipFunction(1, 2, 3)));
        FuzzyShootingForce.Terms.Add(new FuzzyTerm("MedShot", new TriangularMembershipFunction(2, 3, 4)));
        FuzzyShootingForce.Terms.Add(new FuzzyTerm("HighShot", new TriangularMembershipFunction(3, 4, 5)));
        FuzzySystemShoot.Output.Add(FuzzyShootingForce);

        //Rules for Shooting  force fuzzy logic
        MamdaniFuzzyRule rule1 = FuzzySystemShoot.ParseRule("if (DistFromGoal is CloseToGoal )  and (ShootingConf is HighConf) then ShootForce is HighShot");
        MamdaniFuzzyRule rule2 = FuzzySystemShoot.ParseRule("if (DistFromGoal is CloseToGoal )  and (ShootingConf is MedConf) then ShootForce is HighShot");
        MamdaniFuzzyRule rule3 = FuzzySystemShoot.ParseRule("if (DistFromGoal is CloseToGoal )  and (ShootingConf is LowConf) then ShootForce is MedShot");

        MamdaniFuzzyRule rule4 = FuzzySystemShoot.ParseRule("if (DistFromGoal is MediumToGoal )  and (ShootingConf is HighConf) then ShootForce is HighShot");
        MamdaniFuzzyRule rule5 = FuzzySystemShoot.ParseRule("if (DistFromGoal is MediumToGoal )  and (ShootingConf is MedConf) then ShootForce is MedShot");
        MamdaniFuzzyRule rule6 = FuzzySystemShoot.ParseRule("if (DistFromGoal is MediumToGoal )  and (ShootingConf is LowConf) then ShootForce is LowShot");

        MamdaniFuzzyRule rule7 = FuzzySystemShoot.ParseRule("if (DistFromGoal is FarToGoal )  and (ShootingConf is HighConf) then ShootForce is MedShot");
        MamdaniFuzzyRule rule8 = FuzzySystemShoot.ParseRule("if (DistFromGoal is FarToGoal )  and (ShootingConf is MedConf) then ShootForce is LowShot");
        MamdaniFuzzyRule rule9 = FuzzySystemShoot.ParseRule("if (DistFromGoal is FarToGoal )  and (ShootingConf is LowConf) then ShootForce is LowShot");


        FuzzySystemShoot.Rules.Add(rule1);
        FuzzySystemShoot.Rules.Add(rule2);
        FuzzySystemShoot.Rules.Add(rule3);
        FuzzySystemShoot.Rules.Add(rule4);
        FuzzySystemShoot.Rules.Add(rule5);
        FuzzySystemShoot.Rules.Add(rule6);
        FuzzySystemShoot.Rules.Add(rule7);
        FuzzySystemShoot.Rules.Add(rule8);
        FuzzySystemShoot.Rules.Add(rule9);


        //Passing Force
        //Fuzzy input variables
        FuzzyDistanceToPassingPlayer.Terms.Add(new FuzzyTerm("CloseToPlayer", new TriangularMembershipFunction(0, 1, 2)));
        FuzzyDistanceToPassingPlayer.Terms.Add(new FuzzyTerm("MediumToPlayer", new TriangularMembershipFunction(1, 2.5, 4)));
        FuzzyDistanceToPassingPlayer.Terms.Add(new FuzzyTerm("FarToPlayer", new TriangularMembershipFunction(2.5, 10, 40)));
        FuzzySystemPass.Input.Add(FuzzyDistanceToPassingPlayer);


        FuzzyPassingForceIn.Terms.Add(new FuzzyTerm("LowForce", new TriangularMembershipFunction(1, 2, 3)));
        FuzzyPassingForceIn.Terms.Add(new FuzzyTerm("MedForce", new TriangularMembershipFunction(2, 3, 4)));
        FuzzyPassingForceIn.Terms.Add(new FuzzyTerm("HighForce", new TriangularMembershipFunction(3, 4, 5)));
        FuzzySystemPass.Input.Add(FuzzyPassingForceIn);

        //Fuzzy output variables
        FuzzyPassingForceOut.Terms.Add(new FuzzyTerm("LowForce", new TriangularMembershipFunction(1, 2, 3)));
        FuzzyPassingForceOut.Terms.Add(new FuzzyTerm("MedForce", new TriangularMembershipFunction(2, 3, 4)));
        FuzzyPassingForceOut.Terms.Add(new FuzzyTerm("HighForce", new TriangularMembershipFunction(3, 4, 5)));
        FuzzySystemPass.Output.Add(FuzzyPassingForceOut);

        //Rules for passing  force fuzzy logic
        MamdaniFuzzyRule rule10 = FuzzySystemPass.ParseRule("if (DistFromPlayer is CloseToPlayer )  and (PassingForceIn is HighForce) then PassingForceOut is HighForce");
        MamdaniFuzzyRule rule11 = FuzzySystemPass.ParseRule("if (DistFromPlayer is CloseToPlayer )  and (PassingForceIn is MedForce) then PassingForceOut is HighForce");
        MamdaniFuzzyRule rule12 = FuzzySystemPass.ParseRule("if (DistFromPlayer is CloseToPlayer )  and (PassingForceIn is LowForce) then PassingForceOut is MedForce");
                                 
        MamdaniFuzzyRule rule13 = FuzzySystemPass.ParseRule("if (DistFromPlayer is MediumToPlayer )  and (PassingForceIn is HighForce) then PassingForceOut is HighForce");
        MamdaniFuzzyRule rule14 = FuzzySystemPass.ParseRule("if (DistFromPlayer is MediumToPlayer )  and (PassingForceIn is MedForce) then PassingForceOut is MedForce");
        MamdaniFuzzyRule rule15 = FuzzySystemPass.ParseRule("if (DistFromPlayer is MediumToPlayer )  and (PassingForceIn is LowForce) then PassingForceOut is LowForce");
                                 
        MamdaniFuzzyRule rule16 = FuzzySystemPass.ParseRule("if (DistFromPlayer is FarToPlayer )  and (PassingForceIn is HighForce) then PassingForceOut is MedForce");
        MamdaniFuzzyRule rule17 = FuzzySystemPass.ParseRule("if (DistFromPlayer is FarToPlayer )  and (PassingForceIn is MedForce) then PassingForceOut is LowForce");
        MamdaniFuzzyRule rule18 = FuzzySystemPass.ParseRule("if (DistFromPlayer is FarToPlayer )  and (PassingForceIn is LowForce) then PassingForceOut is LowForce");


       FuzzySystemPass.Rules.Add(rule10);
       FuzzySystemPass.Rules.Add(rule11);
       FuzzySystemPass.Rules.Add(rule12);
       FuzzySystemPass.Rules.Add(rule13);
       FuzzySystemPass.Rules.Add(rule14);
       FuzzySystemPass.Rules.Add(rule15);
       FuzzySystemPass.Rules.Add(rule16);
       FuzzySystemPass.Rules.Add(rule17);
        FuzzySystemPass.Rules.Add(rule18);

        //Iputs
        //DistanceToHomePos
        //DistanceToClosestplayer
        //Output
        //At home Yes or now



    }




    // Use this for initialization
    void Start()
    {
        if (Random.Range(0, 1) == 0) //randomise the turn Direction
        {
            PreferedTurnDir = -1;
        }

        SteerController = GetComponent<SteeringController>();

        HomePosition = transform.position;
        DefendingPosition = transform.position;
        AttackingPosition = AttackPoint.transform.position;

        CurrentState = ReturnToHomeRegion.Instance();
        PreviousState = ReturnToHomeRegion.Instance();
        GlobalState = GlobalPlayerState.Instance();

        CurrentState.Enter(gameObject);

        UpdateStateText();

        NextKick = Time.time + KickDelay;

        SetUpFuzzyLogic();
    }
    // Update is called once per frame
    void Update ()
    {
        CurrentState.Excute(gameObject);

        StateTextObject.transform.position = transform.position + new Vector3(0, TextOffSet,0);

    }

    public void FindSupport()
    {

        if (PlayersTeam.SupportingPlayer == null)
        {
            Player Guy = PlayersTeam.DetermineBestSupportingAttacker();

            PlayersTeam.SupportingPlayer = Guy;

            Dispatcher.Instance().DispatchMessage(0, gameObject, Guy.gameObject, PlayerMessages.SupportAttacker);
        }
    }

    public bool InShootingRange()
    {

        float Dist = Vector3.Distance(OpponentsGoal.transform.position, gameObject.transform.position);

        if (Dist < ShootingRange)
        {
            return true;
        }

        return false;
    }
    

    public void SetDefaultHomeRegion()
    {
        HomePosition = DefendingPosition;
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
        float Dist = Vector3.Distance(Ball.transform.position, gameObject.transform.position);

        if (PlayersTeam.UsingFuzzyLogic())
        {
            // double KickChance = FuzzyEngineBallInKickRange.Defuzzify(new { FuzzyDistanceFromBall = (double)Dist });

            float KickChance = 0;

            if (FuzzyDebugOn)
            {
                Debug.Log("Kick Distance =  " + Dist);

                Debug.Log("Kick Chance =  " + KickChance );
            }

            if (KickChance > 50)
            {
                return true;
            }
        }


       // else
        {
            if (KickingDistance > Dist)
            {
                return true;
            }
        }

        return false;
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

        UpdateStateText();
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
        //transform.rotation = Quaternion.LookRotation(Vector3.right, Ball.transform.position - transform.position);

        Vector2 Dir = Ball.transform.position - transform.position;

        float Angle = Mathf.Atan2(Dir.y, Dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(Angle, Vector3.forward);
    }

    public bool AheadOfAttacker()
    {
        float GoalXPos = OpponentsGoal.transform.position.x;
        float AttackerX = PlayersTeam.ControllingPlayer.transform.position.x;
        float PlayersX = gameObject.transform.position.x;

        float PlayersDistToGoal = Vector2.Distance(OpponentsGoal.transform.position, gameObject.transform.position);
        float AttackersDistToGoal = Vector2.Distance(OpponentsGoal.transform.position, PlayersTeam.ControllingPlayer.transform.position);


        if (PlayersDistToGoal > AttackersDistToGoal)
        {
            return true;
        }

        return false;
    }

    public bool IsReadyForNextKick()
    {
        if (NextKick < Time.time)
        {
            NextKick = Time.time + KickDelay;
            return true;
        }
        return false;
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
        foreach (Player Guy in PlayersTeam.Opponents.Players)
        {

            float Dist = 1f;
            if (Dist > Vector2.Distance(Guy.gameObject.transform.position, gameObject.transform.position))
            {
                return true;
            }

        }
        return false;
    }

    public bool BallInReceivingRange()
    {
        float Dist = Vector3.Distance(Ball.transform.position, gameObject.transform.position);

        if (KickingDistance > Dist)
        {
            return true;
        }

        return false;
    }

    public bool IsThreatened()
    {
        if (IsOppenentWithinRadius())
        {
            return true;

        }
        return false;
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
        float Dist = Vector3.Distance(HomePosition, gameObject.transform.position);

        //If you are close to home and far from the closest player - home chance is high

        if (0.5f > Dist)
        {
            return true;
        }

        return false;
    }

    void UpdateStateText()
    {
        StateTextObject.GetComponent<TextMesh>().text = CurrentState.GetType().ToString();
        
    }


    public float GetShootingForce()
    {


        float ShotForce = MaxShootingForce;

        if (PlayersTeam.UsingFuzzyLogic())
        {
            float StartTime = TestBase.GetStartTime(); ;
            float Dist = Vector3.Distance(OpponentsGoal.transform.position, gameObject.transform.position);


            Dictionary<FuzzyVariable, double> InputValues = new Dictionary<FuzzyVariable, double>();
            InputValues.Add(FuzzyDistanceFromGoal, (double)Dist);
            InputValues.Add(FuzzyShootingConfidence, (double)ShootingConfidence);

            Dictionary<FuzzyVariable, double> Result = FuzzySystemShoot.Calculate(InputValues);

            ShotForce = (float)Result[FuzzyShootingForce];

            if (FuzzyDebugOn)
            {
                Debug.Log("Distacne to goal = " + Dist + "::: Shooting Confidence = " + ShootingConfidence);
                Debug.Log("Shot Force = " + ShotForce);

            }


            float EndTime = TestBase.GetEndTime(); 
            TestBase.AddToMeassuredTime(EndTime - StartTime);
        }

        return ShotForce;
    }

    public float GetPassingForce(Vector3 Target)
    {
        float PassForce = PassingForce;

        if (PlayersTeam.UsingFuzzyLogic())
        {
            float StartTime = TestBase.GetStartTime();

            float Dist = Vector3.Distance(Target, gameObject.transform.position);

            if (FuzzyDebugOn)
            {
                Debug.Log("Distacne to Passing player = " + Dist);
            }

            Dictionary<FuzzyVariable, double> InputValues = new Dictionary<FuzzyVariable, double>();
            InputValues.Add(FuzzyDistanceToPassingPlayer, (double)Dist);
            InputValues.Add(FuzzyPassingForceIn, (double)PassingForce);

            Dictionary<FuzzyVariable, double> Result = FuzzySystemPass.Calculate(InputValues);

            PassForce = (float)Result[FuzzyPassingForceOut];

            if (FuzzyDebugOn)
            {
                Debug.Log("Shot Force = " + PassForce);
            }



            float EndTime = TestBase.GetEndTime();
            TestBase.AddToMeassuredTime(EndTime - StartTime);
        }

        return PassForce;
    }
}
