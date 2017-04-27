﻿using System.Collections;
using System.Collections.Generic;
using AI.Fuzzy.Library;
using UnityEngine;

public class Team : MonoBehaviour
{

    /**
    *   The Current team State
    */
    State CurrentState;
    State PreviousState;
    State GlobalState;//  = null;


    /**
    *   A list Holding all players
    */
    [HideInInspector]
    public List<Player> Players;

    /**
    *   References to Key players
    */
    public Player ControllingPlayer;// = null;
    public Player ClosetPlayerToBall;//  = null; 
    public Player RecievingPlayer;//  = null; 
    public Player SupportingPlayer;//  = null;

    /**
    *   Reference to the pitch
    */
    Region PitchRef;// = null;

    /**
    *  Refecence to the opposition
    */
    public Team Opponents;// = null;

    public GameObject GoalObject;
    public int NoAttemptsToFindGoodShotAtGoal = 5;
    public float ShootingOffsetForFindingGoodShot = 0.5f;

    public int ChancePlayersWontListenToPassRequest = 5;
    /**
    *  Refecence to the football
    */
    public GameObject Ball;

    /**
    *  Ref to the best supporting spot for the players 
    */
    SupportPosition BestSupportingSpot;// = null;
    public bool UseFuzzyLogic = false;
    public bool DebugOn = false;

    //Fuzzy logic system
    MamdaniFuzzySystem FuzzySystemTeam = new MamdaniFuzzySystem();

    FuzzyVariable FuzzyDistanceToAttackingPlayer = new FuzzyVariable("DistToPlayer", 0.0, 20.0);
    FuzzyVariable FuzzyCanShoot = new FuzzyVariable("CanShoot", -1.0, 2.0);
    FuzzyVariable FuzzySafePass = new FuzzyVariable("SafePass", -1.0, 2.0);
    FuzzyVariable FuzzySupportingPlayer = new FuzzyVariable("SupportingPlayer", 0.0, 0.5);

