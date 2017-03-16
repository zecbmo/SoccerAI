using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour {

    /**
    *   A list Holding all players
    */
    private List<Player> Players = new List<Player>();

    /**
    *   References to Key players
    */
    Player ControllingPlayer = null;
    Player ClosetPlayerToBall = null;
    Player RecievingPlayer = null;
    Player SupportingPlayer = null;

    /**
    *   Reference to the pitch
    */
    Region PitchRef = null;


    public void Init(Region Pitch, List<GameObject> NewPlayers)
    {
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
    }

    void Update()
    {
        Debug.Log(PitchRef.counter);
    }

}
