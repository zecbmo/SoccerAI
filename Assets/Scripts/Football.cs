using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Football : MonoBehaviour {

    public Steer2D.Seek[] AgentSeek;
    public Steer2D.Arrive[] AgentArrive;

    void Start()
    {
        UpdateAgentsTargets();              
    }

    void Update()
    {
        UpdateAgentsTargets();
    }


    void UpdateAgentsTargets()
    {

        foreach (Steer2D.Seek Seek in AgentSeek)
        {
            if (Seek != null)
            {
                Seek.TargetPoint = transform.position;
            }
        }

        foreach (Steer2D.Arrive Arrive in AgentArrive)
        {
            if (Arrive != null)
            {
                Arrive.TargetPoint = transform.position;
            }
        }
    }

  
}