    public void SetUpFuzzyLogicSystem()
    {
        //Passing Force
        //Fuzzy input variables
        FuzzyDistanceToAttackingPlayer.Terms.Add(new FuzzyTerm("CloseToPlayer", new TriangularMembershipFunction(0, 1, 2)));
        FuzzyDistanceToAttackingPlayer.Terms.Add(new FuzzyTerm("MediumToPlayer", new TriangularMembershipFunction(1, 2.5, 4)));
        FuzzyDistanceToAttackingPlayer.Terms.Add(new FuzzyTerm("FarToPlayer", new TriangularMembershipFunction(2.5, 10, 20)));
        FuzzySystemTeam.Input.Add(FuzzyDistanceToAttackingPlayer);


        FuzzyCanShoot.Terms.Add(new FuzzyTerm("No", new TriangularMembershipFunction(-1, 0.0, 0.5)));
        FuzzyCanShoot.Terms.Add(new FuzzyTerm("Yes", new TriangularMembershipFunction(0.0, 1, 2)));
        FuzzySystemTeam.Input.Add(FuzzyCanShoot);

        FuzzySafePass.Terms.Add(new FuzzyTerm("No", new TriangularMembershipFunction(-1, 0.0, 0.5)));
        FuzzySafePass.Terms.Add(new FuzzyTerm("Yes", new TriangularMembershipFunction(0.0, 1, 2)));
        FuzzySystemTeam.Input.Add(FuzzySafePass);

        //Fuzzy output variables
        FuzzySupportingPlayer.Terms.Add(new FuzzyTerm("No", new TriangularMembershipFunction(0.1, 0.15, 0.2)));
        FuzzySupportingPlayer.Terms.Add(new FuzzyTerm("Maybe", new TriangularMembershipFunction(0.1, 0.2, 0.3)));
        FuzzySupportingPlayer.Terms.Add(new FuzzyTerm("Definetly", new TriangularMembershipFunction(0.3, 0.4, 0.5)));
        FuzzySystemTeam.Output.Add(FuzzySupportingPlayer);


        MamdaniFuzzyRule rule1 = FuzzySystemTeam.ParseRule("if (DistToPlayer is CloseToPlayer )  and (CanShoot is No) and (SafePass is No) then SupportingPlayer is No");
        MamdaniFuzzyRule rule2 = FuzzySystemTeam.ParseRule("if (DistToPlayer is CloseToPlayer )  and (CanShoot is Yes) and (SafePass is No) then SupportingPlayer is Maybe");

        MamdaniFuzzyRule rule3 = FuzzySystemTeam.ParseRule("if (DistToPlayer is CloseToPlayer )  and (CanShoot is No) and (SafePass is Yes) then SupportingPlayer is Maybe");
        MamdaniFuzzyRule rule4 = FuzzySystemTeam.ParseRule("if (DistToPlayer is CloseToPlayer )  and (CanShoot is Yes) and (SafePass is Yes) then SupportingPlayer is Definetly");


        MamdaniFuzzyRule rule5 = FuzzySystemTeam.ParseRule("if (DistToPlayer is MediumToPlayer )  and (CanShoot is No) and (SafePass is No) then SupportingPlayer is No");
        MamdaniFuzzyRule rule6 = FuzzySystemTeam.ParseRule("if (DistToPlayer is MediumToPlayer )  and (CanShoot is Yes) and (SafePass is No) then SupportingPlayer is Maybe");

        MamdaniFuzzyRule rule7 = FuzzySystemTeam.ParseRule("if (DistToPlayer is MediumToPlayer )  and (CanShoot is No) and (SafePass is Yes) then SupportingPlayer is Maybe");
        MamdaniFuzzyRule rule8 = FuzzySystemTeam.ParseRule("if (DistToPlayer is MediumToPlayer )  and (CanShoot is Yes) and (SafePass is Yes) then SupportingPlayer is Definetly");

        MamdaniFuzzyRule rule9 = FuzzySystemTeam.ParseRule("if (DistToPlayer is FarToPlayer )  and (CanShoot is No) and (SafePass is No) then SupportingPlayer is No");
        MamdaniFuzzyRule rule10 = FuzzySystemTeam.ParseRule("if (DistToPlayer is FarToPlayer )  and (CanShoot is Yes) and (SafePass is No) then SupportingPlayer is No");

        MamdaniFuzzyRule rule11 = FuzzySystemTeam.ParseRule("if (DistToPlayer is FarToPlayer )  and (CanShoot is No) and (SafePass is Yes) then SupportingPlayer is Maybe");
        MamdaniFuzzyRule rule12 = FuzzySystemTeam.ParseRule("if (DistToPlayer is FarToPlayer )  and (CanShoot is Yes) and (SafePass is Yes) then SupportingPlayer is Maybe");


       FuzzySystemTeam.Rules.Add(rule1);
       FuzzySystemTeam.Rules.Add(rule2);
       FuzzySystemTeam.Rules.Add(rule3);
       FuzzySystemTeam.Rules.Add(rule4);
       FuzzySystemTeam.Rules.Add(rule5);
       FuzzySystemTeam.Rules.Add(rule6);
       FuzzySystemTeam.Rules.Add(rule7);
       FuzzySystemTeam.Rules.Add(rule8);
       FuzzySystemTeam.Rules.Add(rule9);
       
       FuzzySystemTeam.Rules.Add(rule10);
       FuzzySystemTeam.Rules.Add(rule11);
       FuzzySystemTeam.Rules.Add(rule12);
    }

    public void Init(Region Pitch, List<GameObject> NewPlayers)
    {
        SetUpFuzzyLogicSystem();
        Players = new List<Player>();

        PitchRef = Pitch;
        SetUpPlayers(NewPlayers);

        CurrentState = Defending.Instance();
        PreviousState = Defending.Instance();

        CurrentState.Enter(gameObject);

    }

