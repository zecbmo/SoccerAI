using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SupportPosition
{
    public Vector2 Position;
    public float Weighting;
}

public class Region : MonoBehaviour {

    /**
    *   Support Position
    *
    *   This is posiiton on the pitch that players can use to calculate a score on
    *   whether it is a worthwhile position moving to, to supoprt the controlling
    *   player
    */
  

    /*
    * List Of support Positions in the Level     
    */
    public List<SupportPosition> SupportPositions = new List<SupportPosition>();

    /**
    *   Divisons 
    *
    *   Number of Divisions/Supporting Positions there will be on the pitch
    *   Cant Be less than zero
    */
    [SerializeField]
    private int WidthDivisions = 1;
    [SerializeField]
    private int HeightDivisions = 1;

    /**
    *   Default Weigthing
    *
    *   This is the Base score for each positons
    *   However It acts as a visual represntaiton as well for Debuging
    *   (The Radius of the debug circle) 
    */
    [SerializeField]
    private float DefaultWeighting = 0.1f;
    [SerializeField]
    private float SafePassScore = 0.1f;
    [SerializeField]
    private float ShootingChanceScore = 0.2f;


    private SpriteRenderer Rend;

    /**
    *   Bottom Left And Top Right Vectors of the pitch
    */
    private Vector2 BottomLeftVec;
    private Vector2 TopRightVec;

    /**
    *   Players/Agents to be added to each Team
    */
    public List<GameObject> RedTeamPlayers = new List<GameObject>();
    public List<GameObject> BlueTeamPlayers = new List<GameObject>();

    Team RedTeam = new Team();
    Team BlueTeam = new Team();

    /**
    *   Bool monitoring whether the Game is play or not
    */
    private bool GameInPlay = false;

    public int counter = 0;


    void Start ()
    {
        //Get up the Dimensions of the Pitch
        Rend = GetComponent<SpriteRenderer>();
        BottomLeftVec = Rend.bounds.min;
        TopRightVec = Rend.bounds.max;

        //Make Sure that we won't be dividing by Zero
        if ((WidthDivisions <= 0) || (HeightDivisions <= 0))
        {
            Debug.LogError("Divisions in the Region cannot be less than or equal to 0");
            return;
        }

        //Set up the Support positions Regions
        SetUpSupportRegions();

        //Set up the teams
        RedTeam.Init(this, RedTeamPlayers);
        BlueTeam.Init(this, BlueTeamPlayers);


    }

    // Update is called once per frame
    void Update ()
    {
        counter++;
        RedTeam.DetermineBestSupportingPosition();
    }

  

    void SetUpSupportRegions()
    {
        //Calculate the offsets for each axis
        float XOffSet = (Mathf.Abs((TopRightVec.x - BottomLeftVec.x))/ (WidthDivisions+1));
        float YOffSet = (Mathf.Abs((TopRightVec.y - BottomLeftVec.y)) / (HeightDivisions+1));

      //starting point is bootom left of the pitch
        Vector2 CurrentPos = BottomLeftVec;

        //Loop for each Division on Each Axis 
        for (int x = 1; x <= WidthDivisions; x++)
        {    
            for (int y = 1; y <= HeightDivisions; y++)
            {
                //Add the offsets to the current position
                CurrentPos = new Vector2(BottomLeftVec.x+(XOffSet * x), BottomLeftVec.y + (YOffSet *y));

                //Create a Support position at that location
                SupportPosition NewPosition = new SupportPosition();
                NewPosition.Position = CurrentPos;
                NewPosition.Weighting = DefaultWeighting;

                //Add to the support posiiton list
                SupportPositions.Add(NewPosition);
            }             
        }
    }

    /**
     *  Getters and setters  
     */
    public float GetDefaultWeighting()
    {
        return DefaultWeighting;
    }
    public float GetSafePassScore()
    {
        return SafePassScore;
    }
    public float GetShootingChanceScore()
    {
        return ShootingChanceScore;
    }

    void OnDrawGizmos()
    {
        //Draw the Support Possitions based on their current Weightings
        foreach (SupportPosition SP in SupportPositions)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(SP.Position, SP.Weighting);
        }    
    }

    public void SetGameInPlay(bool InPlayOrNot)
    {
        GameInPlay = InPlayOrNot;
    }

    public bool GetGameInPlay()
    {
        return GameInPlay;
    }


    public bool GoalKeeperHasBall()
    {
        //TODO
        return true;
    }


}
