using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Football : MonoBehaviour {

    //public Steer2D.Seek[] AgentSeek;
    //public Steer2D.Arrive[] AgentArrive;

    //void Start()
    //{
    //    UpdateAgentsTargets();              
    //}

    //void Update()
    //{
    //    UpdateAgentsTargets();
    //}


    //void UpdateAgentsTargets()
    //{

    //    foreach (Steer2D.Seek Seek in AgentSeek)
    //    {
    //        if (Seek != null)
    //        {
    //            Seek.TargetPoint = transform.position;
    //        }
    //    }

    //    foreach (Steer2D.Arrive Arrive in AgentArrive)
    //    {
    //        if (Arrive != null)
    //        {
    //            Arrive.TargetPoint = transform.position;
    //        }
    //    }
    //}

    public bool DebugOn = false;

    Rigidbody2D RB;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    public float CalculateTimeToCoverDistance(Vector3 Target, float Force)
    {
        float TimeToCoverDistance = 0.0f;

       // Vector2 StartingVelocity = RB.velocity;
        //Vector2 StartingForce = (Target - gameObject.transform.position).normalized * Force;
        float Acceleration = Force / RB.mass;
        float Distance = Vector3.Distance(Target, gameObject.transform.position);


        TimeToCoverDistance = Mathf.Sqrt(((2 * Distance) / Acceleration));


        return TimeToCoverDistance;
    }

    public void AddForce(Vector2 ForceVec, string DebugString = "Kicking Ball")
    {
        if (DebugOn)
        {
            Debug.Log(DebugString);
        }

        //print("Kicking Ball");
        RB.velocity = new Vector2(0, 0); //Kicker wont deal with opposing forces already inacting on ball
        RB.AddForce(ForceVec, ForceMode2D.Impulse);
    }
  
}