    /**
    *   Set up for all the players in the team
    */
    public void SetUpPlayers(List<GameObject> NewPlayers)
    {
        foreach (GameObject Footballer in NewPlayers)
        {
            Player Guy = Footballer.GetComponent<Player>();

            Guy.SetTeam(this);

            Players.Add(Guy);
        }

        ControllingPlayer = Players[0];
    }

    void Update()
    {
       CalculateClosetPlayerToTheBall();
       CurrentState.Excute(gameObject); 
    }
    public void DebugThis()
    {
       // Debug.Log(PitchRef.counter);
    }

    public Player DetermineBestSupportingAttacker()
    {
        if (DebugOn)
        {
            Debug.Log("Determining Best Supporting Attacker");
        }

        float ShortestDistanceSoFar = 100000f;

        Player ClosestPlayer = null;

        foreach (Player Guy in Players)
        {
            if (Guy.GetRole() == PlayerRoles.Attacker && Guy != ControllingPlayer)
            {
                float Dist = Vector3.Distance(BestSupportingSpot.Position, Guy.gameObject.transform.position);

                if ( Dist < ShortestDistanceSoFar)
                {
                    ClosestPlayer = Guy;
                    ShortestDistanceSoFar = Dist;
                }
            }
        }

        if (!ClosestPlayer)
        { 
            print("ERROR!! Closest Player Not Found.... This function is broke");
        }

        return ClosestPlayer;
    }

    public bool IsPassSafeFromAllOpponents(Vector2 From, Vector2 Target, GameObject Reciever, float PassingForce)
    {
        foreach (Player Opp in Opponents.Players)
        {
            if (!IsPassSafeFromOpponent(From, Target, Reciever, Opp, PassingForce))
            {
                return false;
            }
        }
        
        return true;
    }

    public bool IsPassSafeFromOpponent(Vector2 From, Vector2 Target, GameObject Reciever, Player Opponent, float PassingForce)
    {
        //TODO ***make sure this works!***
        Vector2 ToTarget = (Target - From).normalized;
        Vector2 ToOpp = (Opponent.transform.position - (Vector3)From).normalized;

        float dot = Vector3.Dot(ToTarget, ToOpp);


        //float dot = Vector3.Dot(ToTarget, ( Reciever.gameObject.transform.position + Opponent.gameObject.transform.position).normalized);

        if (dot > 0.95f) //opponent between player and reciever
        {
            return false;
        }
        else 
        {
            return true;
        }
        
        float RecievcerDistToTarget = Vector3.Distance(Target, Reciever.gameObject.transform.position);
        float OpponentDistToTarget = Vector3.Distance(Target, Opponent.gameObject.transform.position);

        if (RecievcerDistToTarget < OpponentDistToTarget)
        {
            return true;
        }

        return false;
    }

    public bool FindPass(Player From, out Player Reciever, out Vector3 PassTarget, float PassingForce)
    {
        float ClosestToGoalSoFar = 1000000f;

        Reciever = null;

        Vector3 Target = new Vector3();
        PassTarget = Target;

        foreach (Player Guy in Players)
        {
            float PassingDistance = Vector3.Distance(From.gameObject.transform.position, Guy.gameObject.transform.position);

            if (Guy != From && (PassingDistance > Guy.MinPassDistance))
            {
                //TODO fixme
                // if (GetBestPassToReciever(From, Guy, out Target, PassingForce))
                //{
                float DistToGoal = Vector3.Distance(Opponents.GoalObject.transform.position, Target);

                if(DistToGoal < ClosestToGoalSoFar)
                {
                    Reciever = Guy;
                    ClosestToGoalSoFar = DistToGoal;
                    PassTarget = Reciever.transform.position;
                }

                //}                
            }
        }

        if (Reciever)
        {
            return true;
        }

        print("No Pass Found");
        return false;
    }

    public bool GetBestPassToReciever(Player From, Player Reciever, out Vector3 PassTarget, float PassingForce)
    {
        Vector3 Target = new Vector3();
        PassTarget = Target;

        //TODO fixme

        //float PassTime = Ball.GetComponent<Football>().CalculateTimeToCoverDistance(Reciever.gameObject.transform.position, PassingForce);

        //if (PassTime < 0)
        //{
        //    return false;
        //}

        //float InterceptRange = PassTime * Reciever.GetMaxSpeed();

        return true;
    }

