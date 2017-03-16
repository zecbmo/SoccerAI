using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team
{

    /**
    *   A list Holding all players
    */
    private List<Player> Players;

    /**
    *   References to Key players
    */
    public Player ControllingPlayer = null;
    public Player ClosetPlayerToBall = null; 
    public Player RecievingPlayer = null; 
    public Player SupportingPlayer = null; 

    /**
    *   Reference to the pitch
    */
    Region PitchRef = null;

    /**
    *  Refecence to the opposition
    */
    public Team Opponents = null;

    /**
    *  Ref to the best supporting spot for the players 
    */
    SupportPosition BestSupportingSpot = null;

    public void Init(Region Pitch, List<GameObject> NewPlayers)
    {
        Players = new List<Player>();

        PitchRef = Pitch;
        SetUpPlayers(NewPlayers);
    }

    /**
    *   Set up for all the players in the team
    */
    public void SetUpPlayers(List<GameObject> NewPlayers)
    {
        foreach (GameObject Footballer in NewPlayers)
        {
            Players.Add(Footballer.GetComponent<Player>());
        }

        ControllingPlayer = Players[0];
    }

    void Update()
    {
       
    }
    public void DebugThis()
    {
        Debug.Log(PitchRef.counter);
    }

    public bool IsPassSafeFromAllOpponents(Vector2 From, Vector2 Target, Player Reciever, float PassingForce)
    {
        //TODO
        return true;
    }

    public bool IsPassSafeFromOpponent(Vector2 From, Vector2 Target, Player Reciever, Player Opponent, float PassingForce)
    {
        //TODO

        return true;
    }

    public bool CanShoot(Vector2 ShootingPosition, float ShootingForce)
    {
        //TODO

        return true;
    }


    public Vector2 DetermineBestSupportingPosition()
    {

        Vector2 BestPoition = new Vector2(0, 0);


        float BestScoreSoFar = 0.0f;

        foreach (SupportPosition SP in PitchRef.SupportPositions)
        {
            //reset the current Weighing
            SP.Weighting = PitchRef.GetDefaultWeighting();

            //Calculate the Passing Score to position
            if (IsPassSafeFromAllOpponents(ControllingPlayer.transform.position, SP.Position, null, ControllingPlayer.PassingForce))
            {
                SP.Weighting += PitchRef.GetSafePassScore();
            }

            //Determine if a goal can be scored from the posiiton
            if (CanShoot(SP.Position, ControllingPlayer.MaxShootingForce))
            {
                SP.Weighting += PitchRef.GetShootingChanceScore();
            }

            //Check to See if the supporting player is close

            if (SupportingPlayer)
            {
                float OptimalDisitance = 5.0f;

                Vector2 PassingVector = SP.Position - (Vector2)SupportingPlayer.transform.position;
                float dist = PassingVector.magnitude;

                if (dist < OptimalDisitance)
                {
                   //TODO 
                }

           
            }

            //If the current position has a better score make it so
            if (SP.Weighting > BestScoreSoFar)
            {
                BestScoreSoFar = SP.Weighting;
                BestSupportingSpot = SP;
            }
        }  

        return BestPoition;

    }

    public void ReturnAllFieldPlayersToHome()
    {
        //TODO
    }

    public bool AllPlayersAtHome()
    {
        //TODO
        return true;
    }

    public void ChangeState(GameObject CallingObject, State NewState)
    {
        //TODO    
    }
}