    public bool CanShoot(Vector2 ShootingPosition, out Vector2 Target, float ShootingForce, float Confidence = 1.0f)
    {
        Vector2 BestTarget = new Vector2();
        Target = BestTarget;

        int NumAttempts = NoAttemptsToFindGoodShotAtGoal;

        while (NumAttempts > 0)
        {
            Vector2 Shot = Opponents.GoalObject.transform.position;
            Shot += Random.insideUnitCircle * (1.0f - ShootingOffsetForFindingGoodShot);

            float ShotTime = Ball.GetComponent<Football>().CalculateTimeToCoverDistance(Shot, ShootingForce);

            if (ShotTime >= 0)
            {
                if (IsPassSafeFromAllOpponents(ShootingPosition, Shot, Opponents.GoalObject, ShootingForce))
                {
                    Target = Shot;
                    return true;
                }
            }

            NumAttempts--;
        }


        return false;
    }

    public bool UsingFuzzyLogic()
    {
        return UseFuzzyLogic;
    }

    public Vector2 GetSupportSpot()
    {
        return BestSupportingSpot.Position;
    }

    public Vector2 DetermineBestSupportingPosition()
    {
        if (DebugOn)
        {
           // Debug.Log("Determining Best Support Position For Attacker");
        }
        Vector2 BestPoition = new Vector2(0, 0);


        float BestScoreSoFar = 0.0f;

        foreach (SupportPosition SP in PitchRef.SupportPositions)
        {
            //reset the current Weighing
            SP.Weighting = PitchRef.GetDefaultWeighting();
            float PassScore =0 , ShootingScore = 0, DistScore = 0;
            //Calculate the Passing Score to position
            if (IsPassSafeFromAllOpponents(ControllingPlayer.transform.position, SP.Position, SP.obj, ControllingPlayer.PassingForce))
            {
                SP.Weighting += PitchRef.GetSafePassScore() ;
                PassScore = PitchRef.GetSafePassScore();
            }

            //Determine if a goal can be scored from the posiiton
            Vector2 Outtarget = new Vector2();
            if (CanShoot(SP.Position, out Outtarget, 4))
            {
                SP.Weighting += PitchRef.GetShootingChanceScore();
                ShootingScore = PitchRef.GetShootingChanceScore();
            }

            //Check to See if the supporting player is close

            if (SupportingPlayer)
            {
                float OptimalDisitance = 1.0f;

                Vector2 PassingVector = SP.Position - (Vector2)SupportingPlayer.transform.position;
                float dist = PassingVector.magnitude;

                if (dist < OptimalDisitance)
                {
                    SP.Weighting += (OptimalDisitance - dist)/4f;
                    DistScore = dist;
                }

           
            }

            if (UseFuzzyLogic)
            {
                Dictionary<FuzzyVariable, double> InputValues = new Dictionary<FuzzyVariable, double>();
                InputValues.Add(FuzzyCanShoot, ShootingScore);
                InputValues.Add(FuzzySafePass, PassScore);
                InputValues.Add(FuzzyDistanceToAttackingPlayer, Opponents.GetClosetsPlayerDistanceToGivenPoint(SP.Position));


                Dictionary<FuzzyVariable, double> Result = FuzzySystemTeam.Calculate(InputValues);

                float Weight = (float)Result[FuzzySupportingPlayer];

                if (Weight > BestScoreSoFar)
                {
                    BestScoreSoFar = Weight;
                    SP.Weighting = Weight;
                    BestSupportingSpot = SP;
                }

            }
            else
            {
                //If the current position has a better score make it so
                if (SP.Weighting > BestScoreSoFar)
                {
                    BestScoreSoFar = SP.Weighting;
                    BestSupportingSpot = SP;
                }
            }
        }  






        return BestPoition;

    }

    public void ReturnAllFieldPlayersToHome()
    {
        foreach (Player Guy in Players)
        {
            if (Guy.GetRole() != PlayerRoles.GoalKeeper)
            {
                Dispatcher.Instance().DispatchMessage(0, gameObject, Guy.gameObject, PlayerMessages.GoHome);
            }
        }
    }
    public bool AllPlayersAtHome()
    {

        foreach (Player Guy in Players)
        {
            if (!Guy.InHomePosition())
            {
                return false;
            }
        }

        return true;
    }

    public void ChangeState(GameObject CallingObject, State NewState)
    {
      
        PreviousState = CurrentState;

        CurrentState.Exit(gameObject);

        CurrentState = NewState;

        CurrentState.Enter(gameObject);
         
    }

    public Region GetPitch()
    {
        return PitchRef;
    }

    public void UpdateTargetsOfWaitingPlayers()
    {
        foreach (Player Guy in Players)
        {
            if (Guy.GetRole() != PlayerRoles.GoalKeeper)
            {
                if (Guy.IsInState(Wait.Instance()) || Guy.IsInState(ReturnToHomeRegion.Instance()))
                {
                    Steer2D.Arrive Arr = (Steer2D.Arrive)Guy.GetSteeringController().GetBehaviourByTypeName("Steer2D.Arrive");
                    Arr.TargetPoint = Guy.HomePosition;     
                }
            }
        }
    }

    public void SetHomeRegions(HomeRegions RegionEnum)
    {
        switch (RegionEnum)
        {
            case HomeRegions.Attacking:
                foreach (Player Guy in Players)
                {
                    Guy.HomePosition = Guy.AttackingPosition;

                }
                break;

            case HomeRegions.Defending:

                foreach (Player Guy in Players)
                {
                    Guy.HomePosition = Guy.DefendingPosition;

                }
                break;

        }
    }

    public bool InControl()
    {
        if (ControllingPlayer)
        {
            return true;
        }

        return false;
    }

    public void RequestPass(GameObject RequestingPlayer)
    {
        int ListenConstant = 3;
        int Rand = Random.Range(0, ChancePlayersWontListenToPassRequest + ListenConstant);

        if (Rand < 5)
        {
            return;
        }

        if(IsPassSafeFromAllOpponents(ControllingPlayer.gameObject.transform.position, RequestingPlayer.transform.position, RequestingPlayer, ControllingPlayer.PassingForce))
        {
            Dispatcher.Instance().DispatchMessage(0, RequestingPlayer, ControllingPlayer.gameObject, PlayerMessages.PassToMe);
        }

    }

    void CalculateClosetPlayerToTheBall()
    {
        float ShortestDistanceSoFar = 100000f;

        Player ClosestPlayer = null;

        foreach (Player Guy in Players)
        {
            Guy.SetClosestTeamMemberToBall(false);

            float Dist = Vector3.Distance(Ball.transform.position, Guy.gameObject.transform.position);

            if ( Dist < ShortestDistanceSoFar)
            {
                ClosestPlayer = Guy;
                ShortestDistanceSoFar = Dist;

            }
        }

        if (ClosestPlayer)
        {
            ClosestPlayer.SetClosestTeamMemberToBall(true);
            ClosetPlayerToBall = ClosestPlayer;
        }
        else
        {
            print("ERROR!! Closest Player Not Found.... This function is broke");
        }

    }

    public void SetControllingPlayer(Player NewControllingPlayer)
    {
        ControllingPlayer = NewControllingPlayer;

        Opponents.ControllingPlayer = null;
    }

    public State GetState()
    {
        return CurrentState;
    }


    public float GetClosetsPlayerDistanceToGivenPoint(Vector3 Point)
    {
        float FinalDist = 1000000.0f;

        foreach (Player Guy in Players)
        {
            float Dist = Vector3.Distance(Point, Guy.gameObject.transform.position);

            if (Dist < FinalDist)
            {
                FinalDist = Dist;
            }

        }

        return FinalDist;
    }
         
}
